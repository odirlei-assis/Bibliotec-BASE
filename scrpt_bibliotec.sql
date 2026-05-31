--==================== DDL ====================
CREATE DATABASE Bibliotec;
GO
--DROP DATABASE Bibliotec

USE Bibliotec;
GO

-- Tabela Base – Usuario
CREATE TABLE Usuario (
    id INT IDENTITY (1, 1) PRIMARY KEY,
    matricula VARCHAR(20) NOT NULL UNIQUE,
    ativo BIT NOT NULL,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    senha VARCHAR(50) NOT NULL,
    numCel VARCHAR(11) NOT NULL,
    tipoBib BIT NOT NULL
);
GO

-- Tabela Categoria
CREATE TABLE Categoria (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);
GO

-- Tabela Livro
CREATE TABLE Livro (
    id INT IDENTITY (1, 1) PRIMARY KEY,
    titulo VARCHAR(150) NOT NULL,
    autor VARCHAR(100) NOT NULL,
    anoPublicacao INT NOT NULL,
    status CHAR(1) NOT NULL
        CHECK (status IN ('D', 'E', 'I')),
    sinopse VARCHAR(MAX),
    imagem VARCHAR(MAX),
    editora VARCHAR(50) NOT NULL
);
GO

--DROP TABLE Livro

-- Tabela Livro — Categoria (N:N)
CREATE TABLE LivroCategoria (
    livroId INT NOT NULL FOREIGN KEY (livroId) REFERENCES Livro(id) ON DELETE CASCADE,
    categoriaId INT NOT NULL FOREIGN KEY (categoriaId) REFERENCES Categoria(id) ON DELETE CASCADE
);
GO

-- Tabela Reserva (N:N)
CREATE TABLE Reserva (
    id INT IDENTITY (1, 1) PRIMARY KEY,
    dataReserva DATE NOT NULL,
    dataEmprestimo DATE,
    dataPrevistaDevolucao DATE,
    danoLivro VARCHAR(255),
    status CHAR(1) NOT NULL
        CHECK (status IN ('E', 'P', 'A', 'F')),-- E = ESPERA, P = POSSE ,A = ATRASO, F = FINALIZADA
    alunoId INT NOT NULL FOREIGN KEY (alunoId) REFERENCES Usuario (id) ON DELETE CASCADE,
    livroId INT NOT NULL FOREIGN KEY (livroId) REFERENCES Livro (id) ON DELETE CASCADE,
    CONSTRAINT UQ_Reserva UNIQUE (alunoId, livroId, dataReserva)
);
GO
--DROP TABLE Reserva


--==================== DML ====================
-- USUARIOS - tipoBib: 0 = aluno | 1 = bibliotecaria
INSERT INTO Usuario (matricula, ativo, nome, email, senha, numCel, tipoBib) VALUES
('MAT001', 1, 'João Silva', 'joao@email.com', '123456', '11999990001', 0),
('MAT002', 1, 'Maria Souza', 'maria@email.com', '123456', '11999990002', 0),
('MAT003', 1, 'Carlos Lima', 'carlos@email.com', '123456', '11999990003', 0),
('FUNC001', 1, 'Ana Costa', 'ana@email.com', '123456', '11999990004', 1),
('FUNC002', 1, 'Fernanda Alves', 'fernanda@email.com', '123456', '11999990005', 1);
GO

-- CATEGORIAS
INSERT INTO Categoria (nome) VALUES
('Tecnologia'),
('Romance'),
('Ficção Científica'),
('História'),
('Educação');
GO

-- LIVROS - status: D = disponivel | E = emprestado | I = indisponivel
INSERT INTO Livro (titulo, autor, anoPublicacao, status, sinopse, imagem, editora) VALUES
('Clean Code', 'Robert C. Martin', 2008, 'D', 'Boas práticas de programação', '', 'Prentice Hall'),
('Dom Casmurro', 'Machado de Assis', 1899, 'E', 'Clássico da literatura brasileira', '', 'Editora Globo'),
('1984', 'George Orwell', 1949, 'D', 'Distopia política', '', 'Secker & Warburg'),
('Sapiens', 'Yuval Noah Harari', 2011, 'I', 'História da humanidade', '', 'Harper'),
('O Pequeno Príncipe', 'Antoine de Saint-Exupéry', 1943, 'D', 'Fábula filosófica', '', 'Reynal & Hitchcock');
GO

-- LIVRO x CATEGORIA
INSERT INTO LivroCategoria (livroId, categoriaId) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 2),
(5, 5);
GO

