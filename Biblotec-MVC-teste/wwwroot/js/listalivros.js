// -------------------- Lógica do Modal de Detalhes --------------------
// const botoesDetalhes = document.querySelectorAll('.btn_detalhes');
// const modalDetalhes = document.getElementById('modalDetalhes');
// const fecharModal = document.getElementById('fecharModal');

// botoesDetalhes.forEach(btn => {
//     btn.addEventListener('click', (e) => {
//         e.preventDefault();

//         // Pegar os dados
//         const titulo = btn.getAttribute('data-titulo');
//         const id = btn.getAttribute('data-id');
//         const autor = btn.getAttribute('data-autor');
//         const editora = btn.getAttribute('data-editora');
//         const ano = btn.getAttribute('data-ano');
//         const status = btn.getAttribute('data-status');
//         const categorias = btn.getAttribute('data-categorias');
//         const sinopse = btn.getAttribute('data-sinopse');
//         const imagem = btn.getAttribute('data-imagem');

//         // Preencher o modal
//         document.getElementById('modalTitulo').innerText = titulo;
//         document.getElementById('modalId').innerText = id;
//         document.getElementById('modalAutor').innerText = autor;
//         document.getElementById('modalEditora').innerText = editora;
//         document.getElementById('modalAno').innerText = ano;
//         document.getElementById('modalStatus').innerText = status;
//         document.getElementById('modalCategorias').innerText = categorias || 'Nenhuma';
//         document.getElementById('modalSinopse').innerText = sinopse || 'Sem sinopse disponível.';
//         document.getElementById('modalImagem').src = imagem;

//         // Exibir modal
//         modalDetalhes.style.display = 'flex';
//     });
// });

// if (fecharModal) {
//     fecharModal.addEventListener('click', () => {
//         modalDetalhes.style.display = 'none';
//     });
// }

// -------------------- LÓGICA DO MODAL DE EDIÇÃO --------------------
// const modalEditar = document.getElementById('modalEditar');
// const fecharModalEditar = document.getElementById('fecharModalEditar');
// const botoesEditar = document.querySelectorAll('.btn_editar');

// botoesEditar.forEach(btn => {
//     btn.addEventListener('click', (e) => {
//         e.preventDefault();

//         const id = btn.getAttribute('data-id');
//         const titulo = btn.getAttribute('data-titulo');
//         const autor = btn.getAttribute('data-autor');
//         const editora = btn.getAttribute('data-editora');
//         const ano = btn.getAttribute('data-ano');
//         const status = btn.getAttribute('data-status');
//         const sinopse = btn.getAttribute('data-sinopse');

//         // Categorias
//         const catIdsStr = btn.getAttribute('data-categoria-ids');
//         const catNomesStr = btn.getAttribute('data-categoria-nomes');

//         document.getElementById('editId').value = id;
//         document.getElementById('editTitulo').value = titulo;
//         document.getElementById('editAutor').value = autor;
//         document.getElementById('editEditora').value = editora;
//         document.getElementById('editAnoPublicacao').value = ano;
//         document.getElementById('editSinopse').value = sinopse;

//         // Status (D = Disponível, etc.) -> Ativo ou não.
//         document.getElementById('editAtivo').checked = (status === 'D');

//         // Preencher categorias selecionadas
//         editCatsSelecionadas = [];
//         if (catIdsStr && catNomesStr) {
//             const ids = catIdsStr.split(',');
//             const nomes = catNomesStr.split(',');
//             for(let i=0; i<ids.length; i++) {
//                 if(ids[i]) {
//                     editCatsSelecionadas.push({ id: ids[i].trim(), nome: nomes[i].trim() });
//                 }
//             }
//         }
//         editRenderizarItens();

//         modalEditar.style.display = 'flex';
//     });
// });

// if(fecharModalEditar) {
//     fecharModalEditar.addEventListener('click', () => {
//         modalEditar.style.display = 'none';
//     });
// }

// -------------------- Fechar ao clicar fora dos modais --------------------
// window.addEventListener('click', (e) => {
//     if (e.target === modalDetalhes) {
//         modalDetalhes.style.display = 'none';
//     }
//     if (e.target === modalEditar) {
//         modalEditar.style.display = 'none';
//     }
// });

// -------------------- Lógica de busca --------------------
const inputBusca = document.getElementById('inputBusca');
const linhasTabela = document.querySelectorAll('.container_lista_livros tbody tr');

