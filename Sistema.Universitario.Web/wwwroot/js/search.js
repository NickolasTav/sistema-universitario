window.appSearch = (function () {
    async function fetchResults(controller, q) {
        const url = `/${controller}/Search?q=${encodeURIComponent(q || '')}`;
        const res = await fetch(url, { credentials: 'same-origin' });
        if (!res.ok) return [];
        return await res.json();
    }

    function renderCards(container, items, templateFn) {
        container.innerHTML = items.map(templateFn).join('\n');
    }

    function getProp(item, name) {
        if (!item) return '';
        return item[name] ?? item[name.charAt(0).toLowerCase() + name.slice(1)] ?? '';
    }

    function defaultTemplate(item, controller) {
        const id = getProp(item, 'Id');
        if (controller === 'Alunos') {
            const nome = getProp(item, 'Nome');
            const matricula = getProp(item, 'Matricula');
            const curso = getProp(item, 'CursoNome');
            const initials = (nome || '?').charAt(0).toUpperCase();
            return `
            <div class="card-custom">
                <div style="display:flex;gap:12px;align-items:center;">
                    <div class="card-avatar">${escapeHtml(initials)}</div>
                    <div>
                        <div class="card-title-custom">${escapeHtml(nome)}</div>
                        <div class="card-subtitle-custom">Matrícula: ${escapeHtml(matricula)}</div>
                        <div class="card-subtitle-custom">Curso: ${escapeHtml(curso)}</div>
                    </div>
                </div>
                <div class="card-actions">
                    <a class="btn btn-sm btn-info" href="/${controller}/Details/${id}">Detalhes</a>
                    <a class="btn btn-sm btn-secondary" href="/${controller}/Edit/${id}">Editar</a>
                    <a class="btn btn-sm btn-danger" href="/${controller}/Delete/${id}">Remover</a>
                </div>
            </div>`;
        }
        if (controller === 'Materias') {
            const nome = getProp(item, 'Nome');
            const curso = getProp(item, 'CursoNome');
            const professor = getProp(item, 'ProfessorNome');
            const initials = (nome || '?').charAt(0).toUpperCase();
            return `
            <div class="card-custom">
                <div style="display:flex;gap:12px;align-items:center;">
                    <div class="card-avatar">${escapeHtml(initials)}</div>
                    <div>
                        <div class="card-title-custom">${escapeHtml(nome)}</div>
                        <div class="card-subtitle-custom">Curso: ${escapeHtml(curso)}</div>
                        <div class="card-subtitle-custom">Professor: ${escapeHtml(professor)}</div>
                    </div>
                </div>
                <div class="card-actions">
                    <a class="btn btn-sm btn-info" href="/${controller}/Details/${id}">Detalhes</a>
                    <a class="btn btn-sm btn-secondary" href="/${controller}/Edit/${id}">Editar</a>
                    <a class="btn btn-sm btn-danger" href="/${controller}/Delete/${id}">Remover</a>
                </div>
            </div>`;
        }
        // generic fallback for other controllers
        const nome = getProp(item, 'Nome');
        const initials = (nome || '?').charAt(0).toUpperCase();
        return `
            <div class="card-custom">
                <div style="display:flex;gap:12px;align-items:center;">
                    <div class="card-avatar">${escapeHtml(initials)}</div>
                    <div>
                        <div class="card-title-custom">${escapeHtml(nome)}</div>
                    </div>
                </div>
                <div class="card-actions">
                    <a class="btn btn-sm btn-info" href="/${controller}/Details/${id}">Detalhes</a>
                    <a class="btn btn-sm btn-secondary" href="/${controller}/Edit/${id}">Editar</a>
                    <a class="btn btn-sm btn-danger" href="/${controller}/Delete/${id}">Remover</a>
                </div>
            </div>`;
    }

    function escapeHtml(s) {
        if (!s) return '';
        return s.replace(/[&<>"']/g, function (c) { return ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'})[c]; });
    }

    function attach(controller) {
        const input = document.getElementById('searchBox');
        const container = document.querySelector('.cards-grid');
        if (!input || !container) return;

        let timeout = null;
        input.addEventListener('input', function () {
            clearTimeout(timeout);
            timeout = setTimeout(async () => {
                const q = input.value;
                const items = await fetchResults(controller, q);
                renderCards(container, items, (it) => defaultTemplate(it, controller));
            }, 250);
        });
    }

    return { attach };
})();
