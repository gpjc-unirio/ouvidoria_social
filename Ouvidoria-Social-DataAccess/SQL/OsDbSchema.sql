CREATE DATABASE  IF NOT EXISTS `os_database` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `os_database`;
-- MySQL dump 10.13  Distrib 5.6.17, for Win64 (x86_64)
--
-- Host: localhost    Database: os_database
-- ------------------------------------------------------
-- Server version	5.6.23-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `cidade`
--

DROP TABLE IF EXISTS `cidade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cidade` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cidade` varchar(100) DEFAULT NULL,
  `uf` varchar(2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_cidade_uf_idx` (`uf`),
  CONSTRAINT `fk_cidade_uf` FOREIGN KEY (`uf`) REFERENCES `uf` (`uf`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=19921 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `comentario`
--

DROP TABLE IF EXISTS `comentario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `comentario` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `descricao` longtext,
  `likes` bigint(5) DEFAULT NULL,
  `deslikes` bigint(5) DEFAULT NULL,
  `id_pessoa` int(11) DEFAULT NULL,
  `id_reclamacao` int(11) DEFAULT NULL,
  `dataComentario` date DEFAULT NULL,
  `isSolucao` bit(1) DEFAULT NULL,
  `anexo` varchar(250) DEFAULT NULL,
  `isSolucaoFinal` bit(1) DEFAULT b'0',
  PRIMARY KEY (`id`),
  KEY `fk_comentario_reclamacao_idx` (`id_reclamacao`),
  KEY `fk_comentario_pessoa_idx` (`id_pessoa`),
  CONSTRAINT `fk_comentario_pessoa` FOREIGN KEY (`id_pessoa`) REFERENCES `pessoa` (`id`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `fk_comentario_reclamacao` FOREIGN KEY (`id_reclamacao`) REFERENCES `reclamacao` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=81 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `departamento`
--

DROP TABLE IF EXISTS `departamento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `departamento` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) DEFAULT NULL,
  `id_instituicao` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_departamento_instituicao_idx` (`id_instituicao`),
  CONSTRAINT `fk_departamento_instituicao` FOREIGN KEY (`id_instituicao`) REFERENCES `instituicao` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `desafios`
--

DROP TABLE IF EXISTS `desafios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `desafios` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Descricao` varchar(100) DEFAULT NULL,
  `Titulo` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `instituicao`
--

DROP TABLE IF EXISTS `instituicao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `instituicao` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) DEFAULT NULL,
  `nome_fantasia` varchar(100) DEFAULT NULL,
  `telefone` varchar(11) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `id_cidade` int(11) DEFAULT NULL,
  `contato` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_intituicao_cidae_idx` (`id_cidade`),
  CONSTRAINT `fk_intituicao_cidae` FOREIGN KEY (`id_cidade`) REFERENCES `cidade` (`id`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `log_acesso`
--

DROP TABLE IF EXISTS `log_acesso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `log_acesso` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `datahora` datetime DEFAULT NULL,
  `id_usuario` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_logacesso_usuario_idx` (`id_usuario`),
  CONSTRAINT `fk_logacesso_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=89 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `log_participapao`
--

