// Lógica dos Modais
const modalDetalhes = document.getElementById('modalDetalhes');
const fecharModalDetalhes = document.getElementById('fecharModalDetalhes');
const botoesDetalhes = document.querySelectorAll('.btn_detalhes_reserva');

const modalEditar = document.getElementById('modalEditar');
const fecharModalEditar = document.getElementById('fecharModalEditar');
const botoesEditar = document.querySelectorAll('.btn_editar_reserva');

// Detalhes
botoesDetalhes.forEach(btn => {
    btn.addEventListener('click', (e) => {
        e.preventDefault();
        
        document.getElementById('detId').innerText = btn.getAttribute('data-id');
        document.getElementById('detLivro').innerText = btn.getAttribute('data-livro');
        document.getElementById('detAluno').innerText = btn.getAttribute('data-aluno');
        document.getElementById('detDataReserva').innerText = btn.getAttribute('data-datareserva');
        document.getElementById('detDataEmprestimo').innerText = btn.getAttribute('data-dataemprestimo') || 'N/A';
        document.getElementById('detDataDevolucao').innerText = btn.getAttribute('data-datadevolucao') || 'N/A';
        document.getElementById('detStatus').innerText = btn.getAttribute('data-status');
        document.getElementById('detDano').innerText = btn.getAttribute('data-dano') || 'Nenhum dano registrado.';

        modalDetalhes.style.display = 'flex';
    });
});

// Edição
botoesEditar.forEach(btn => {
    btn.addEventListener('click', (e) => {
        e.preventDefault();
        
        document.getElementById('editId').value = btn.getAttribute('data-id');
        document.getElementById('editDataEmprestimo').value = btn.getAttribute('data-dataemprestimo');
        document.getElementById('editDataDevolucao').value = btn.getAttribute('data-datadevolucao');
        document.getElementById('editStatus').value = btn.getAttribute('data-status');
        document.getElementById('editDano').value = btn.getAttribute('data-dano');

        modalEditar.style.display = 'flex';
    });
});

// Fechar modais
if(fecharModalDetalhes) fecharModalDetalhes.addEventListener('click', () => modalDetalhes.style.display = 'none');
if(fecharModalEditar) fecharModalEditar.addEventListener('click', () => modalEditar.style.display = 'none');

// Filtros
const filtroTitulo = document.getElementById('filtroTitulo');
const filtroStatus = document.getElementById('filtroStatus');
const linhasReserva = document.querySelectorAll('.reserva_row');

function filtrarReservas() {
    const termoBusca = filtroTitulo.value.toLowerCase();
    const statusSelecionado = filtroStatus.value;

    linhasReserva.forEach(linha => {
        const tituloCell = linha.querySelector('td[data-cell="Título"]');
        const btnEditar = linha.querySelector('.btn_editar_reserva');
        
        if (tituloCell && btnEditar) {
            const titulo = tituloCell.innerText.toLowerCase();
            const status = btnEditar.getAttribute('data-status');

            const exibePorTitulo = titulo.includes(termoBusca);
            const exibePorStatus = statusSelecionado === "" || status === statusSelecionado;

            if (exibePorTitulo && exibePorStatus) {
                linha.style.display = '';
            } else {
                linha.style.display = 'none';
            }
        }
    });
}

if (filtroTitulo && filtroStatus) {
    filtroTitulo.addEventListener('input', filtrarReservas);
    filtroStatus.addEventListener('change', filtrarReservas);
}

window.addEventListener('click', (e) => {
    if (e.target === modalDetalhes) modalDetalhes.style.display = 'none';
    if (e.target === modalEditar) modalEditar.style.display = 'none';
});
