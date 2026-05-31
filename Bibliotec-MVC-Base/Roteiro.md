# Roteiro Completo do Sistema Bibliotec (MVC)

Este documento detalha o passo a passo de todas as funcionalidades implementadas até o momento no projeto Bibliotec-MVC-Base.

---

## 1. Estrutura Inicial e Layout Global

### 1.1 Configuração de Arquivos Estáticos e Layout
*   **O que foi feito**: Limpeza dos arquivos padrões gerados pelo template MVC do .NET e incorporação do design UI/UX fornecido.
*   **Arquivos Criados/Alterados**:
    *   `wwwroot/css/`: Todos os arquivos CSS foram importados (ex: `padrao.css`, `header.css`, `footer.css`, `aside.css`).
    *   `wwwroot/js/`: Todos os scripts front-end foram inseridos (ex: `logado.js`, `login.js`, `home.js`).
    *   `Views/Shared/_Layout.cshtml`: O layout original do .NET foi substituído pelo nosso. Adicionamos a lógica C# `ViewContext.RouteData.Values["controller"]` para exibir o *Header* e *Footer* apenas nas páginas `Home` e `Login` (onde não há menu lateral).
    *   `Views/Shared/_MenuLateral.cshtml`: Criada como uma *Partial View* contendo o menu em `<aside>`. Este menu é carregado nas telas internas (logadas).

### 1.2 Menu Lateral Responsivo
*   **O que foi feito**: A lógica do menu responsivo (abrir e fechar, esmaecer fundo em telas menores) foi unificada para evitar código JS espalhado.
*   **Arquivos Criados/Alterados**:
    *   `wwwroot/js/logado.js`: Contém o script `window.addEventListener('resize')` e funções de *toggle* das classes `menu_lateral_fechado` e `movimento_sombra` para funcionar globalmente.

---

## 2. Banco de Dados e Entidades (Entity Framework)

### 2.1 Criação das Entidades
*   **O que foi feito**: Mapeamento das tabelas do banco de dados para classes C# na pasta `Entities/`.
*   **Arquivos Criados**: `Categoria.cs`, `Livro.cs`, `LivroCategoria.cs`, `Reserva.cs`, `Usuario.cs`.
*   **Detalhes**: Todas as classes foram anotadas com `[Key]`, `[Required]` e propriedades de relacionamento do EF Core (`ICollection`, chaves estrangeiras).

### 2.2 Configuração do Contexto
*   **O que foi feito**: Inicialização do Entity Framework Core.
*   **Arquivos Criados/Alterados**:
    *   `Context/AppDbContext.cs`: Herda de `DbContext` e engloba todos os `DbSet<T>`.
    *   `appsettings.json`: Inserida a string de conexão local do banco de dados.
    *   `Program.cs`: Adicionada a instrução `builder.Services.AddDbContext<AppDbContext>(...)`.

---

## 3. Funcionalidade: Login e Sessão (Arquitetura Completa)

Esta funcionalidade foi implementada utilizando a separação de Interface, Repository e Service.

### 3.1 Camada de Repositório
*   **Arquivos Criados**: `Interfaces/IUsuarioRepository.cs`, `Repositories/UsuarioRepository.cs`.
*   **Detalhes**: O repositório injeta o `AppDbContext` e possui a implementação do método `ObterPorEmail(string email)`, retornando o usuário se existir.

### 3.2 Camada de Serviço
*   **Arquivos Criados**: `Interfaces/IUsuarioService.cs`, `Services/UsuarioService.cs`.
*   **Detalhes**: O Service injeta o Repositório, checa se a senha é válida (em texto plano ou hash futuramente) e avalia o campo `TipoBib`. Se `TipoBib == true`, ele marca a flag de bibliotecária (Admin).

### 3.3 Configuração e Injeção
*   **Arquivos Alterados**: `Program.cs`.
*   **Detalhes**: Registramos `builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>()` e `<IUsuarioService, UsuarioService>()`. Também ativamos a Sessão com `AddSession()` e `UseSession()`.

### 3.4 Controller e View
*   **Arquivos Alterados**: `Controllers/LoginController.cs`, `Views/Login/Index.cshtml`.
*   **Detalhes**: 
    *   O Controller injeta o `IUsuarioService`. O método POST `/Login/Logar` recebe Email e Senha. Se os dados forem válidos, salva as chaves `"Usuario"` e `"Admin"` em `HttpContext.Session`.
    *   A View exibe o form e, em caso de erro, exibe a tag `<p class="msg_erro_login">` recebendo o texto de erro via `ViewBag.Erro`.