DROP TABLE IF EXISTS `log_participapao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `log_participapao` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `datahora` datetime DEFAULT NULL,
  `id_pessoa` int(11) DEFAULT NULL,
  `id_reclamacao` int(11) DEFAULT NULL,
  `acesso` bit(1) DEFAULT NULL,
  `tipo` varchar(100) DEFAULT NULL,
  `visto` bit(1) DEFAULT b'0',
  `id_comentario` int(11) DEFAULT NULL,
  `id_curtidor` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_logpart_pessoa_idx` (`id_pessoa`),
  KEY `fk_logpart_reclamacao_idx` (`id_reclamacao`),
  KEY `fk_logpart_comentario_idx` (`id_comentario`),
  CONSTRAINT `fk_logpart_comentario` FOREIGN KEY (`id_comentario`) REFERENCES `comentario` (`id`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `fk_logpart_pessoa` FOREIGN KEY (`id_pessoa`) REFERENCES `pessoa` (`id`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `fk_logpart_reclamacao` FOREIGN KEY (`id_reclamacao`) REFERENCES `reclamacao` (`id`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=323 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `log_pontos_pessoa`
--

DROP TABLE IF EXISTS `log_pontos_pessoa`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `log_pontos_pessoa` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_pessoa` int(11) DEFAULT NULL,
  `pontos` bigint(9) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `tipoPonto` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_logPonto_pessoa_idx` (`id_pessoa`),
  CONSTRAINT `fk_logPonto_pessoa` FOREIGN KEY (`id_pessoa`) REFERENCES `pessoa` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=571 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pessoa`
--

DROP TABLE IF EXISTS `pessoa`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pessoa` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) NOT NULL,
  `apelido` varchar(100) DEFAULT NULL,
  `data_nascimento` date DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `telefone` varchar(11) DEFAULT NULL,
  `descricao` varchar(500) DEFAULT NULL,
  `avatar` varchar(500) DEFAULT NULL,
  `tipo` varchar(1) DEFAULT NULL,
  `id_cidade` int(11) DEFAULT NULL,
  `isAtivo` varchar(1) DEFAULT 'S',
  `id_instituicao` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_pessoa_cidade_idx` (`id_cidade`),
  KEY `fk_pessoa_instituicao_idx` (`id_instituicao`),
  CONSTRAINT `fk_pessoa_cidade` FOREIGN KEY (`id_cidade`) REFERENCES `cidade` (`id`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `fk_pessoa_instituicao` FOREIGN KEY (`id_instituicao`) REFERENCES `instituicao` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=73 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pessoa_desafio`
--

DROP TABLE IF EXISTS `pessoa_desafio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pessoa_desafio` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `idPessoa` int(11) DEFAULT NULL,
  `idDesafio` int(11) DEFAULT NULL,
  `Ganho` bit(1) DEFAULT NULL,
  `Data` date DEFAULT NULL,
  `Hora` time DEFAULT NULL,
  `VezesGanhas` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_pessoa_desafiopessoa_idx` (`idPessoa`),
  KEY `fk_desafio_desafiopessoa_idx` (`idDesafio`),
  CONSTRAINT `fk_desafio_desafiopessoa` FOREIGN KEY (`idDesafio`) REFERENCES `desafios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_pessoa_desafiopessoa` FOREIGN KEY (`idPessoa`) REFERENCES `pessoa` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=109 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pessoa_nivel_pontos`
--

DROP TABLE IF EXISTS `pessoa_nivel_pontos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pessoa_nivel_pontos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_pessoa` int(11) DEFAULT NULL,
  `nivel` int(11) DEFAULT NULL,
  `pontos` bigint(9) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_nivel_pessoa_idx` (`id_pessoa`),
  CONSTRAINT `fk_nivel_pessoa` FOREIGN KEY (`id_pessoa`) REFERENCES `pessoa` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `reclamacao`
--

DROP TABLE IF EXISTS `reclamacao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `reclamacao` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Descricao` longtext,
  `Titulo` varchar(150) DEFAULT NULL,
  `IsSolucionado` bit(1) DEFAULT NULL,
  `Departamento` varchar(100) DEFAULT NULL,
  `id_instituicao` int(11) DEFAULT NULL,
  `id_pessoa` int(11) DEFAULT NULL,
  `likes` int(11) DEFAULT NULL,
  `deslikes` int(11) DEFAULT NULL,
  `anexo` varchar(50) DEFAULT NULL,
  `dataHora` datetime DEFAULT NULL,
  `isAtivo` bit(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_reclamacao_pessoa_idx` (`id_pessoa`),
  KEY `fk_reclamacao_instituicao_idx` (`id_instituicao`),
  CONSTRAINT `fk_reclamacao_instituicao` FOREIGN KEY (`id_instituicao`) REFERENCES `instituicao` (`id`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `fk_reclamacao_pessoa` FOREIGN KEY (`id_pessoa`) REFERENCES `pessoa` (`id`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `uf`
--

DROP TABLE IF EXISTS `uf`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `uf` (
  `uf` varchar(2) NOT NULL,
  `nome` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`uf`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `usuario` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `login` varchar(50) DEFAULT NULL,
  `senha` varchar(100) DEFAULT NULL,
  `id_pessoa` int(11) DEFAULT NULL,
  `gamify` bit(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_usuario_permissao_idx` (`id_pessoa`),
  CONSTRAINT `fk_usuario_pessoa` FOREIGN KEY (`id_pessoa`) REFERENCES `pessoa` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=87 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'os_database'
--

--
-- Dumping routines for database 'os_database'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-12-21 10:46:01
