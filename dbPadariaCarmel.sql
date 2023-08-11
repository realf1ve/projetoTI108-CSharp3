-- Apagando Banco de Dados
drop database dbPadariaCarmel;

-- Criando Banco de Dados
create database dbPadariaCarmel;

-- Acessando Banco de Dados
use dbPadariaCarmel;

-- Criando as Tabelas
create table tbFuncionarios(
codFunc int not null auto_increment, 
nome varchar(100),
email varchar(100),
telCel char (15),
cpf char (14) unique,
endereco varchar(10),
bairro varchar(100),
cidade varchar(100),
estado varchar(2),
cep char(9),
primary key(codFunc)
);