-- RESERVAS - (somente usuários tipoBib = 0)
INSERT INTO Reserva (dataReserva, dataEmprestimo, dataPrevistaDevolucao, danoLivro, status, alunoId, livroId) VALUES
('2026-04-01', '2026-04-02', '2026-04-15', NULL, 'F', 1, 1),
('2026-04-01', '2026-04-02', '2026-04-15', NULL, 'A', 1, 2),
('2026-04-03', '2026-04-04', '2026-06-30', 'Capa levemente danificada', 'P', 2, 3),
('2026-04-05', NULL, NULL, NULL, 'E', 3, 1);



--==================== DQL ====================
-- Listar todos os usuários
SELECT * FROM Usuario;

-- Listar apenas alunos
SELECT *
FROM Usuario
WHERE tipoBib = 0;

-- Listar apenas bibliotecárias
SELECT *
FROM Usuario
WHERE tipoBib = 1;

-- Listar todos os livros
SELECT * FROM Livro;

-- Livros com descrição do status
SELECT id, titulo, autor, anoPublicacao,
    CASE 
        WHEN status = 'D' THEN 'Disponível'
        WHEN status = 'E' THEN 'Emprestado'
        WHEN status = 'I' THEN 'Indisponível'
    END AS statusDescricao
FROM Livro;

-- Livros com suas categorias
SELECT 
    L.titulo,
    C.nome AS categoria
FROM Livro L
INNER JOIN LivroCategoria LC ON LC.livroId = L.id
INNER JOIN Categoria C ON C.id = LC.categoriaId
ORDER BY L.titulo;

-- Livros com múltiplas categorias
SELECT 
    L.titulo,
    STRING_AGG(C.nome, ', ') AS categorias
FROM Livro L
INNER JOIN LivroCategoria LC ON LC.livroId = L.id
INNER JOIN Categoria C ON C.id = LC.categoriaId
GROUP BY L.titulo;

-- Reservas com dados completos
SELECT 
    R.id,
    U.nome AS aluno,
    L.titulo AS livro,
    R.dataReserva,
    R.dataEmprestimo,
    R.dataPrevistaDevolucao,
    R.danoLivro
FROM Reserva R
INNER JOIN Usuario U ON U.id = R.alunoId
INNER JOIN Livro L ON L.id = R.livroId;

-- Reservas em aberto
SELECT 
    R.id,
    U.nome,
    L.titulo,
    R.dataReserva
FROM Reserva R
INNER JOIN Usuario U ON U.id = R.alunoId
INNER JOIN Livro L ON L.id = R.livroId
WHERE R.dataEmprestimo IS NULL;

-- Livros emprestados
SELECT 
    L.titulo,
    U.nome AS aluno,
    R.dataEmprestimo,
    R.dataPrevistaDevolucao
FROM Reserva R
INNER JOIN Livro L ON L.id = R.livroId
INNER JOIN Usuario U ON U.id = R.alunoId
WHERE L.status = 'E';

-- Livros atrasados
SELECT 
    L.titulo,
    U.nome AS aluno,
    R.dataPrevistaDevolucao
FROM Reserva R
INNER JOIN Livro L ON L.id = R.livroId
INNER JOIN Usuario U ON U.id = R.alunoId
WHERE 
    R.dataPrevistaDevolucao < GETDATE()
    AND L.status = 'E';

-- Quantidade de livros por categoria
SELECT 
    C.nome,
    COUNT(LC.livroId) AS totalLivros
FROM Categoria C
LEFT JOIN LivroCategoria LC ON LC.categoriaId = C.id
GROUP BY C.nome;

-- Quantidade de reservas por aluno
SELECT 
    U.nome,
    COUNT(R.id) AS totalReservas
FROM Usuario U
LEFT JOIN Reserva R ON R.alunoId = U.id
WHERE U.tipoBib = 0
GROUP BY U.nome;

-- Buscar livro por título
SELECT *
FROM Livro
WHERE titulo LIKE '%Code%';

-- Dashboard resumido
SELECT
    (SELECT COUNT(*) FROM Livro) AS totalLivros,
    (SELECT COUNT(*) FROM Usuario WHERE tipoBib = 0) AS totalAlunos,
    (SELECT COUNT(*) FROM Reserva) AS totalReservas,
    (SELECT COUNT(*) FROM Livro WHERE status = 'D') AS livrosDisponiveis,
    (SELECT COUNT(*) FROM Livro WHERE status = 'E') AS livrosEmprestados;

SELECT * FROM Categoria