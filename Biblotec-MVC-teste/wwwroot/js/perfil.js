// -------------------- ELEMENTOS DO FORMULÁRIO --------------------
// Select de categorias
const selectCategoria = document.getElementById("categoria");

// Botão para inserir categoria
const botaoInserir = document.getElementById("cad_btn_inserir");

// Div onde serão exibidas as categorias selecionadas
const divListaCats = document.getElementById("cad_lista_cats");

// Formulário
const form = document.querySelector(".form_cad_livro");





// -------------------- BLOQUEIA O RELOAD DO FORM --------------------
// Impede que o formulário recarregue a página
form.addEventListener("submit", function (event) {
    event.preventDefault();
});





// -------------------- ARRAY DE CATEGORIAS DISPONÍVEIS --------------------
const listaOpcoes = [
    "Programação",
    "Desenvolvimento Web",
    "Banco de Dados",
    "Redes",
    "Segurança da Informação",
    "Engenharia de Software",
    "Inteligência Artificial",
    "UX/UI Design",
    "Carreira em TI",
    "Lógica e Algoritmos",
    "Didático",
    "Ficção Científica"
];





// -------------------- ARRAY DE CATEGORIAS SELECIONADAS --------------------
let catsSelecionadas = [];





// -------------------- POPULAR SELECT COM AS OPÇÕES --------------------
function carregarCategorias() {

    listaOpcoes.forEach(opcao => {

        // Cria elemento option
        const option = document.createElement("option");

        // Define texto e valor
        option.value = opcao;
        option.textContent = opcao;

        // Adiciona no select
        selectCategoria.appendChild(option);
    });
}





// -------------------- ADICIONAR CATEGORIA --------------------
function adicionarCat() {

    const valorSelecionado = selectCategoria.value;

    // Verifica se algo foi selecionado
    if (valorSelecionado === "") {
        alert("Selecione uma categoria");
        return;
    }

    // Verifica se a categoria já foi adicionada
    if (catsSelecionadas.includes(valorSelecionado)) {
        alert("Essa categoria já foi selecionada");
        return;
    }

    // Adiciona ao array
    catsSelecionadas.push(valorSelecionado);

    // Limpa o select
    selectCategoria.value = "";

    // Atualiza a lista na tela
    renderizarItens();
}






// -------------------- RENDERIZAR CATEGORIAS NA TELA --------------------
function renderizarItens() {

    let template = "";

    // Se houver categorias selecionadas
    if (catsSelecionadas.length > 0) {

        catsSelecionadas.forEach(cat => {

            template += `
            <div class="cad_item_cat">

                <span class="cad_span_cat">
                    ${cat}
                </span>

                <button 
                    type="button"
                    class="btn_item_excluir"
                    onclick="excluirCat('${cat}')">

                    <svg xmlns="http://www.w3.org/2000/svg" height="1em" viewBox="0 0 384 512">
                        <path d="M342.6 150.6c12.5-12.5 12.5-32.8 
                        0-45.3s-32.8-12.5-45.3 0L192 210.7 
                        86.6 105.4c-12.5-12.5-32.8-12.5-45.3 
                        0s-12.5 32.8 0 45.3L146.7 256 
                        41.4 361.4c-12.5 12.5-12.5 32.8 
                        0 45.3s32.8 12.5 45.3 0L192 
                        301.3 297.4 406.6c12.5 12.5 
                        32.8 12.5 45.3 0s12.5-32.8 
                        0-45.3L237.3 256 342.6 150.6z"/>
                    </svg>

                </button>

            </div>
            `;
        });

    } else {

        // Mensagem quando não houver categorias
        template = `
            <span class="cad_span_cat_vazio">
                Nenhuma categoria cadastrada
            </span>
        `;
    }

    // Atualiza o HTML da div
    divListaCats.innerHTML = template;
}





// -------------------- REMOVER CATEGORIA --------------------
function excluirCat(cat) {

    // Remove o item do array
    catsSelecionadas = catsSelecionadas.filter(item => item !== cat);

    // Atualiza a lista na tela
    renderizarItens();
}





// -------------------- EVENTOS --------------------
// Evento do botão inserir
botaoInserir.addEventListener("click", adicionarCat);





// -------------------- INICIALIZAÇÃO --------------------
// Carrega categorias no select ao abrir a página
carregarCategorias();

// Renderiza a lista inicialmente
renderizarItens();