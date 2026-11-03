// EVO RRHH - utilidades del sitio

(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        initSplash();
        initTheme();
        initSearch();
        initDataTables();
        initBackToTop();
        initCookieBanner();
        initPasswordToggle();
        initCopyButtons();
        initToasts();
        initSidebar();
        initCargaTablas();
        initBotonesCarga();
        initTooltips();
    });

    // SC-011: con "reducir movimiento" activado no se desplaza nada; solo quedan los fundidos.
    function reducirMovimiento() {
        return !!(window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches);
    }
    window.evoReducirMovimiento = reducirMovimiento;

    /* ---------- Splash de carga ---------- */
    function initSplash() {
        var splash = document.getElementById('splashLoader');
        if (!splash) return;
        var ocultar = function () { splash.classList.add('hidden'); };
        if (document.readyState === 'complete') setTimeout(ocultar, 600);
        else window.addEventListener('load', function () { setTimeout(ocultar, 600); });
        // Respaldo por si 'load' tarda demasiado
        setTimeout(ocultar, 3000);
    }

    /* ---------- Modo oscuro ---------- */
    function initTheme() {
        var STORE = 'evo-theme';
        var root = document.documentElement;
        var toggle = document.getElementById('themeToggle');
        if (!toggle) return;

        var saved = localStorage.getItem(STORE);
        var prefiereOscuro = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
        var tema = saved || (prefiereOscuro ? 'dark' : 'light');
        aplicar(tema);

        toggle.addEventListener('click', function () {
            var nuevo = root.getAttribute('data-bs-theme') === 'dark' ? 'light' : 'dark';
            // Fundido corto de toda la página. No anima cada elemento, así que no pesa en las tablas grandes.
            if (document.startViewTransition) document.startViewTransition(function () { aplicar(nuevo); });
            else aplicar(nuevo);
        });

        function aplicar(t) {
            root.setAttribute('data-bs-theme', t);
            try { localStorage.setItem(STORE, t); } catch (e) { }
            var icono = document.getElementById('themeIcon');
            if (icono) icono.className = t === 'dark' ? 'bi bi-sun' : 'bi bi-moon-stars';
            // Avisa a quien lo necesite (p. ej. los gráficos del dashboard) para recolorear sin recargar
            document.dispatchEvent(new CustomEvent('evo:theme', { detail: { tema: t } }));
        }
    }

    /* ---------- Buscador del sitio ---------- */
    function initSearch() {
        var input = document.getElementById('siteSearch');
        if (!input) return;
        input.addEventListener('input', function () {
            var termino = input.value.trim().toLowerCase();
            document.querySelectorAll('[data-search-target]').forEach(function (target) {
                // Las tablas se filtran desde initDataTables (para respetar orden/paginación)
                if (target.matches && target.matches('table')) return;
                var items = target.children || [];
                for (var i = 0; i < items.length; i++) {
                    if (!items[i].matches || !items[i].matches('[data-search-item]')) continue;
                    var txt = items[i].textContent.toLowerCase();
                    items[i].style.display = txt.indexOf(termino) === -1 ? 'none' : '';
                }
            });
        });
    }

    /* ---------- Tablas: orden por columna, paginación y contador ---------- */
    function initDataTables() {
        document.querySelectorAll('table[data-search-target]').forEach(function (table) {
            var tbody = table.querySelector('tbody');
            var thead = table.querySelector('thead');
            if (!tbody || !thead) return;
            var ths = Array.prototype.slice.call(thead.querySelectorAll('th'));
            if (ths.length === 0) return;

            // Si la vista ya trae su propia paginación (servidor), no la duplicamos
            var wrapper = table.closest('.table-responsive');
            var siguiente = wrapper ? wrapper.nextElementSibling : null;
            if (siguiente && siguiente.matches && (siguiente.matches('nav') || siguiente.matches('ul.pagination'))) return;

            // Filas reales; ignora mensajes vacíos (una celda con colspan)
            var rows = Array.prototype.filter.call(tbody.querySelectorAll('tr'), function (r) {
                return r.cells.length > 0 && !r.cells[0].hasAttribute('colspan');
            });
            if (rows.length === 0) return;

            var PAGE_SIZE = 10;
            var state = { sortIdx: -1, sortAsc: true, page: 1 };
            var toolbar = null, countEl = null, pagesEl = null;
            var msgVacio = null;

            function cabeceraTexto(th) {
                return (th.textContent || '').replace(/\s+/g, ' ').trim();
            }

            ths.forEach(function (th, i) {
                var cab = cabeceraTexto(th).replace(/Acciones|Acción/i, '');
                if (!cab) { th.classList.add('nosort'); return; }
                th.classList.add('sortable');
                var si = document.createElement('i');
                si.className = 'bi bi-arrow-down-up si';
                th.appendChild(si);
                th.addEventListener('click', function () {
                    if (th.classList.contains('nosort')) return;
                    if (th.querySelector('button, a:not(.si), input, select')) return;
                    if (state.sortIdx === i) state.sortAsc = !state.sortAsc;
                    else { state.sortIdx = i; state.sortAsc = true; }
                    state.page = 1;
                    aplicar();
                });
            });

            function terminoBusqueda() {
                var input = document.getElementById('siteSearch');
                return input ? input.value.trim().toLowerCase() : '';
            }

            function esNumerica(idx) {
                for (var i = 0; i < rows.length; i++) {
                    var t = (rows[i].cells[idx] ? rows[i].cells[idx].textContent : '').trim();
                    if (t === '') continue;
                    if ((t.replace(/[^\d.,-]/g, '')).length === 0) return false;
                }
                return true;
            }

            function asegurarToolbar() {
                if (toolbar) return;
                toolbar = document.createElement('div');
                toolbar.className = 'dt-toolbar';
                countEl = document.createElement('span');
                countEl.className = 'dt-count';
                pagesEl = document.createElement('div');
                pagesEl.className = 'dt-pages';
                toolbar.appendChild(countEl);
                toolbar.appendChild(pagesEl);
                var ancla = wrapper || table;
                ancla.parentNode.insertBefore(toolbar, ancla.nextSibling);
            }

            function mostrarVacio(on) {
                if (!msgVacio) {
                    msgVacio = document.createElement('tr');
                    msgVacio.className = 'dt-empty';
                    var td = document.createElement('td');
                    td.colSpan = ths.length;
                    td.innerHTML = '<i class="bi bi-inbox"></i>No se encontraron registros';
                    msgVacio.appendChild(td);
                }
                if (on) {
                    if (!msgVacio.parentNode) tbody.appendChild(msgVacio);
                    msgVacio.style.display = '';
                } else if (msgVacio.parentNode) {
                    msgVacio.remove();
                }
            }

            function dibujarPaginas(total, totalPaginas) {
                pagesEl.innerHTML = '';
                if (totalPaginas <= 1) { pagesEl.style.display = 'none'; return; }
                pagesEl.style.display = 'flex';

                var crearBtn = function (etiqueta, activa, deshabilitada, accion) {
                    var b = document.createElement('button');
                    b.type = 'button';
                    b.className = 'dt-page-btn' + (activa ? ' active' : '');
                    b.innerHTML = etiqueta;
                    b.disabled = !!deshabilitada;
                    b.addEventListener('click', accion);
                    pagesEl.appendChild(b);
                };

                crearBtn('<i class="bi bi-chevron-left"></i>', false, state.page <= 1, function () { state.page--; aplicar(); });

                var MAX = 7;
                var desde = 1, hasta = totalPaginas;
                if (totalPaginas > MAX) {
                    desde = Math.max(1, state.page - Math.floor(MAX / 2));
                    hasta = Math.min(totalPaginas, desde + MAX - 1);
                    if (hasta - desde + 1 < MAX) desde = Math.max(1, hasta - MAX + 1);
                }
                for (var p = desde; p <= hasta; p++) {
                    (function (pp) {
                        crearBtn(String(pp), pp === state.page, false, function () { state.page = pp; aplicar(); });
                    })(p);
                }

                crearBtn('<i class="bi bi-chevron-right"></i>', false, state.page >= totalPaginas, function () { state.page++; aplicar(); });
            }

            function aplicar() {
                var termino = terminoBusqueda();
                var lista = rows.slice();

                if (termino) {
                    lista = lista.filter(function (r) {
                        return r.textContent.toLowerCase().indexOf(termino) !== -1;
                    });
                }

                if (state.sortIdx >= 0) {
                    var idx = state.sortIdx;
                    var dir = state.sortAsc ? 1 : -1;
                    var num = esNumerica(idx);
                    lista.sort(function (a, b) {
                        var ta = a.cells[idx] ? a.cells[idx].textContent.trim() : '';
                        var tb = b.cells[idx] ? b.cells[idx].textContent.trim() : '';
                        var r;
                        if (num) {
                            r = (parseFloat(ta.replace(/\./g, '').replace(',', '.')) || 0) -
                                (parseFloat(tb.replace(/\./g, '').replace(',', '.')) || 0);
                        } else {
                            r = ta.localeCompare(tb, 'es');
                        }
                        return r * dir;
                    });
                }

                var totalPaginas = Math.max(1, Math.ceil(lista.length / PAGE_SIZE));
                if (state.page > totalPaginas) state.page = totalPaginas;
                var inicio = (state.page - 1) * PAGE_SIZE;
                var visibles = lista.slice(inicio, inicio + PAGE_SIZE);

                rows.forEach(function (r) {
                    r.style.display = visibles.indexOf(r) === -1 ? 'none' : '';
                });
                mostrarVacio(lista.length === 0);

                var desde = lista.length === 0 ? 0 : inicio + 1;
                var hasta = Math.min(inicio + PAGE_SIZE, lista.length);
                if (countEl) countEl.textContent = 'Mostrando ' + desde + '–' + hasta + ' de ' + lista.length;
                dibujarPaginas(lista.length, totalPaginas);

                ths.forEach(function (th, i) {
                    var ic = th.querySelector('.si');
                    if (!ic) return;
                    if (i === state.sortIdx) {
                        ic.className = 'bi si bi-caret-' + (state.sortAsc ? 'up' : 'down') + '-fill';
                        th.classList.add(state.sortAsc ? 'sort-asc' : 'sort-desc');
                    } else {
                        ic.className = 'bi bi-arrow-down-up si';
                        th.classList.remove('sort-asc', 'sort-desc');
                    }
                });
            }

            var inputGlobal = document.getElementById('siteSearch');
            if (inputGlobal) {
                inputGlobal.addEventListener('input', function () { state.page = 1; aplicar(); });
            }

            asegurarToolbar();
            aplicar();
        });
    }

    /* ---------- Botón volver arriba ---------- */
    function initBackToTop() {
        var btn = document.getElementById('backToTop');
        if (!btn) return;

        function onScroll() {
            if (window.scrollY > 300) btn.classList.add('show');
            else btn.classList.remove('show');
        }
        window.addEventListener('scroll', onScroll, { passive: true });
        onScroll();
        btn.addEventListener('click', function () {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }

    /* ---------- Banner de cookies ---------- */
    function initCookieBanner() {
        var banner = document.getElementById('cookieBanner');
        var aceptar = document.getElementById('cookieAccept');
        if (!banner) return;
        if (localStorage.getItem('evo-cookies') === 'aceptadas') return;
        banner.classList.add('show');
        if (aceptar) aceptar.addEventListener('click', function () {
            localStorage.setItem('evo-cookies', 'aceptadas');
            banner.classList.remove('show');
        });
    }

    /* ---------- Mostrar/ocultar contraseña ---------- */
    function initPasswordToggle() {
        document.querySelectorAll('[data-toggle-password]').forEach(function (btn) {
            btn.addEventListener('click', function () {
                var input = document.querySelector(btn.getAttribute('data-toggle-password'));
                if (!input) return;
                var visible = input.type === 'text';
                input.type = visible ? 'password' : 'text';
                btn.textContent = visible ? 'Mostrar' : 'Ocultar';
            });
        });
    }

    /* ---------- Botón copiar ---------- */
    function initCopyButtons() {
        if (!navigator.clipboard || !window.isSecureContext) return;

        document.querySelectorAll('[data-copy-target]').forEach(function (btn) {
            btn.addEventListener('click', function () {
                var origen = document.querySelector(btn.getAttribute('data-copy-target'));
                if (!origen) return;
                var texto = origen.tagName === 'INPUT' || origen.tagName === 'TEXTAREA' ? origen.value : origen.textContent;
                navigator.clipboard.writeText(texto.trim()).then(function () {
                    var original = btn.textContent;
                    btn.textContent = '\u2713 Copiado';
                    setTimeout(function () { btn.textContent = original; }, 1500);
                });
            });
        });
    }

    /* ---------- Toasts (aviso global) ----------
       Los mensajes de TempData llegan como alerta (.alert-auto). Con JavaScript se muestran como toast;
       sin él, la alerta queda visible en la página. */
    function initToasts() {
        document.querySelectorAll('.alert-auto').forEach(function (alerta) {
            var tipo = alerta.classList.contains('alert-success') ? 'success'
                : alerta.classList.contains('alert-danger') ? 'danger'
                : alerta.classList.contains('alert-warning') ? 'warning' : 'info';
            var texto = alerta.textContent.trim();
            alerta.remove();
            evoToast(texto, tipo);
        });
    }

    window.evoToast = function (mensaje, tipo, delayPersonalizado) {
        tipo = tipo || 'info';
        var host = document.getElementById('toastHost');
        if (!host || !mensaje) return;
        var iconos = {
            success: 'bi-check-circle-fill',
            danger: 'bi-exclamation-triangle-fill',
            warning: 'bi-exclamation-circle-fill',
            info: 'bi-info-circle-fill'
        };
        var restante = delayPersonalizado || (tipo === 'danger' ? 7000 : 4000);

        var el = document.createElement('div');
        el.className = 'evo-toast evo-toast-' + tipo + ' entrando';
        el.setAttribute('role', tipo === 'danger' ? 'alert' : 'status');
        var icono = document.createElement('i');
        icono.className = 'bi ' + (iconos[tipo] || iconos.info) + ' evo-toast-icono';
        icono.setAttribute('aria-hidden', 'true');
        var texto = document.createElement('div');
        texto.className = 'evo-toast-texto';
        texto.textContent = mensaje; // texto plano: el mensaje puede incluir datos ingresados por el usuario
        var cerrarBtn = document.createElement('button');
        cerrarBtn.type = 'button';
        cerrarBtn.className = 'btn-close';
        cerrarBtn.setAttribute('aria-label', 'Cerrar aviso');
        el.appendChild(icono);
        el.appendChild(texto);
        el.appendChild(cerrarBtn);
        host.appendChild(el);
        void el.offsetHeight; // fija el estado inicial para que la entrada tenga transición
        el.classList.remove('entrando');

        // El tiempo se pausa con el mouse o el foco encima y con la pestaña oculta
        var timer = null, inicio = 0, cerrado = false;
        function correr() { if (cerrado || timer) return; inicio = Date.now(); timer = setTimeout(cerrar, restante); }
        function pausar() { if (!timer) return; clearTimeout(timer); timer = null; restante -= Date.now() - inicio; }
        function cerrar() {
            if (cerrado) return;
            cerrado = true;
            clearTimeout(timer);
            document.removeEventListener('visibilitychange', alCambiarVisibilidad);
            el.classList.add('saliendo');
            setTimeout(function () { el.remove(); }, 160);
        }
        function alCambiarVisibilidad() { if (document.hidden) pausar(); else correr(); }
        el.addEventListener('mouseenter', pausar);
        el.addEventListener('mouseleave', correr);
        el.addEventListener('focusin', pausar);
        el.addEventListener('focusout', correr);
        cerrarBtn.addEventListener('click', cerrar);
        document.addEventListener('visibilitychange', alCambiarVisibilidad);
        if (!document.hidden) correr();
    };

    /* ---------- Menú móvil (sidebar) ---------- */
    function initSidebar() {
        var sidebar = document.getElementById('sidebar');
        var backdrop = document.getElementById('sidebarBackdrop');
        var btn = document.getElementById('btnMenu');
        if (!sidebar || !backdrop || !btn) return;

        function abrir() {
            sidebar.classList.add('open');
            backdrop.classList.add('open');
        }
        function cerrar() {
            sidebar.classList.remove('open');
            backdrop.classList.remove('open');
        }
        btn.addEventListener('click', abrir);
        backdrop.addEventListener('click', cerrar);
        // Cierra al navegar (click en enlace del sidebar) en móvil
        sidebar.querySelectorAll('a.sidebar-link, a.sidebar-sublink').forEach(function (enlace) {
            enlace.addEventListener('click', function () {
                if (window.innerWidth < 992) cerrar();
            });
        });
    }

    /* ---------- SC-011: skeleton de tablas ----------
       Al filtrar (formulario GET) o cambiar de página, la tabla actual se cubre con filas de muestra
       hasta que llega la página nueva. Solo agrega una capa encima: no recorre ni modifica las filas. */
    function initCargaTablas() {
        var tablas = document.querySelectorAll('main .table-responsive');
        if (tablas.length === 0) return;
        var espera = null;

        function cubrir(wrap) {
            if (wrap.querySelector('.evo-skeleton')) return;
            var capa = document.createElement('div');
            capa.className = 'evo-skeleton';
            capa.setAttribute('aria-hidden', 'true');
            var cab = document.createElement('div');
            cab.className = 'evo-skeleton-cabecera';
            capa.appendChild(cab);
            var anchos = [['22%', '30%', '14%', '18%'], ['18%', '26%', '20%', '12%'], ['24%', '22%', '16%', '20%']];
            var filas = Math.max(3, Math.min(16, Math.floor((wrap.clientHeight - 38) / 41)));
            for (var i = 0; i < filas; i++) {
                var fila = document.createElement('div');
                fila.className = 'evo-skeleton-fila';
                anchos[i % anchos.length].forEach(function (w) {
                    var barra = document.createElement('span');
                    barra.className = 'evo-skeleton-barra';
                    barra.style.width = w;
                    fila.appendChild(barra);
                });
                capa.appendChild(fila);
            }
            wrap.classList.add('cargando');
            wrap.setAttribute('aria-busy', 'true');
            wrap.appendChild(capa);
            void capa.offsetHeight;
            capa.classList.add('visible');
        }
        function quitar() {
            clearTimeout(espera);
            tablas.forEach(function (wrap) {
                var capa = wrap.querySelector('.evo-skeleton');
                if (capa) capa.remove();
                wrap.classList.remove('cargando');
                wrap.removeAttribute('aria-busy');
            });
        }
        // Espera 150 ms: si la página llega antes, no aparece ningún parpadeo
        function programar() { clearTimeout(espera); espera = setTimeout(function () { tablas.forEach(cubrir); }, 150); }

        document.addEventListener('submit', function (e) {
            var form = e.target;
            if (e.defaultPrevented || !form.closest || !form.closest('main')) return;
            if ((form.getAttribute('method') || 'get').toLowerCase() !== 'get') return;
            programar();
        });
        document.addEventListener('click', function (e) {
            var a = e.target.closest && e.target.closest('main .pagination a.page-link[href]');
            if (!a || e.defaultPrevented || e.button !== 0 || e.ctrlKey || e.metaKey || e.shiftKey || e.altKey) return;
            programar();
        });
        // Al volver con el botón Atrás la página sale de la caché con la capa puesta
        window.addEventListener('pageshow', function (e) { if (e.persisted) quitar(); });
    }

    /* ---------- SC-011: botones con estado de carga ----------
       <button type="submit" data-cargando="Entrando…"> muestra un spinner al enviar el formulario. */
    function initBotonesCarga() {
        document.addEventListener('submit', function (e) {
            if (e.defaultPrevented) return;
            var btn = e.target.querySelector && e.target.querySelector('button[type="submit"][data-cargando]');
            if (!btn || btn.classList.contains('btn-cargando')) return;
            btn.dataset.textoOriginal = btn.innerHTML;
            btn.classList.add('btn-cargando');
            btn.setAttribute('aria-disabled', 'true');
            btn.innerHTML = '<span class="spinner-border me-2" aria-hidden="true"></span>';
            btn.appendChild(document.createTextNode(btn.dataset.cargando));
        });
        window.addEventListener('pageshow', function (e) {
            if (!e.persisted) return;
            document.querySelectorAll('.btn-cargando[data-cargando]').forEach(function (btn) {
                btn.innerHTML = btn.dataset.textoOriginal;
                btn.classList.remove('btn-cargando');
                btn.removeAttribute('aria-disabled');
            });
        });
    }

    /* ---------- Tooltips ---------- */
    function initTooltips() {
        document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
            bootstrap.Tooltip.getOrCreateInstance(el);
        });
    }

    /* ---------- Spinner de carga (uso: window.evoShowLoading()) ---------- */
    window.evoShowLoading = function (visible) {
        var spinner = document.getElementById('loadingSpinner');
        if (spinner) spinner.classList.toggle('show', visible);
    };
    /* ---------- SC-008: rangos de fechas y horas en formularios ----------
       <input data-fecha-desde="FechaInicio"> no admite una fecha anterior a la de FechaInicio.
       <input data-hora-desde="Hora_Sal"> exige una hora posterior cuando el permiso es de un solo día.
       El servidor vuelve a validar lo mismo (por si el JavaScript está desactivado). */
    function initRangos() {
        document.querySelectorAll('[data-fecha-desde]').forEach(function (fin) {
            var inicio = fin.form && fin.form.querySelector('[name="' + fin.dataset.fechaDesde + '"]');
            if (!inicio) return;
            function actualizar() {
                fin.setCustomValidity(inicio.value && fin.value && fin.value < inicio.value
                    ? 'La fecha de fin no puede ser anterior a la fecha de inicio.' : '');
                if (!fin.validationMessage) fin.classList.remove('is-invalid');
            }
            inicio.addEventListener('change', actualizar);
            fin.addEventListener('change', actualizar);
            actualizar();
        });
        document.querySelectorAll('[data-hora-desde]').forEach(function (ret) {
            var form = ret.form;
            var sal = form && form.querySelector('[name="' + ret.dataset.horaDesde + '"]');
            if (!sal) return;
            var ini = form.querySelector('[name="FechaInicio"]');
            var fin = form.querySelector('[name="FechaFin"]');
            function actualizar() {
                var mismoDia = !fin || !fin.value || (ini && fin.value === ini.value);
                ret.setCustomValidity(mismoDia && sal.value && ret.value && ret.value <= sal.value
                    ? 'La hora de retorno debe ser posterior a la hora de salida.' : '');
                if (!ret.validationMessage) ret.classList.remove('is-invalid');
            }
            [sal, ret, ini, fin].forEach(function (el) { if (el) el.addEventListener('change', actualizar); });
            actualizar();
        });
    }
    // jquery.validate pone novalidate en el formulario, así que también se revisa al enviar
    // y se muestra el mensaje en el <span asp-validation-for> del campo.
    document.addEventListener('submit', function (e) {
        var form = e.target;
        var invalidos = form.querySelectorAll ? form.querySelectorAll('[data-fecha-desde], [data-hora-desde]') : [];
        var primero = null;
        invalidos.forEach(function (el) {
            var span = form.querySelector('[data-valmsg-for="' + el.name + '"]');
            if (el.validationMessage) {
                if (span) { span.textContent = el.validationMessage; span.classList.add('field-validation-error'); }
                el.classList.add('is-invalid');
                primero = primero || el;
            } else if (span && span.dataset.rango === '1') {
                span.textContent = '';
            }
            if (span) span.dataset.rango = el.validationMessage ? '1' : '';
        });
        if (primero) { e.preventDefault(); e.stopImmediatePropagation(); primero.focus(); }
    }, true);
    document.addEventListener('DOMContentLoaded', initRangos);
})();