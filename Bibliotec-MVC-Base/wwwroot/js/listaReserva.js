// Lógica dos Modais
const modalDetalhes = document.getElementById('modalDetalhes');
const fecharModalDetalhes = document.getElementById('fecharModalDetalhes');
const botoesDetalhes = document.querySelectorAll('.btn_detalhes_reserva');

const modalEditar = document.getElementById('modalEditar');
const fecharModalEditar = document.getElementById('fecharModalEditar');
const botoesEditar = document.querySelectorAll('.btn_editar_reserva');

// Funções de máscara para celular
function mascararCelular(num) {
    if (!num) return "N/A";
    if (num.length === 11) {
        return `(${num.substring(0, 2)}) *****-${num.substring(7)}`;
    }
    return "*******";
}

function formatarCelular(num) {
    if (!num) return "N/A";
    if (num.length === 11) {
        return `(${num.substring(0, 2)}) ${num.substring(2, 7)}-${num.substring(7)}`;
    }
    return num;
}

// Detalhes
botoesDetalhes.forEach(btn => {
    btn.addEventListener('click', (e) => {
        e.preventDefault();
        
        document.getElementById('detId').innerText = btn.getAttribute('data-id');
        document.getElementById('detLivro').innerText = btn.getAttribute('data-livro');
        document.getElementById('detAluno').innerText = btn.getAttribute('data-aluno');
        
        const celularRaw = btn.getAttribute('data-celular');
        const detCelular = document.getElementById('detCelular');
        if (detCelular) {
            detCelular.setAttribute('data-raw', celularRaw || "");
            detCelular.setAttribute('data-hidden', 'true');
            detCelular.innerText = mascararCelular(celularRaw);
        }

        document.getElementById('detDataReserva').innerText = btn.getAttribute('data-datareserva');
        document.getElementById('detDataEmprestimo').innerText = btn.getAttribute('data-dataemprestimo') || 'N/A';
        document.getElementById('detDataDevolucao').innerText = btn.getAttribute('data-datadevolucao') || 'N/A';
        document.getElementById('detStatus').innerText = btn.getAttribute('data-status');
        document.getElementById('detDano').innerText = btn.getAttribute('data-dano') || 'Nenhum dano registrado.';

        modalDetalhes.style.display = 'flex';
    });
});

// Lógica para alternar visibilidade do celular
const toggleCelular = document.getElementById('toggleCelular');
if (toggleCelular) {
    toggleCelular.addEventListener('click', () => {
        const detCelular = document.getElementById('detCelular');
        if (!detCelular) return;
        
        const isHidden = detCelular.getAttribute('data-hidden') === 'true';
        const raw = detCelular.getAttribute('data-raw');
        
        if (isHidden) {
            detCelular.innerText = formatarCelular(raw);
            detCelular.setAttribute('data-hidden', 'false');
        } else {
            detCelular.innerText = mascararCelular(raw);
            detCelular.setAttribute('data-hidden', 'true');
        }
    });
}

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
        
        if (tituloCell) {
            const titulo = tituloCell.innerText.toLowerCase();
            const status = linha.getAttribute('data-status');

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

// Lógica de exclusão com SweetAlert2
const botoesExcluir = document.querySelectorAll('.btn_excluir_reserva');
botoesExcluir.forEach(btn => {
    btn.addEventListener('click', () => {
        const id = btn.getAttribute('data-id');
        
        Swal.fire({
            title: 'Tem certeza?',
            text: "Deseja excluir esta reserva? Esta ação não pode ser desfeita!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sim, excluir!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(`/Reserva/Excluir/${id}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Excluído!',
                            'A reserva foi excluída com sucesso.',
                            'success'
                        ).then(() => {
                            window.location.reload();
                        });
                    } else {
                        Swal.fire(
                            'Erro!',
                            'Ocorreu um problema ao tentar excluir a reserva.',
                            'error'
                        );
                    }
                }).catch(error => {
                    Swal.fire('Erro!', 'Não foi possível completar a operação.', 'error');
                });
            }
        });
    });
});