if (inputBusca) {
    inputBusca.addEventListener('input', function () {
        const termo = this.value.toLowerCase();

        linhasTabela.forEach(linha => {
            if (linha.querySelector('td[colspan]')) return;

            const titulo = linha.querySelector('td[data-cell="Título"]')?.textContent.toLowerCase() || '';
            const categoria = linha.querySelector('td[data-cell="Cat."]')?.textContent.toLowerCase() || '';
            const status = linha.querySelector('td[data-cell="Status"]')?.textContent.toLowerCase() || '';

            if (titulo.includes(termo) || categoria.includes(termo) || status.includes(termo)) {
                linha.style.display = '';
            } else {
                linha.style.display = 'none';
            }
        });
    });
}

// -------------------- Lógica de exclusão com SweetAlert2 --------------------
// const botoesExcluir = document.querySelectorAll('.btn_excluir_livro');
// botoesExcluir.forEach(btn => {
//     btn.addEventListener('click', () => {
//         const id = btn.getAttribute('data-id');

//         Swal.fire({
//             title: 'Tem certeza?',
//             text: "Esta ação não pode ser desfeita!",
//             icon: 'warning',
//             showCancelButton: true,
//             confirmButtonColor: '#3085d6',
//             cancelButtonColor: '#d33',
//             confirmButtonText: 'Sim, excluir!',
//             cancelButtonText: 'Cancelar'
//         }).then((result) => {
//             if (result.isConfirmed) {
//                 fetch(`/Livro/Excluir/${id}`, {
//                     method: 'POST',
//                     headers: {
//                         'Content-Type': 'application/json'
//                     }
//                 }).then(response => {
//                     if (response.ok) {
//                         Swal.fire(
//                             'Excluído!',
//                             'O livro foi excluído com sucesso.',
//                             'success'
//                         ).then(() => {
//                             window.location.reload();
//                         });
//                     } else {
//                         Swal.fire(
//                             'Erro!',
//                             'Ocorreu um problema ao tentar excluir o livro.',
//                             'error'
//                         );
//                     }
//                 }).catch(error => {
//                     Swal.fire('Erro!', 'Não foi possível completar a operação.', 'error');
//                 });
//             }
//         });
//     });
// });

// -------------------- LÓGICA DE CATEGORIAS (EDIÇÃO) --------------------
// const editSelectCategoria = document.getElementById("editCategoria");
// const editBotaoInserir = document.getElementById("edit_btn_inserir");
// const editDivListaCats = document.getElementById("edit_lista_cats");
// const editInputCategoriasSelecionadas = document.getElementById("editCategoriasSelecionadas");

// let editCatsSelecionadas = [];

// function editAdicionarCat() {
//     const valorSelecionado = editSelectCategoria.value;
//     const nomeSelecionado = editSelectCategoria.options[editSelectCategoria.selectedIndex]?.text;

//     if (valorSelecionado === "") {
//         alert("Selecione uma categoria");
//         return;
//     }

//     if (editCatsSelecionadas.find(c => c.id === valorSelecionado)) {
//         alert("Essa categoria já foi selecionada");
//         return;
//     }

//     editCatsSelecionadas.push({ id: valorSelecionado, nome: nomeSelecionado });
//     editSelectCategoria.value = "";
//     editRenderizarItens();
// }

// // -------------------- Renderizar itens Cat --------------------
// function editRenderizarItens() {
//     let template = "";

//     if (editCatsSelecionadas.length > 0) {
//         editCatsSelecionadas.forEach(cat => {
//             template += `
//             <div class="cad_item_cat">
//                 <span class="cad_span_cat">${cat.nome}</span>
//                 <button type="button" class="btn_item_excluir" onclick="editExcluirCat('${cat.id}')">
//                     <svg xmlns="http://www.w3.org/2000/svg" height="1em" viewBox="0 0 384 512">
//                         <path fill="currentColor" d="M342.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L192 210.7 86.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L146.7 256 41.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L192 301.3 297.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L237.3 256 342.6 150.6z"/>
//                     </svg>
//                 </button>
//             </div>`;
//         });
//     } else {
//         template = `<span class="cad_span_cat_vazio" style="color: var(--cor_texto_fraco);">Nenhuma categoria selecionada</span>`;
//     }

//     editDivListaCats.innerHTML = template;
//     editInputCategoriasSelecionadas.value = editCatsSelecionadas.map(c => c.id).join(',');
// }

// // -------------------- Excluir Cat da lista --------------------
// function editExcluirCat(id) {
//     editCatsSelecionadas = editCatsSelecionadas.filter(item => item.id !== id);
//     editRenderizarItens();
// }

// editBotaoInserir.addEventListener("click", editAdicionarCat);