---

## 4. Funcionalidade: Gestão de Livros

Diferente do Login, as regras dos Livros foram simplificadas e o `AppDbContext` é injetado diretamente no Controller para agilizar as operações LINQ.

### 4.1 Cadastro de Livros
*   **O que foi feito**: Página de cadastro com seleção de múltiplas categorias. A página é validada via sessão (somente Admin).
*   **Arquivos Criados/Alterados**:
    *   `Controllers/LivroController.cs`: No método POST de Cadastro, o sistema recebe a foto via `IFormFile`, faz o download na pasta `wwwroot/img/livros/`, converte as `CategoriasSelecionadas` (que vêm em uma string separada por vírgula) em uma lista de números, e cadastra os elos na tabela `LivroCategoria`.
    *   `Views/Livro/Cadastro.cshtml`: View contendo o formulário HTML.
    *   `wwwroot/js/cadastroLivro.js`: Um JS específico foi criado para a interface. Sempre que o admin clica em "Inserir" categoria, o JS pega o `value` do `<select>`, cria uma `tag` visual (HTML) na tela e anexa o ID num input hidden. Isso facilita o envio de uma relação N:N em um form tradicional.

### 4.2 Listagem, Modais e Busca
*   **O que foi feito**: Tela com a listagem completa, modais de "Detalhes" e "Editar", barra de pesquisa e exclusão com modal de confirmação.
*   **Arquivos Criados/Alterados**:
    *   `Controllers/LivroController.cs`: O método `Index()` carrega os livros incluindo seus relacionamentos `Include(l => l.LivroCategorias).ThenInclude(lc => lc.Categoria)`.
    *   `Views/Livro/Index.cshtml`: Laço `@foreach` que preenche cada `<tr>`. Os botões possuem atributos `data-...` (ex: `data-titulo`, `data-autor`) guardando os dados de cada linha. Toda a estilização inline foi removida e movida para o CSS.
    *   `wwwroot/js/listalivros.js`: 
        *   **Modais**: O script captura o evento de "click" nos botões, lê os atributos `data-` e preenche os campos do modal (tanto o visual de detalhes quanto os inputs do form de edição).
        *   **Busca em tempo real**: Há um event listener no `<input id="inputBusca">`. O script usa `.includes()` para comparar o valor digitado contra o texto das colunas `<td>`. Ele dá `display: none` em linhas que não correspondem, atuando como um filtro visual.
        *   **SweetAlert2 (Exclusão)**: Substituímos o "confirm" feio do navegador pela lib externa. Ao confirmar a deleção, disparamos um `fetch` (POST) assíncrono pro endpoint `/Livro/Excluir/`.

---

## 5. Funcionalidade: Gestão de Reservas

### 5.1 Listagem e Modais
*   **O que foi feito**: Visão e edição das reservas existentes, listadas numa tabela. O `AppDbContext` é injetado no Controller.
*   **Arquivos Criados/Alterados**:
    *   `Controllers/ReservaController.cs`: Método `Index` consulta as Reservas com `.Include(r => r.Livro)` e `.Include(r => r.Aluno)`. Método POST de `Editar` recebe o status, data e dano.
    *   `Views/Reserva/Index.cshtml`: Tabela de listagem. Adicionadas classes como `.reserva_row`, `.msg_vazio`, removendo todo CSS inline. O form de edição gerencia campos de data, Dano, e o enum/status do empréstimo.
    *   `wwwroot/css/listaReserva.css`: Arquivo criado para hospedar todo o estilo específico das páginas de Reserva, isolando o design.

### 5.2 Filtros Customizados
*   **O que foi feito**: Para a página de reservas, foi solicitada a possibilidade de combinar dois filtros: uma barra de texto e um `<select>` de Status.
*   **Arquivos Criados/Alterados**:
    *   `wwwroot/js/listaReserva.js`: Função `filtrarReservas()`. Ao digitar o nome do Livro ou trocar a opção do `<select>` de Status, o JS verifica se as duas condições são verdadeiras (match do termo *AND* match do status) para então ocultar/exibir as linhas (`.reserva_row`) localmente via JS.
