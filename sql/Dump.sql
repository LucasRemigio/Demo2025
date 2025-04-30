CREATE DATABASE  IF NOT EXISTS `masterferro_engimatrix` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `masterferro_engimatrix`;
-- MySQL dump 10.13  Distrib 8.0.26, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: masterferro_engimatrix
-- ------------------------------------------------------
-- Server version	8.0.26

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

CREATE USER IF NOT EXISTS 'masterferro_user'@'%' IDENTIFIED BY 'masterferrouserq1w2e3r4!';
GRANT ALL PRIVILEGES ON masterferro_engimatrix . * TO 'masterferro_user'@'%';FLUSH PRIVILEGES;
SET SQL_MODE='ALLOW_INVALID_DATES';

--
-- Table structure for table `config`
--

DROP TABLE IF EXISTS `config`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `config` (
  `config` varchar(45) NOT NULL,
  `value` mediumtext NOT NULL,
  `changable` tinyint NOT NULL,
  `last_change` datetime NOT NULL,
  `author` varchar(45) NOT NULL,
  `previous_value` varchar(45) NOT NULL,
  `is_password` tinyint NOT NULL,
  `description` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`config`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `config`
--

LOCK TABLES `config` WRITE;
/*!40000 ALTER TABLE `config` DISABLE KEYS */;
INSERT INTO `config` VALUES ('client_endpoint','http://localhost:4200',1,'0001-01-01 00:00:00','SYS','0',0,NULL),
    ('dnos_number_requests','20',0,'0001-01-01 00:00:00','SYS','0',0,NULL),
    ('dnos_number_requests_diff_seconds','2',0,'0001-01-01 00:00:00','SYS','0',0,NULL),
    ('log_level','DEBUG',0,'0001-01-01 00:00:00','SYS','0',0,NULL),
    ('notification_email_hostname','smtp.office365.com',1,'0001-01-01 00:00:00','SYS','0',0,NULL),
    ('notification_email_password','uEQ08qKhrD0lXlgCBW1ymw==',1,'2022-08-09 14:03:13','config_script','',1,''),
    ('notification_email_port','587',1,'0001-01-01 00:00:00','SYS','0',0,NULL),
    ('notification_email_username','apps@engibots.com',1,'0001-01-01 00:00:00','SYS','0',0,NULL),
    ('password_expiration_policy_days','180',1,'0001-01-01 00:00:00','SYS','0',0,'Password expiration number of days'),
    ('reset_token_expiration_time','1',1,'0001-01-01 00:00:00','SYS','0',0,'Maximum expiration time in hours for reset password tokens');
INSERT INTO `config` VALUES ('email_feedback_exp_time',1,1,'0001-01-01 00:00:00','SYS','0',0,'Max. experation time in minutes for sending changes emails'),
    ('email_sender_feedback','tiago.pereira@engibots.com',1,'0001-01-01 00:00:00','SYS','0',0,'Users to receive notification changes email');
INSERT INTO `config` VALUES ('auth2f_code_expiration_time','30',1,'0001-01-01 00:00:00','SYS','0',0,NULL),
    ('delete_auth2fcodes_older_than_this_minutes','30',1,'0001-01-01 00:00:00','SYS','0',0,'0'),
    ('use_2factor_auth','false',1,'0001-01-01 00:00:00','SYS','0',0,NULL),
    ('last_msToken_validation_time','4/30/2024 2:38:39 PM',1,'0001-01-01 00:00:00','SYS','0',0,NULL);
    LOCK TABLES `config` WRITE;

/*!40000 ALTER TABLE `config` ENABLE KEYS */;
UNLOCK TABLES;


-- Table structure for table `notification_email`
--

DROP TABLE IF EXISTS `notification_email`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notification_email` (
  `id_notification_email` int NOT NULL AUTO_INCREMENT,
  `send_to` varchar(250) NOT NULL,
  `subject` varchar(100) NOT NULL,
  `email_template` varchar(150) NOT NULL,
  `email_args` varchar(1000) NOT NULL,
  `email_lang` varchar(45) NOT NULL,
  `last_send` datetime DEFAULT NULL,
  `retries` int unsigned DEFAULT '0',
  PRIMARY KEY (`id_notification_email`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `reset_pass_token`
--

DROP TABLE IF EXISTS `reset_pass_token`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reset_pass_token` (
  `email` varchar(250) NOT NULL,
  `token` varchar(100) NOT NULL,
  `expiration_time` datetime NOT NULL,
  PRIMARY KEY (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `user_email` varchar(250) NOT NULL,
  `user_name` varchar(90) NOT NULL,
  `user_password` varchar(100) NOT NULL,
  `user_role_id` int NOT NULL,
  `active_since` datetime NOT NULL,
  `last_login` datetime DEFAULT NULL,
  `language` varchar(10) NOT NULL,
  `pass_expires` int NOT NULL DEFAULT '1',
  `last_pass_change` datetime NOT NULL,
  `pass_history` varchar(1200) NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `user_id_UNIQUE` (`user_id`),
  UNIQUE KEY `user_email_UNIQUE` (`user_email`),
  KEY `user_role_id_idx` (`user_role_id`),
  CONSTRAINT `user_role_id` FOREIGN KEY (`user_role_id`) REFERENCES `user_role` (`user_role_id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

INSERT INTO `user` VALUES (1,'tiago.pereira@engibots.com','Tiago Pereira','MTIzQEBRd2UxMjM=',2,'2023-09-10 20:58:08','2023-09-28 16:29:34','pt',0,'2023-09-27 10:01:53','Dpwn1zZefMVF+47AOHWiFOTCV3p/CBtw6OFQ/hlsfmfFgzcSsA/KeL1Tnm2T2lUMHSbbJQhSGkIm/QUupUqwSi0NqKlzx8U+HWHVGHt7dcA=');
--
-- Table structure for table `user_pending`
--

DROP TABLE IF EXISTS `user_pending`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_pending` (
  `registry_id` varchar(50) NOT NULL,
  `user_name` varchar(45) NOT NULL,
  `user_email` varchar(250) NOT NULL,
  `user_password` varchar(45) NOT NULL,
  `registry_time` datetime NOT NULL,
  `num_of_contacts` int NOT NULL,
  `last_contact` datetime DEFAULT NULL,
  `language` varchar(2) NOT NULL,
  PRIMARY KEY (`registry_id`),
  UNIQUE KEY `registry_id_UNIQUE` (`registry_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_pending`
--

--
-- Table structure for table `user_role`
--

DROP TABLE IF EXISTS `user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_role` (
  `user_role_id` int NOT NULL AUTO_INCREMENT,
  `user_role` varchar(45) NOT NULL,
  PRIMARY KEY (`user_role_id`),
  UNIQUE KEY `user_role_id_UNIQUE` (`user_role_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_role`
--

LOCK TABLES `user_role` WRITE;
/*!40000 ALTER TABLE `user_role` DISABLE KEYS */;
INSERT INTO `user_role` VALUES (1,'SYS'),(2,'ADMIN'),(3,'USER');
/*!40000 ALTER TABLE `user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `auth2f`
--

DROP TABLE IF EXISTS `auth2f`;

CREATE TABLE `auth2f` (
  `user_email` varchar(250) NOT NULL,
  `token` varchar(25) NOT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`token`),
  KEY `user_email` (`user_email`),
  CONSTRAINT `auth2f_ibfk_1` FOREIGN KEY (`user_email`) REFERENCES `user` (`user_email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE triggers (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    cron_expression VARCHAR(255) NOT NULL,
    script_id INT NOT NULL,
    FOREIGN KEY (script_id) REFERENCES script(id)
);

CREATE TABLE `job` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_script` int NOT NULL,
  `user_operation` varchar(10000) DEFAULT NULL,
  `date_time` varchar(45) DEFAULT NULL,
  `job_details` longtext,
  `status` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_script` (`id_script`),
  KEY `status` (`status`),
  CONSTRAINT `job_ibfk_1` FOREIGN KEY (`id_script`) REFERENCES `script` (`id`),
  CONSTRAINT `job_ibfk_2` FOREIGN KEY (`status`) REFERENCES `status` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `status`
--

DROP TABLE IF EXISTS `status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `status` (
  `id` int NOT NULL AUTO_INCREMENT,
  `description` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `status`
--

LOCK TABLES `status` WRITE;
/*!40000 ALTER TABLE `status` DISABLE KEYS */;
INSERT INTO `status` VALUES (1,'Ativo'),(2,'Inativo'),(3,'Sucesso'),
(4,'Insucesso'),(5,'Erro'),(6,'Reprocessado'),(7,'Triagem Realizada'),(8,'Apagado'),(9,'Novo'), (10, 'Aguarda Validação'), (11, "A Processar"), (12, "Resolvido Manualmente");
/*!40000 ALTER TABLE `status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `logs`
--

DROP TABLE IF EXISTS `logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `logs` (
  `id_log` int NOT NULL AUTO_INCREMENT,
  `operation` varchar(10000) DEFAULT NULL,
  `user_operation` varchar(255) DEFAULT NULL,
  `operation_state` varchar(255) DEFAULT NULL,
  `date_time` datetime DEFAULT NULL,
  `operation_context` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_log`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `logs`
--

LOCK TABLES `logs` WRITE;
/*!40000 ALTER TABLE `logs` DISABLE KEYS */;
/*!40000 ALTER TABLE `logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Master Ferro Specific Dumps
--


--
-- Table structure for table `department`
--

DROP TABLE IF EXISTS `department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `department` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(80) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `department`
--

LOCK TABLES `department` WRITE;
/*!40000 ALTER TABLE `department` DISABLE KEYS */;
INSERT INTO "department" VALUES (1, 'filtering'), (2, 'communications'), (3, 'orders'), (4, 'quotations'), (5, 'receipts'), (6, 'certificates');
/*!40000 ALTER TABLE `department` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_department`
--

DROP TABLE IF EXISTS `user_department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_department` (
  `department_id` int NOT NULL,
  `user_email` varchar(100) NOT NULL,
  PRIMARY KEY (`department_id`, `user_email`),
  CONSTRAINT `user_department_ibfk_1` FOREIGN KEY (`department_id`) REFERENCES `department` (`id`),
  CONSTRAINT `user_department_ibfk_2` FOREIGN KEY (`user_email`) REFERENCES `user` (`user_email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `signature`
--

DROP TABLE IF EXISTS `signature`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `signature` (
  `user_id` int NOT NULL,
  `signature` TEXT NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `user_id_UNIQUE` (`user_id`),
  CONSTRAINT `user_signature_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `category`
--


DROP TABLE IF EXISTS `category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `category` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(500) DEFAULT NULL,
  `slug` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `category`
--

LOCK TABLES `category` WRITE;
/*!40000 ALTER TABLE `category` DISABLE KEYS */;
INSERT INTO `category` VALUES (1,'Encomendas','encomendas'),(2,'Cotações & Orçamentos','pedidos'),(3,'Comprovativos de Pagamento','comprovativos'),(4,'Outros','outros'),(5,'Erro', 'erro'),(6,'Duplicados', 'duplicados'), (7, "Certificados de Qualidade", "certificados");;
/*!40000 ALTER TABLE `category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `validation_motives`
--
DROP TABLE IF EXISTS `validation_motives`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `validation_motives` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(500) DEFAULT NULL,
  `slug` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `validation_motives`
--

LOCK TABLES `validation_motives` WRITE;
/*!40000 ALTER TABLE `validation_motives` DISABLE KEYS */;
INSERT INTO `validation_motives` VALUES (1,'Ocorreu um erro durante o processamento','error'),(2,'Baixa Confiança','confianca');
/*!40000 ALTER TABLE `validation_motives` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `email`
--

DROP TABLE IF EXISTS `email`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `email` (
  `id` int NOT NULL AUTO_INCREMENT,
  `from` varchar(255) NOT NULL,
  `to` varchar(255) NOT NULL,
  `cc` varchar(255) DEFAULT NULL,
  `bcc` varchar(255) DEFAULT NULL,
  `subject` varchar(1000) NOT NULL,
  `body` varchar(100000) NOT NULL,
  `date` datetime NOT NULL,
  KEY `id` (`id`),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `reply`
--
DROP TABLE IF EXISTS `reply`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reply` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email_token` varchar(100) NOT NULL,
  `reply_token` varchar(100) NOT NULL,
  `from` varchar(255) NOT NULL,
  `to` varchar(255) NOT NULL,
  `subject` varchar(1000) NOT NULL,
  `body` longtext NOT NULL,
  `date` datetime NOT NULL,
  `replied_by` varchar(100) NOT NULL,
  `is_read` TINYINT(1) NOT NULL DEFAULT 0,
  KEY `id` (`id`),
  KEY `reply_token` (`reply_token`),
  PRIMARY KEY (`id`),
  CONSTRAINT `reply_ibfk_1` FOREIGN KEY (`email_token`) REFERENCES `filtered_email` (`token`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `reply_attachment`
--

DROP TABLE IF EXISTS `reply_attachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reply_attachment` (
  `id` int NOT NULL AUTO_INCREMENT,
  `reply_token` varchar(100) NOT NULL,
  `name` varchar (300) NOT NULL,
  `size` int NOT NULL,
  `file` longtext NULL,
  KEY `id` (`id`),
  KEY `reply_token` (`reply_token`),
  PRIMARY KEY (`id`),
  CONSTRAINT `reply_attachment_ifbk1` FOREIGN KEY (`reply_token`) REFERENCES `reply` (`reply_token`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `Attachment`
--

DROP TABLE IF EXISTS `attachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attachment` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email` int NOT NULL,
  `name` varchar (300) NOT NULL,
  `size` int NOT NULL,
  `file` longtext NULL,
  KEY `id` (`id`),
  KEY `email` (`email`),
  PRIMARY KEY (`id`),
  CONSTRAINT `email_ifbk1` FOREIGN KEY (`email`) REFERENCES `email` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `FilteredEmail`
--

DROP TABLE IF EXISTS `filtered_email`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `filtered_email` (
  `email` int NOT NULL,
  `status` int NOT NULL,
  `category` int NOT NULL,
  `date` datetime NOT NULL,
  `token` varchar(100) NOT NULL,
  `confidence` int NOT NULL,
  `validated` bool NOT NULL default false,
  `resolved_by` varchar(250);
  `resolved_at` datetime;
  PRIMARY KEY (`email`),
  KEY `email` (`email`),
  KEY `status` (`status`),
  KEY `category` (`category`), 
  CONSTRAINT `filtered_email_ifbk1` FOREIGN KEY (`email`) REFERENCES `email` (`id`),
  CONSTRAINT `filtered_email_ibfk_2` FOREIGN KEY (`status`) REFERENCES `status` (`id`),
  CONSTRAINT `filtered_email_ifbk3` FOREIGN KEY (`category`) REFERENCES `category` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `email_product`
--

DROP TABLE IF EXISTS `email_product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `email_product` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email_token` varchar(100) NOT NULL,
  `name` varchar(300) NOT NULL,
  `size` varchar(100) NOT NULL,
  `quantity` decimal(10,2) NOT NULL,
  `quantity_unit` varchar(50) NOT NULL,
  `confidence` int NOT NULL,
  `product_catalog_id` int NULL, -- Make this field nullable
  `match_confidence` int NULL,
  `calculated_price` decimal(10,2) NULL,
  KEY `id` (`id`),
  KEY `email_token` (`email_token`),
  PRIMARY KEY (`id`),
  CONSTRAINT `filtered_email_token_ifbk2` FOREIGN KEY (`email_token`) REFERENCES `filtered_email` (`token`),
  CONSTRAINT `product_catalog_fk` FOREIGN KEY (`product_catalog_id`) REFERENCES `mf_product_catalog` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;



--
-- Table structure for table `email_forwards`
--

DROP TABLE IF EXISTS `email_forward`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `email_forward` (
  `id` int NOT NULL AUTO_INCREMENT, 
  `email_token` varchar(100), 
  `email_id` int, 
  `forwarded_by` varchar(250) NOT NULL,
  `forwarded_to` varchar(250) NOT NULL, 
  `forwarded_at` datetime NOT NULL,     
  PRIMARY KEY (`id`),                   
  KEY `email_token_idx` (`email_token`), 
  KEY `email_id_idx` (`email_id`), 
  CONSTRAINT `email_forwards_ibfk_1` FOREIGN KEY (`email_token`) REFERENCES `filtered_email` (`token`) ON DELETE CASCADE,
  CONSTRAINT `email_forwards_ibfk_2` FOREIGN KEY (`email_id`) REFERENCES `email` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


--
-- Table structure for table `mf_family`
--

DROP TABLE IF EXISTS `mf_product_family`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mf_product_family` (
    `id` VARCHAR(10) PRIMARY KEY,
    `name` VARCHAR(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

INSERT INTO `mf_product_family` (`id`, `name`) VALUES 
    (0, 'DIVERSOS/FINANCEIRO'), (1, 'VARÃO P/ BETÃO 6MM'), (11, 'VARÃO P/ BETÃO'), (12, 'BARRAMENTOS'), 
    (13, 'VIGAS EM FERRO'), (14, 'CHAPA'), (15, 'REDES'), (16, 'MALHA SOL'), (17, 'ARAMES'), (18, 'PREGOS'), 
    (19, 'TAMPAS'), (2, 'ESTRADOS'), (21, 'TUBO DE AÇO'), (22, 'TUBO GALVANIZADO'), (23, 'TUBO ESTRUTURAL'), 
    (24, 'TUBO ESPECIAL'), (25, 'TUBO DECORATIVO'), (26, 'CALHAS'), (31, 'TUBO CANALIZAÇÃO'), 
    (32, 'ACESSÓRIOS P/ CANALIZAÇÃO ROSCADOS'), (4, 'ACESSORIOS P/SOLDAR'), (41, 'TUBO SEM COSTURA'), 
    (52, 'FOGÕES E CALORIFEROS'), (53, 'TORNEIRAS E VALVULAS DE ESFERA'), (54, 'LAVA-LOUÇAS'), 
    (56, 'FERRO FORJADO'), (6, 'RIDGID'), (61, 'CARROS E FERRAGENS'), (62, 'DESCONTINUADOS'), 
    (7, 'DIVERSOS'), ('PS', 'Prestação de Serviços'), ('R100', 'ISOLAMENTOS'), ('R200', 'IMPERMEABILIZAÇÃO'), 
    ('R300', 'TETOS FALSOS'), ('R350', 'PERFILARIA LACADA'), ('R351', 'PERFILARIA ZINCADA'), 
    ('R352', 'ACESSÓRIOS P/ TECTOS E DIVISÓRIAS'), ('R353', 'MASSAS DIVERSAS'), ('R354', 'PLACAS P/ REVESTIMENTOS'), 
    ('R355', 'SANCAS DECORATIVAS'), ('R356', 'FERRAMENTAS'), ('R400', 'PRODUTOS CORTA-FOGO'), 
    ('R500', 'PRODUTOS DE SEGURANÇA'), ('R520', 'PORTAS E ACESSÓRIOS'), ('R600', 'PAVIMENTOS E PAREDES'), 
    ('R700', 'MATERIAL ELECTRICO'), ('R800', 'PORTES'), ('R900', 'DIVERSOS');


--
-- Table structure for table `mf_segment`
--

DROP TABLE IF EXISTS `mf_segment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mf_segment` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `id` INT PRIMARY KEY,
    `name` VARCHAR(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

INSERT INTO `mf_segment` (`id`, `name`) VALUES(1, 'Serralharia'),(2, 'Construção'),(3, 'Revenda'),(4, 'Indústria'),(5, 'Outros');


--
-- Table structure for table `mf_product_discount`
--

DROP TABLE IF EXISTS `mf_product_discount`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mf_product_discount` (
    `product_family_id` VARCHAR(10) NOT NULL,
    `segment_id` INT NOT NULL,
    `mb_min` DECIMAL(5,2) NOT NULL,
    `desc_max` DECIMAL(5,2) NOT NULL,
    FOREIGN KEY (`product_family_id`) REFERENCES `mf_product_family`(`id`),
    FOREIGN KEY (`segment_id`) REFERENCES `mf_segment`(`id`),
    PRIMARY KEY (`product_family_id`, `segment_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


INSERT INTO `mf_product_discount` (`product_family_id`, `segment_id`, `mb_min`, `desc_max`) VALUES
-- VARÃO P/ BETÃO 6MM
('1', 1, 10.00, 10.00),   -- Serralharia
('1', 2, 10.00, 10.00),   -- Construção
('1', 3, 8.00, 10.00),    -- Revenda
('1', 4, 10.00, 10.00),   -- Indústria
('1', 5, 10.00, 10.00),   -- Outros
-- VARÃO P/ BETÃO
('11', 1, 10.00, 10.00),  -- Serralharia
('11', 2, 10.00, 10.00),  -- Construção
('11', 3, 8.00, 10.00),   -- Revenda
('11', 4, 10.00, 10.00),  -- Indústria
('11', 5, 10.00, 10.00),  -- Outros
-- BARRAMENTOS
('12', 1, 12.00, 10.00),  -- Serralharia
('12', 2, 12.00, 10.00),  -- Construção
('12', 3, 10.00, 10.00),  -- Revenda
('12', 4, 12.00, 10.00),  -- Indústria
('12', 5, 12.00, 10.00),  -- Outros
-- VIGAS EM FERRO
('13', 1, 12.00, 10.00),  -- Serralharia
('13', 2, 12.00, 10.00),  -- Construção
('13', 3, 10.00, 10.00),  -- Revenda
('13', 4, 12.00, 10.00),  -- Indústria
('13', 5, 12.00, 10.00),  -- Outros
-- CHAPA
('14', 1, 12.00, 10.00),  -- Serralharia
('14', 2, 12.00, 10.00),  -- Construção
('14', 3, 10.00, 10.00),  -- Revenda
('14', 4, 12.00, 10.00),  -- Indústria
('14', 5, 12.00, 10.00),  -- Outros
-- REDES
('15', 1, 12.00, 10.00),  -- Serralharia
('15', 2, 12.00, 10.00),  -- Construção
('15', 3, 10.00, 10.00),  -- Revenda
('15', 4, 12.00, 10.00),  -- Indústria
('15', 5, 12.00, 10.00),  -- Outros
-- MALHA SOL
('16', 1, 12.00, 10.00),  -- Serralharia
('16', 2, 12.00, 10.00),  -- Construção
('16', 3, 10.00, 10.00),  -- Revenda
('16', 4, 12.00, 10.00),  -- Indústria
('16', 5, 12.00, 10.00),  -- Outros
-- ARAMES
('17', 1, 12.00, 10.00),  -- Serralharia
('17', 2, 12.00, 10.00),  -- Construção
('17', 3, 10.00, 10.00),  -- Revenda
('17', 4, 12.00, 10.00),  -- Indústria
('17', 5, 12.00, 10.00),  -- Outros
-- PREGOS
('18', 1, 12.00, 10.00),  -- Serralharia
('18', 2, 12.00, 10.00),  -- Construção
('18', 3, 10.00, 10.00),  -- Revenda
('18', 4, 12.00, 10.00),  -- Indústria
('18', 5, 12.00, 10.00),  -- Outros
-- TAMPAS
('19', 1, 12.00, 10.00),  -- Serralharia
('19', 2, 12.00, 10.00),  -- Construção
('19', 3, 10.00, 10.00),  -- Revenda
('19', 4, 12.00, 10.00),  -- Indústria
('19', 5, 12.00, 10.00),  -- Outros
-- ESTRADOS
('2', 1, 12.00, 10.00),   -- Serralharia
('2', 2, 12.00, 10.00),   -- Construção
('2', 3, 10.00, 10.00),   -- Revenda
('2', 4, 12.00, 10.00),   -- Indústria
('2', 5, 12.00, 10.00),   -- Outros
-- TUBO DE AÇO
('21', 1, 12.00, 10.00),  -- Serralharia
('21', 2, 12.00, 10.00),  -- Construção
('21', 3, 10.00, 10.00),  -- Revenda
('21', 4, 10.00, 10.00),  -- Indústria
('21', 5, 12.00, 10.00),  -- Outros
-- TUBO GALVANIZADO
('22', 1, 12.00, 10.00),  -- Serralharia
('22', 2, 12.00, 10.00),  -- Construção
('22', 3, 10.00, 10.00),  -- Revenda
('22', 4, 10.00, 10.00),  -- Indústria
('22', 5, 12.00, 10.00),  -- Outros
-- TUBO ESTRUTURAL
('23', 1, 12.00, 10.00),  -- Serralharia
('23', 2, 12.00, 10.00),  -- Construção
('23', 3, 10.00, 10.00),  -- Revenda
('23', 4, 10.00, 10.00),  -- Indústria
('23', 5, 12.00, 10.00),  -- Outros
-- TUBO ESPECIAL
('24', 1, 12.00, 10.00),  -- Serralharia
('24', 2, 12.00, 10.00),  -- Construção
('24', 3, 10.00, 10.00),  -- Revenda
('24', 4, 10.00, 10.00),  -- Indústria
('24', 5, 12.00, 10.00),  -- Outros
-- TUBO DECORATIVO
('25', 1, 12.00, 10.00),  -- Serralharia
('25', 2, 12.00, 10.00),  -- Construção
('25', 3, 10.00, 10.00),  -- Revenda
('25', 4, 10.00, 10.00),  -- Indústria
('25', 5, 12.00, 10.00),  -- Outros
-- CALHAS
('26', 1, 12.00, 10.00),  -- Serralharia
('26', 2, 12.00, 10.00),  -- Construção
('26', 3, 10.00, 10.00),  -- Revenda
('26', 4, 12.00, 10.00),  -- Indústria
('26', 5, 12.00, 10.00),  -- Outros
-- TUBO CANALIZAÇÃO
('31', 1, 12.00, 10.00),  -- Serralharia
('31', 2, 12.00, 10.00),  -- Construção
('31', 3, 10.00, 10.00),  -- Revenda
('31', 4, 10.00, 10.00),  -- Indústria
('31', 5, 12.00, 10.00),  -- Outros
-- ACESSÓRIOS P/ CANALIZAÇÃO ROSCADOS
('32', 1, 12.00, 10.00),  -- Serralharia
('32', 2, 12.00, 10.00),  -- Construção
('32', 3, 10.00, 10.00),  -- Revenda
('32', 4, 12.00, 10.00),  -- Indústria
('32', 5, 12.00, 10.00),  -- Outros
-- ACESSORIOS P/SOLDAR
('4', 1, 12.00, 10.00),   -- Serralharia
('4', 2, 12.00, 10.00),   -- Construção
('4', 3, 10.00, 10.00),   -- Revenda
('4', 4, 12.00, 10.00),   -- Indústria
('4', 5, 12.00, 10.00),   -- Outros
-- TUBO SEM COSTURA
('41', 1, 12.00, 10.00),  -- Serralharia
('41', 2, 12.00, 10.00),  -- Construção
('41', 3, 10.00, 10.00),  -- Revenda
('41', 4, 12.00, 10.00),  -- Indústria
('41', 5, 12.00, 10.00),  -- Outros
-- FOGÕES E CALORIFEROS
('52', 1, 12.00, 10.00),  -- Serralharia
('52', 2, 12.00, 10.00),  -- Construção
('52', 3, 10.00, 10.00),  -- Revenda
('52', 4, 12.00, 10.00),  -- Indústria
('52', 5, 12.00, 10.00),  -- Outros
-- TORNEIRAS E VÁLVULAS DE ESFERA
('53', 1, 12.00, 10.00),  -- Serralharia
('53', 2, 12.00, 10.00),  -- Construção
('53', 3, 10.00, 10.00),  -- Revenda
('53', 4, 12.00, 10.00),  -- Indústria
('53', 5, 12.00, 10.00),  -- Outros
-- LAVA-LOUÇAS
('54', 1, 12.00, 10.00),  -- Serralharia
('54', 2, 12.00, 10.00),  -- Construção
('54', 3, 10.00, 10.00),  -- Revenda
('54', 4, 12.00, 10.00),  -- Indústria
('54', 5, 12.00, 10.00),  -- Outros
-- FERRO FORJADO
('56', 1, 12.00, 10.00),  -- Serralharia
('56', 2, 12.00, 10.00),  -- Construção
('56', 3, 10.00, 10.00),  -- Revenda
('56', 4, 12.00, 10.00),  -- Indústria
('56', 5, 12.00, 10.00),  -- Outros
-- RIDGID
('6', 1, 12.00, 10.00),   -- Serralharia
('6', 2, 12.00, 10.00),   -- Construção
('6', 3, 10.00, 10.00),   -- Revenda
('6', 4, 12.00, 10.00),   -- Indústria
('6', 5, 12.00, 10.00),   -- Outros
-- CARROS E FERRAGENS
('61', 1, 12.00, 10.00),  -- Serralharia
('61', 2, 12.00, 10.00),  -- Construção
('61', 3, 10.00, 10.00),  -- Revenda
('61', 4, 12.00, 10.00),  -- Indústria
('61', 5, 12.00, 10.00),  -- Outros
-- DESCONTINUADOS
('62', 1, 12.00, 10.00),  -- Serralharia
('62', 2, 12.00, 10.00),  -- Construção
('62', 3, 10.00, 10.00),  -- Revenda
('62', 4, 12.00, 10.00),  -- Indústria
('62', 5, 12.00, 10.00),  -- Outros
-- DIVERSOS
('7', 1, 12.00, 10.00),   -- Serralharia
('7', 2, 12.00, 10.00),   -- Construção
('7', 3, 10.00, 10.00),   -- Revenda
('7', 4, 12.00, 10.00),   -- Indústria
('7', 5, 12.00, 10.00),   -- Outros
-- PRESTAÇÃO DE SERVIÇOS
('PS', 1, 12.00, 10.00),  -- Serralharia
('PS', 2, 12.00, 10.00),  -- Construção
('PS', 3, 10.00, 10.00),  -- Revenda
('PS', 4, 12.00, 10.00),  -- Indústria
('PS', 5, 12.00, 10.00),  -- Outros
-- ISOLAMENTOS
('R100', 1, 14.00, 10.00),  -- Serralharia
('R100', 2, 14.00, 10.00),  -- Construção
('R100', 3, 12.00, 10.00),  -- Revenda
('R100', 4, 14.00, 10.00),  -- Indústria
('R100', 5, 14.00, 10.00),  -- Outros
-- IMPERMEABILIZAÇÃO
('R200', 1, 14.00, 10.00),  -- Serralharia
('R200', 2, 14.00, 10.00),  -- Construção
('R200', 3, 12.00, 10.00),  -- Revenda
('R200', 4, 14.00, 10.00),  -- Indústria
('R200', 5, 14.00, 10.00),  -- Outros
-- TETOS FALSOS
('R300', 1, 14.00, 10.00),  -- Serralharia
('R300', 2, 14.00, 10.00),  -- Construção
('R300', 3, 12.00, 10.00),  -- Revenda
('R300', 4, 14.00, 10.00),  -- Indústria
('R300', 5, 14.00, 10.00),  -- Outros
-- PERFILARIA LACADA
('R350', 1, 14.00, 10.00),  -- Serralharia
('R350', 2, 14.00, 10.00),  -- Construção
('R350', 3, 12.00, 10.00),  -- Revenda
('R350', 4, 14.00, 10.00),  -- Indústria
('R350', 5, 14.00, 10.00),  -- Outros
-- PERFILARIA ZINCADA
('R351', 1, 14.00, 10.00),  -- Serralharia
('R351', 2, 14.00, 10.00),  -- Construção
('R351', 3, 12.00, 10.00),  -- Revenda
('R351', 4, 14.00, 10.00),  -- Indústria
('R351', 5, 14.00, 10.00),  -- Outros
-- ACESSÓRIOS P/ TECTOS E DIVISÓRIAS
('R352', 1, 14.00, 10.00),  -- Serralharia
('R352', 2, 14.00, 10.00),  -- Construção
('R352', 3, 12.00, 10.00),  -- Revenda
('R352', 4, 14.00, 10.00),  -- Indústria
('R352', 5, 14.00, 10.00),  -- Outros
-- MASSAS DIVERSAS
('R353', 1, 14.00, 10.00),  -- Serralharia
('R353', 2, 14.00, 10.00),  -- Construção
('R353', 3, 12.00, 10.00),  -- Revenda
('R353', 4, 14.00, 10.00),  -- Indústria
('R353', 5, 14.00, 10.00),  -- Outros
-- PLACAS P/ REVESTIMENTOS
('R354', 1, 14.00, 10.00),  -- Serralharia
('R354', 2, 14.00, 10.00),  -- Construção
('R354', 3, 12.00, 10.00),  -- Revenda
('R354', 4, 14.00, 10.00),  -- Indústria
('R354', 5, 14.00, 10.00),  -- Outros
-- SANCAS DECORATIVAS
('R355', 1, 14.00, 10.00),  -- Serralharia
('R355', 2, 14.00, 10.00),  -- Construção
('R355', 3, 12.00, 10.00),  -- Revenda
('R355', 4, 14.00, 10.00),  -- Indústria
('R355', 5, 14.00, 10.00),  -- Outros
-- FERRAMENTAS
('R356', 1, 14.00, 10.00),  -- Serralharia
('R356', 2, 14.00, 10.00),  -- Construção
('R356', 3, 12.00, 10.00),  -- Revenda
('R356', 4, 14.00, 10.00),  -- Indústria
('R356', 5, 14.00, 10.00),  -- Outros
-- PRODUTOS CORTA-FOGO
('R400', 1, 14.00, 10.00),  -- Serralharia
('R400', 2, 14.00, 10.00),  -- Construção
('R400', 3, 12.00, 10.00),  -- Revenda
('R400', 4, 14.00, 10.00),  -- Indústria
('R400', 5, 14.00, 10.00),  -- Outros
-- PRODUTOS DE SEGURANÇA
('R500', 1, 14.00, 10.00),  -- Serralharia
('R500', 2, 14.00, 10.00),  -- Construção
('R500', 3, 12.00, 10.00),  -- Revenda
('R500', 4, 14.00, 10.00),  -- Indústria
('R500', 5, 14.00, 10.00),  -- Outros
-- PORTAS E ACESSÓRIOS
('R520', 1, 14.00, 10.00),  -- Serralharia
('R520', 2, 14.00, 10.00),  -- Construção
('R520', 3, 12.00, 10.00),  -- Revenda
('R520', 4, 14.00, 10.00),  -- Indústria
('R520', 5, 14.00, 10.00),  -- Outros
-- PAVIMENTOS E PAREDES
('R600', 1, 14.00, 10.00),  -- Serralharia
('R600', 2, 14.00, 10.00),  -- Construção
('R600', 3, 12.00, 10.00),  -- Revenda
('R600', 4, 14.00, 10.00),  -- Indústria
('R600', 5, 14.00, 10.00),  -- Outros
-- MATERIAL ELÉCTRICO
('R700', 1, 14.00, 10.00),  -- Serralharia
('R700', 2, 14.00, 10.00),  -- Construção
('R700', 3, 12.00, 10.00),  -- Revenda
('R700', 4, 14.00, 10.00),  -- Indústria
('R700', 5, 14.00, 10.00),  -- Outros
-- PORTES
('R800', 1, 14.00, 10.00),  -- Serralharia
('R800', 2, 14.00, 10.00),  -- Construção
('R800', 3, 12.00, 10.00),  -- Revenda
('R800', 4, 14.00, 10.00),  -- Indústria
('R800', 5, 14.00, 10.00),  -- Outros
-- DIVERSOS
('R900', 1, 14.00, 10.00),  -- Serralharia
('R900', 2, 14.00, 10.00),  -- Construção
('R900', 3, 12.00, 10.00),  -- Revenda
('R900', 4, 14.00, 10.00),  -- Indústria
('R900', 5, 14.00, 10.00);  -- Outros




--
-- Table structure for table `mf_product_catalog`
--

DROP TABLE IF EXISTS `mf_product_catalog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mf_product_catalog` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `product_code` VARCHAR(50),
    `description` VARCHAR(255) NOT NULL,
    `unit` VARCHAR(10),
    `stock_current` DECIMAL(10,2),
    `currency` VARCHAR(3) NOT NULL,
    `price_pvp` DECIMAL(10,2) DEFAULT 0.0,
    `price_avg` DECIMAL(10,2),
    `price_last` DECIMAL(10,2),
    `date_last_entry` DATE,
    `date_last_exit` DATE,
    `family_id` VARCHAR(10),
    `price_ref_market` DECIMAL(10,2),
    `type_id` INT,
    `material_id` INT,
    `shape_id` INT,
    `finishing_id` INT,
    `surface_id` INT,
    `length` DECIMAL(10,2),     
    `width` DECIMAL(10,2),      
    `height` DECIMAL(10,2),    
    FOREIGN KEY (`family_id`) REFERENCES `mf_product_family`(`id`),
    FOREIGN KEY (`type_id`) REFERENCES `mf_product_type`(`id`),
    FOREIGN KEY (`material_id`) REFERENCES `mf_product_material`(`id`),
    FOREIGN KEY (`shape_id`) REFERENCES `mf_product_shape`(`id`),
    FOREIGN KEY (`finishing_id`) REFERENCES `mf_product_finishing`(`id`),
    FOREIGN KEY (`surface_id`) REFERENCES `mf_product_surface`(`id`),
    INDEX `idx_product_code` (`product_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- To import this table's data, there is a python script on this same folder, ./MasterFerroDataImports with the indications
-- and instructions to import the data to the mysql database needed, named mf_product_catalog_data_import.py
-- 
DROP TABLE IF EXISTS `mf_product_type`;
CREATE TABLE `mf_product_type` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `name` VARCHAR(50) UNIQUE NOT NULL
);

INSERT INTO `mf_product_type` (`name`) VALUES 
('Varão'),
('Barra'),
('Viga'),
('Canteiro'),
('Chapa'),
('Tubo'),
('Calha'),
('Curva'),
('Rede'),
('Painel'),
('Poste'),
('Malha'),
('Arame'),
('Grelha'),
('União'),
('Joelho'),
('Te'),
('Diversos');

DROP TABLE IF EXISTS `mf_product_material`;
CREATE TABLE `mf_product_material` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `name` VARCHAR(50) UNIQUE NOT NULL
);

INSERT INTO `mf_product_material` (`name`) VALUES 
('Ferro'),
('Aço'),
('Alumínio'),
('Inox'),
('Cobre'),
('PVC'),
('Plástico'),
('Malha');

DROP TABLE IF EXISTS `mf_product_shape`;
CREATE TABLE `mf_product_shape` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `name` VARCHAR(50) UNIQUE NOT NULL
);

INSERT INTO `mf_product_shape` (`name`) VALUES 
('Retangular'),
('Quadrado'),
('UPN'),
('Vergalhão'),
('T'),
('IPN'),
('HEB'),
('HEA'),
('HEM'),
('IPE'),
('Redondo'),
('Corrimão'), 
('Aba'),
('Decorativo'),
('Canalização'),
('Cantoneira'),
('Nó'),
('Jarditor'),
('Hexagonal'),
('Eletrossoldado'),
('Hércules')
;

DROP TABLE IF EXISTS `mf_product_finishing`;
CREATE TABLE `mf_product_finishing` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `name` VARCHAR(50) UNIQUE NOT NULL
);

INSERT INTO `mf_product_finishing` (`name`) VALUES 
('Decapado'),
('Galvanizado'),
('Preto'),
('Zincado'),
('Zincor'),
('Corten'),
('Polida'),
('Verde'),
('Branco'),
('Cinza');

DROP TABLE IF EXISTS `mf_product_surface`;
CREATE TABLE `mf_product_surface` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `name` VARCHAR(50) UNIQUE NOT NULL
);
INSERT INTO `mf_product_surface` (`name`) VALUES 
('Liso'),
('Nervurado'),
('Ondulado'),
('Laminado'),
('Perfurado'),
('Xadrez'), 
('Série Média'),
('Série Ligeira'),
('Com Costura'),
('Sem Costura'),
('Gotas'),
('Abas'),
('Abas Iguais'),
('Abas Desiguais');

CREATE OR REPLACE VIEW vw_product_catalog AS
SELECT 
    pc.id,
    pc.product_code,
    pc.description,
    pc.unit,
    pc.stock_current,
    pc.currency,
    pc.price_pvp,
    pc.price_avg,
    pc.price_last,
    pc.date_last_entry,
    pc.date_last_exit,
    f.name AS family_name,
    t.name AS type_name,
    m.name AS material_name,
    s.name AS shape_name,
    fn.name AS finishing_name,
    pc.length,
    pc.width,
    pc.height
FROM 
    mf_product_catalog pc
LEFT JOIN mf_product_family f ON pc.family_id = f.id
LEFT JOIN mf_product_type t ON pc.type_id = t.id
LEFT JOIN mf_product_material m ON pc.material_id = m.id
LEFT JOIN mf_product_shape s ON pc.shape_id = s.id
LEFT JOIN mf_product_finishing fn ON pc.finishing_id = fn.id;

--
-- Table structure for table `mf_quote_request`
--

DROP TABLE IF EXISTS `mf_quote_request`;
CREATE TABLE `mf_quote_request` (
    `id` INT AUTO_INCREMENT PRIMARY KEY, 
    `quote_id_erp` INT,                          -- Corresponds to ID-ORC-ERP
    `quote_date` DATE,                       -- Corresponds to DATA
    `client_id` INT,                         -- Corresponds to ID_CLT
    `client_name` VARCHAR(255),              -- Corresponds to NOME_CLT
    `product_code` VARCHAR(50),              -- Corresponds to SKU
    `quantity_requested` DECIMAL(10,2),      -- Corresponds to QD
    `erp_price` DECIMAL(10,4),               -- Corresponds to PRECO_ERP
    `erp_price_modification_percent` DECIMAL(5,2), -- Corresponds to MODvsERP
    `alert_flag` TINYINT,                 -- Corresponds to ALERTA (0/1)
    `special_flag` TINYINT,               -- Corresponds to ESPECIAL (0/1)
    `final_price` DECIMAL(10,4),             -- Corresponds to PRECO_FINAL
    `order_quantity` DECIMAL(10,2),          -- Corresponds to QD ENCOMENDA
    `order_id` VARCHAR(50),                  -- Corresponds to ID ENCOMENDA
    `observation` TEXT,                      -- Corresponds to OBSERVACOES
    `unit_price` DECIMAL(10,4),              -- Corresponds to PREÇO_UN
    `margin_percent` DECIMAL(5,2),           -- Corresponds to MB_CL
    `price_difference_erp` DECIMAL(10,4),    -- Corresponds to DIF.P_ERP
    `price_difference_percent_erp` DECIMAL(5,2), -- Corresponds to DIF._ERP%
    `total_difference_erp` DECIMAL(15,2),    -- Corresponds to DIF_Total_ERP
    `total_difference_final` DECIMAL(15,5),  -- Corresponds to DIF_Total_FIN
    INDEX `idx_quote_request_erp_id` (`quote_id_erp`),
    INDEX `idx_quote_product_code` (`product_code`),      
    INDEX `idx_client_id` (`client_id`),                 
    INDEX `idx_quote_date` (`quote_date`),               
    INDEX `idx_order_id` (`order_id`) 
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


--
-- To import this table's data, there is a python script on this same folder, ./MasterFerroDataImports with the indications
-- and instructions to import the data to the mysql database needed, named mf_quote_request_data_import.py
-- 



--
-- Table structure for table `mf_client_rating`
--

DROP TABLE IF EXISTS `mf_client_rating`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mf_client_rating` (
	  `client_code` VARCHAR(50) NOT NULL, -- original id
	  `client_id` INT PRIMARY KEY,  -- datatype to be better determined
    `segment_id` INT NOT NULL,
    `zone` VARCHAR(50), -- still dont know the datatype
    `credit_rating` CHAR(1) NOT NULL,
    `payment_rating` CHAR(1) NOT NULL,
    `historical_volume_rating` CHAR(1) NOT NULL,
    `potential_volume_rating` CHAR(1) NOT NULL,
    `operational_cost_rating` CHAR(1) NOT NULL,
    `logistic_rating` CHAR(1) NOT NULL,
    `weighted_rating` DECIMAL(4,2) NOT NULL,
    FOREIGN KEY (`segment_id`) REFERENCES `mf_segment`(`id`),
    INDEX `idx_client_rating_id` (`client_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

INSERT INTO `mf_client_rating` (`client_code`, `client_id`, `segment_id`, `zone`, `credit_rating`, `payment_rating`, `historical_volume_rating`, `potential_volume_rating`, `operational_cost_rating`, `logistics_rating`, `weighted_rating`) VALUES
('15001', 1, 1, NULL, 'C', 'B', 'A', 'D', 'C', 'B', 0.61),
('14968', 2, 2, NULL, 'B', 'A', 'A', 'D', 'C', 'B', 0.71),
('10064', 3, 1, NULL, 'B', 'A', 'A', 'B', 'D', 'C', 0.79),
('5935', 4, 1, NULL, 'A', 'A', 'A', 'B', 'D', 'B', 0.86),
('14937', 5, 1, NULL, 'B', 'A', 'A', 'B', 'D', 'B', 0.83),
('13522', 6, 1, NULL, 'C', 'B', 'A', 'B', 'C', 'C', 0.74),
('15008', 7, 5, NULL, 'A', 'B', 'A', 'D', 'D', 'B', 0.66),
('14867', 8, 1, NULL, 'A', 'A', 'A', 'C', 'C', 'C', 0.80),
('8982', 9, 1, NULL, 'C', 'A', 'A', 'A', 'C', 'B', 0.84),
('1635', 10, 3, NULL, 'A', 'A', 'A', 'A', 'D', 'D', 0.80),
('7942', 11, 2, NULL, 'B', 'B', 'A', 'B', 'D', 'C', 0.76),
('P2063', 12, 2, NULL, 'A', 'C', 'A', 'B', 'C', 'B', 0.81),
('P2479', 13, 1, NULL, 'A', 'B', 'A', 'B', 'D', 'C', 0.79),
('2140', 14, 1, NULL, 'A', 'C', 'A', 'A', 'C', 'A', 0.85),
('13578', 15, 2, NULL, 'A', 'B', 'A', 'A', 'C', 'C', 0.87),
('14479', 16, 2, NULL, 'A', 'A', 'A', 'A', 'C', 'C', 0.90),
('8857', 17, 3, NULL, 'B', 'C', 'A', 'B', 'D', 'D', 0.64),
('P9274', 18, 1, NULL, 'A', 'C', 'A', 'A', 'C', 'C', 0.80),
('4107', 19, 1, NULL, 'B', 'C', 'A', 'A', 'D', 'C', 0.72),
('P2478', 20, 1, NULL, 'B', 'C', 'A', 'A', 'D', 'C', 0.72),
('1389', 21, 2, NULL, 'A', 'C', 'A', 'A', 'C', 'B', 0.84),
('1713', 22, 2, NULL, 'A', 'C', 'A', 'A', 'C', 'D', 0.75),
('13018', 23, 2, NULL, 'C', 'B', 'A', 'B', 'D', 'B', 0.73),
('10040', 24, 1, NULL, 'B', 'C', 'A', 'B', 'D', 'B', 0.73),
('10405', 25, 2, NULL, 'D', 'C', 'A', 'B', 'D', 'B', 0.56),
('8848', 26, 1, NULL, 'A', 'C', 'A', 'A', 'D', 'B', 0.79),
('10266', 27, 1, NULL, 'A', 'C', 'A', 'A', 'D', 'A', 0.80),
('11586', 28, 1, NULL, 'D', 'A', 'A', 'B', 'C', 'B', 0.71),
('8774', 29, 1, NULL, 'A', 'C', 'A', 'A', 'C', 'C', 0.80),
('7091', 30, 2, NULL, 'A', 'C', 'A', 'B', 'C', 'D', 0.72),
('13300', 31, 4, NULL, 'B', 'A', 'A', 'A', 'A', 'D', 0.87),
('13206', 32, 1, NULL, 'B', 'C', 'A', 'A', 'C', 'C', 0.77),
('13434', 33, 5, NULL, 'B', 'B', 'A', 'A', 'C', 'B', 0.88),
('12890', 34, 1, NULL, 'A', 'B', 'A', 'A', 'C', 'C', 0.87),
('12544', 35, 1, NULL, 'A', 'C', 'A', 'B', 'D', 'C', 0.72),
('14280', 36, 2, NULL, 'B', 'B', 'A', 'A', 'C', 'D', 0.79),
('14044', 37, 2, NULL, 'A', 'A', 'A', 'A', 'A', 'D', 0.90),
('14038', 38, 4, NULL, 'B', 'B', 'A', 'B', 'C', 'C', 0.81),
('12331', 39, 4, NULL, 'C', 'A', 'A', 'B', 'C', 'B', 0.81),
('R0711', 40, 4, NULL, 'B', 'B', 'A', 'A', 'A', 'C', 0.89),
('14289', 41, 2, NULL, 'A', 'A', 'A', 'A', 'C', 'D', 0.85),
('7942', 42, 2, NULL, 'B', 'B', 'A', 'A', 'C', 'C', 0.84),
('14808', 43, 2, NULL, 'B', 'A', 'A', 'B', 'C', 'D', 0.79),
('13110', 44, 4, NULL, 'B', 'A', 'A', 'A', 'A', 'B', 0.96);


--
-- Table structure for table `mf_weighting`
--


DROP TABLE IF EXISTS `mf_rating_weighting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mf_rating_weighting` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,    
    `description` VARCHAR(100) NOT NULL,          
    `value` DECIMAL(3,2) NOT NULL 
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

INSERT INTO `mf_rating_weighting` (`description`, `value`) VALUES
('Risco Financeiro | Rating CESCE', 0.20),
('Prazo de pagamento \\ cumprimento', 0.20),
('Volume Histórico', 0.20),
('Volume Potencial', 0.20),
('Custo Operação', 0.10),
('Custo Logístico', 0.10);


--
-- Table structure for table `mf_rating_discount`
--

DROP TABLE IF EXISTS `mf_rating_discount`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mf_rating_discount` (
    `rating` CHAR(1) PRIMARY KEY,
    `percentage` DECIMAL(5,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

INSERT INTO `mf_rating_discount` (`rating`, `percentage`) VALUES
('A', 100.00),
('B', 85.00),
('C', 50.00),
('D', 0.00);


DROP TABLE IF EXISTS `mf_rating_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE mf_rating_type (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `description` VARCHAR(50) NOT NULL UNIQUE,  -- Ensures rating type is unique
    `slug` VARCHAR(50) NOT NULL UNIQUE          -- Slug to be used as a URL-friendly version of description
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

INSERT INTO mf_rating_type (`description`, `slug`) VALUES
('Credit', 'credit'),
('Payment Compliance', 'payment-compliance'),
('Historical Volume', 'historical-volume'),
('Potential Volume', 'potential-volume'),
('Operational Cost', 'operational-cost'),
('Logistic', 'logistic');



--
-- Table structure for table `mf_rating_credit`
--

DROP TABLE IF EXISTS `mf_rating_criteria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE mf_rating_criteria (
    `id` INT NOT NULL UNIQUE,
    `rating_type_id` INT NOT NULL,        -- Foreign key to rating type
    `rating` CHAR(1) NOT NULL,            -- The rating (e.g., 'A', 'B', 'C', etc.)
    `criteria` VARCHAR(50) NOT NULL,      -- The criteria associated with the rating
    PRIMARY KEY (`rating_type_id`, `rating`), -- Composite key ensures uniqueness
    FOREIGN KEY (`rating_type_id`) REFERENCES mf_rating_type(`id`), -- Foreign key to rating type
    FOREIGN KEY (`rating`) REFERENCES mf_rating_discount(`rating`) -- Foreign key to discount rating
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Inserts for Credit Ratings (rating_type_id = 1)
INSERT INTO mf_rating_criteria (rating_type_id, rating, criteria) VALUES
(1, 'A', '0-2'),
(1, 'B', '3-5'),
(1, 'C', '6-7'),
(1, 'D', 'S/ SEGURO CREDITO');
-- Inserts for Payment Compliance Ratings (rating_type_id = 2)
INSERT INTO mf_rating_criteria (rating_type_id, rating, criteria) VALUES
(2, 'A', '<= 30 DIAS & CUMPRE'),
(2, 'B', '<= 60 DIAS & CUMPRE'),
(2, 'C', '> 60 DIAS & CUMPRE'),
(2, 'D', 'NÃO CUMPRE (>15DIAS DE ATRASO)');
(2, 'D', 'NÃO CUMPRE (>15DIAS DE ATRASO)');
-- Inserts for Historical Volume Ratings (rating_type_id = 3)
INSERT INTO mf_rating_criteria (rating_type_id, rating, criteria) VALUES
(3, 'A', '<=80% Total de vendas'),
(3, 'B', '<=95% Total de vendas'),
(3, 'C', '<=100% Total de vendas'),
(3, 'D', 's/histórico');

-- Inserts for Historical Volume Ratings (rating_type_id = 3)
INSERT INTO mf_rating_criteria (rating_type_id, rating, criteria) VALUES
(3, 'A', '<=80% Total de vendas'),
(3, 'B', '<=95% Total de vendas'),
(3, 'C', '<=100% Total de vendas'),
(3, 'D', 's/histórico');

-- Inserts for Potential Volume Ratings (rating_type_id = 4)
INSERT INTO mf_rating_criteria (rating_type_id, rating, criteria) VALUES
(4, 'A', 'Elevado (cliente A)'),
(4, 'B', 'Médio (cliente B)'),
(4, 'C', 'Baixo (cliente C)'),
(4, 'D', 'N/A');

-- Inserts for Operational Cost Ratings (rating_type_id = 5)
INSERT INTO mf_rating_criteria (rating_type_id, rating, criteria) VALUES
(5, 'A', 'ATADOS COMPLETOS'),
(5, 'B', ''),
(5, 'C', 'RETALHO'),
(5, 'D', 'MIX RETALHO');

-- Inserts for Logistic Ratings (rating_type_id = 6)
INSERT INTO mf_rating_criteria (rating_type_id, rating, criteria) VALUES
(6, 'A', 'S/TRANSPORTE'),
(6, 'B', '<= 30 KM'),
(6, 'C', '<= 60 KM'),
(6, 'D', '> 60 KM');


CREATE INDEX idx_filtered_email_date ON filtered_email(date);
CREATE INDEX idx_reply_date ON reply(date);
CREATE INDEX idx_email_date ON email(date);

DROP TABLE IF EXISTS  `order`;
CREATE TABLE `order` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email_token` varchar(250) DEFAULT NULL,
  `token` varchar(250) NOT NULL,
  `postal_code` varchar(8) DEFAULT NULL,
  `address` varchar(250) DEFAULT NULL,
  `city` varchar(100) DEFAULT NULL,
  `is_draft` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `token_unique` (`token`), -- Added a unique key on `token`
  KEY `email_token_idx` (`email_token`),
  CONSTRAINT `email_address_ibfk_1` FOREIGN KEY (`email_token`) REFERENCES `filtered_email` (`token`) ON DELETE CASCADE,
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `order_product`;
CREATE TABLE `order_product` (
  `id` int NOT NULL AUTO_INCREMENT,
  `order_token` varchar(250) NOT NULL,
  `product_catalog_id` int NOT NULL,
  `quantity` int NOT NULL,
  `quantity_type` varchar(100) NOT NULL,
  `calculated_price` varchar(100) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_order_product_order_token_idx` (`order_token`),
  CONSTRAINT `fk_order_product_order` FOREIGN KEY (`order_token`) REFERENCES `order` (`token`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_order_product_product_catalog_id` FOREIGN KEY (`product_catalog_id`) REFERENCES `mf_product_catalog` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DROP TABLE IF EXISTS  `cancel_reason`;
CREATE TABLE `cancel_reason` (
    id INT AUTO_INCREMENT PRIMARY KEY,
    reason VARCHAR(50) NOT NULL,
    slug VARCHAR(50) UNIQUE NOT NULL,
    description VARCHAR(255),
    is_active tinyint DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO department VALUES (7, "configurations");


ALTER TABLE `order`
ADD COLUMN `cancel_reason_id` INT DEFAULT NULL,
ADD COLUMN `canceled_by` VARCHAR(250) DEFAULT NULL,
ADD COLUMN `canceled_at` DATETIME DEFAULT NULL,
ADD CONSTRAINT `order_cancel_reason_fk` FOREIGN KEY (`cancel_reason_id`) REFERENCES `cancel_reason` (`id`);

ALTER TABLE mf_product_catalog
    ADD COLUMN `thickness` DECIMAL(10,2) DEFAULT NULL,      
    ADD COLUMN `diameter` DECIMAL(10,2)DEFAULT NULL;

    

INSERT INTO `status` VALUES (13, 'Cancelado');
UPDATE `status` SET `description` = 'Cancelado por Operador' WHERE `id` = 13;
INSERT INTO status (`id`, `description`) VALUES (14, "Confirmado por Operador"), (15, "Confirmado por Cliente");
INSERT INTO status (`id`, `description`) VALUES (16, "Cancelado por Cliente");
INSERT INTO status (`id`, `description`) VALUES (17, "Enviado para Primavera");




--
--
--
--
-- NEW ARCHITECTURE FOR DYNAMIC RATINGS
--
--
--
--
--


--
-- Table structure for table `mf_client_rating`
--

DROP TABLE IF EXISTS `mf_client_rating`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mf_client_rating` (
	  `client_code` VARCHAR(50) NOT NULL, -- original id
	  `client_id` INT PRIMARY KEY,  -- datatype to be better determined
    `segment_id` INT NOT NULL,
    `zone` VARCHAR(50), -- still dont know the datatype
    `credit_rating` CHAR(1) NOT NULL,
    `payment_rating` CHAR(1) NOT NULL,
    `historical_volume_rating` CHAR(1) NOT NULL,
    `potential_volume_rating` CHAR(1) NOT NULL,
    `operational_cost_rating` CHAR(1) NOT NULL,
    `logistic_rating` CHAR(1) NOT NULL,
    `weighted_rating` DECIMAL(4,2) NOT NULL,
    FOREIGN KEY (`segment_id`) REFERENCES `mf_segment`(`id`),
    INDEX `idx_client_rating_id` (`client_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

INSERT INTO `mf_client_rating` (`client_code`, `client_id`, `segment_id`, `zone`, `credit_rating`, `payment_rating`, `historical_volume_rating`, `potential_volume_rating`, `operational_cost_rating`, `logistics_rating`, `weighted_rating`) VALUES
('015001', 1, 1, NULL, 'C', 'B', 'A', 'D', 'C', 'B', 0.61),
('014968', 2, 2, NULL, 'B', 'A', 'A', 'D', 'C', 'B', 0.71),
('010064', 3, 1, NULL, 'B', 'A', 'A', 'B', 'D', 'C', 0.79),
('005935', 4, 1, NULL, 'A', 'A', 'A', 'B', 'D', 'B', 0.86),
('014937', 5, 1, NULL, 'B', 'A', 'A', 'B', 'D', 'B', 0.83),
('013522', 6, 1, NULL, 'C', 'B', 'A', 'B', 'C', 'C', 0.74),
('015008', 7, 5, NULL, 'A', 'B', 'A', 'D', 'D', 'B', 0.66),
('014867', 8, 1, NULL, 'A', 'A', 'A', 'C', 'C', 'C', 0.80),
('008982', 9, 1, NULL, 'C', 'A', 'A', 'A', 'C', 'B', 0.84),
('001635', 10, 3, NULL, 'A', 'A', 'A', 'A', 'D', 'D', 0.80),
('007942', 11, 2, NULL, 'B', 'B', 'A', 'B', 'D', 'C', 0.76),
('P2063', 12, 2, NULL, 'A', 'C', 'A', 'B', 'C', 'B', 0.81),
('P2479', 13, 1, NULL, 'A', 'B', 'A', 'B', 'D', 'C', 0.79),
('002140', 14, 1, NULL, 'A', 'C', 'A', 'A', 'C', 'A', 0.85),
('013578', 15, 2, NULL, 'A', 'B', 'A', 'A', 'C', 'C', 0.87),
('014479', 16, 2, NULL, 'A', 'A', 'A', 'A', 'C', 'C', 0.90),
('008857', 17, 3, NULL, 'B', 'C', 'A', 'B', 'D', 'D', 0.64),
('P9274', 18, 1, NULL, 'A', 'C', 'A', 'A', 'C', 'C', 0.80),
('004107', 19, 1, NULL, 'B', 'C', 'A', 'A', 'D', 'C', 0.72),
('P2478', 20, 1, NULL, 'B', 'C', 'A', 'A', 'D', 'C', 0.72),
('001389', 21, 2, NULL, 'A', 'C', 'A', 'A', 'C', 'B', 0.84),
('001713', 22, 2, NULL, 'A', 'C', 'A', 'A', 'C', 'D', 0.75),
('013018', 23, 2, NULL, 'C', 'B', 'A', 'B', 'D', 'B', 0.73),
('010040', 24, 1, NULL, 'B', 'C', 'A', 'B', 'D', 'B', 0.73),
('010405', 25, 2, NULL, 'D', 'C', 'A', 'B', 'D', 'B', 0.56),
('008848', 26, 1, NULL, 'A', 'C', 'A', 'A', 'D', 'B', 0.79),
('010266', 27, 1, NULL, 'A', 'C', 'A', 'A', 'D', 'A', 0.80),
('011586', 28, 1, NULL, 'D', 'A', 'A', 'B', 'C', 'B', 0.71),
('008774', 29, 1, NULL, 'A', 'C', 'A', 'A', 'C', 'C', 0.80),
('007091', 30, 2, NULL, 'A', 'C', 'A', 'B', 'C', 'D', 0.72),
('013300', 31, 4, NULL, 'B', 'A', 'A', 'A', 'A', 'D', 0.87),
('013206', 32, 1, NULL, 'B', 'C', 'A', 'A', 'C', 'C', 0.77),
('013434', 33, 5, NULL, 'B', 'B', 'A', 'A', 'C', 'B', 0.88),
('012890', 34, 1, NULL, 'A', 'B', 'A', 'A', 'C', 'C', 0.87),
('012544', 35, 1, NULL, 'A', 'C', 'A', 'B', 'D', 'C', 0.72),
('014280', 36, 2, NULL, 'B', 'B', 'A', 'A', 'C', 'D', 0.79),
('014044', 37, 2, NULL, 'A', 'A', 'A', 'A', 'A', 'D', 0.90),
('014038', 38, 4, NULL, 'B', 'B', 'A', 'B', 'C', 'C', 0.81),
('012331', 39, 4, NULL, 'C', 'A', 'A', 'B', 'C', 'B', 0.81),
('R0711', 40, 4, NULL, 'B', 'B', 'A', 'A', 'A', 'C', 0.89),
('014289', 41, 2, NULL, 'A', 'A', 'A', 'A', 'C', 'D', 0.85),
('007942', 42, 2, NULL, 'B', 'B', 'A', 'A', 'C', 'C', 0.84),
('014808', 43, 2, NULL, 'B', 'A', 'A', 'B', 'C', 'D', 0.79),
('013110', 44, 4, NULL, 'B', 'A', 'A', 'A', 'A', 'B', 0.96);


ALTER TABLE mf_client_rating RENAME TO mf_client;


ALTER TABLE `mf_client`
    DROP COLUMN `credit_rating`,
    DROP COLUMN `payment_rating`,
    DROP COLUMN `historical_volume_rating`,
    DROP COLUMN `potential_volume_rating`,
    DROP COLUMN `operational_cost_rating`,
    DROP COLUMN `logistic_rating`,
    DROP COLUMN `weighted_rating`,
    DROP COLUMN `zone`;

ALTER TABLE `mf_client` DROP PRIMARY KEY;


ALTER TABLE `mf_client`
DROP COLUMN `zone`;

ALTER TABLE `mf_client`
    CHANGE COLUMN `client_id` `id` INT PRIMARY KEY,
    ADD INDEX `idx_client_code` (`client_code`);
    
ALTER TABLE `mf_client`
    MODIFY COLUMN `id` INT NOT NULL AUTO_INCREMENT FIRST,
    CHANGE COLUMN `client_code` `code` VARCHAR(50) NOT NULL;
    
    
DROP TABLE IF EXISTS `mf_rating_weighting`;

-- Modify the `mf_rating_type` table to add the `weight` column
ALTER TABLE `mf_rating_type`
    ADD COLUMN `weight` DECIMAL(3,2) NOT NULL;



-- Create a table to connect clients to many ratings
CREATE TABLE `mf_client_rating` (
    `client_code` VARCHAR(50) NOT NULL,
    `rating_type_id` INT NOT NULL,
    `rating` CHAR(1) NOT NULL,
    PRIMARY KEY (`client_code`, `rating_type_id`),  -- Composite primary key
    FOREIGN KEY (`client_code`) REFERENCES `mf_client`(`code`) ON DELETE CASCADE,
    FOREIGN KEY (`rating_type_id`) REFERENCES `mf_rating_type`(`id`) ON DELETE CASCADE,
    FOREIGN KEY (`rating`) REFERENCES `mf_rating_discount`(`rating`) ON DELETE CASCADE,
    INDEX `idx_client_rating` (`client_code`, `rating_type_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- Insert weight values into the `mf_rating_type` table
UPDATE `mf_rating_type`
SET `weight` = 0.20 WHERE `description` IN ('Credit', 'Payment Compliance', 'Historical Volume', 'Potential Volume');
UPDATE `mf_rating_type`
SET `weight` = 0.10 WHERE `description` IN ('Operational Cost', 'Logistic');


INSERT INTO mf_client_rating (client_code, rating_type_id, rating) VALUES
-- Client 15001
('015001', 1, 'C'),  -- Credit
('015001', 2, 'B'),  -- Payment Compliance
('015001', 3, 'A'),  -- Historical Volume
('015001', 4, 'D'),  -- Potential Volume
('015001', 5, 'C'),  -- Operational Cost
('015001', 6, 'B'),  -- Logistic

-- Client 14968
('014968', 1, 'B'),
('014968', 2, 'A'),
('014968', 3, 'A'),
('014968', 4, 'D'),
('014968', 5, 'C'),
('014968', 6, 'B'),

-- Client 10064
('010064', 1, 'B'),
('010064', 2, 'A'),
('010064', 3, 'A'),
('010064', 4, 'B'),
('010064', 5, 'D'),
('010064', 6, 'C'),

-- Client 5935
('005935', 1, 'A'),
('005935', 2, 'A'),
('005935', 3, 'A'),
('005935', 4, 'B'),
('005935', 5, 'D'),
('005935', 6, 'C'),

-- Client 14937
('014937', 1, 'B'),
('014937', 2, 'A'),
('014937', 3, 'A'),
('014937', 4, 'B'),
('014937', 5, 'D'),
('014937', 6, 'B'),

-- Client 13522
('013522', 1, 'C'),
('013522', 2, 'B'),
('013522', 3, 'A'),
('013522', 4, 'B'),
('013522', 5, 'C'),
('013522', 6, 'A'),

-- Client 15008
('015008', 1, 'A'),
('015008', 2, 'B'),
('015008', 3, 'A'),
('015008', 4, 'D'),
('015008', 5, 'D'),
('015008', 6, 'A'),

-- Client 14867
('014867', 1, 'A'),
('014867', 2, 'A'),
('014867', 3, 'A'),
('014867', 4, 'C'),
('014867', 5, 'C'),
('014867', 6, 'B'),

-- Client 8982
('008982', 1, 'C'),
('008982', 2, 'A'),
('008982', 3, 'A'),
('008982', 4, 'A'),
('008982', 5, 'C'),
('008982', 6, 'D'),

-- Client 1635
('001635', 1, 'A'),
('001635', 2, 'A'),
('001635', 3, 'A'),
('001635', 4, 'A'),
('001635', 5, 'D'),
('001635', 6, 'A'),

-- Client 7942
('007942', 1, 'B'),
('007942', 2, 'B'),
('007942', 3, 'A'),
('007942', 4, 'B'),
('007942', 5, 'D'),
('007942', 6, 'C'),

-- Client P2063
('P2063', 1, 'A'),
('P2063', 2, 'C'),
('P2063', 3, 'A'),
('P2063', 4, 'B'),
('P2063', 5, 'C'),
('P2063', 6, 'A'),

-- Client P2479
('P2479', 1, 'A'),
('P2479', 2, 'B'),
('P2479', 3, 'A'),
('P2479', 4, 'B'),
('P2479', 5, 'D'),
('P2479', 6, 'B'),

-- Client 2140
('002140', 1, 'A'),
('002140', 2, 'C'),
('002140', 3, 'A'),
('002140', 4, 'A'),
('002140', 5, 'C'),
('002140', 6, 'B'),

-- Client 13578
('013578', 1, 'A'),
('013578', 2, 'B'),
('013578', 3, 'A'),
('013578', 4, 'A'),
('013578', 5, 'C'),
('013578', 6, 'D'),

-- Client 14479
('014479', 1, 'A'),
('014479', 2, 'A'),
('014479', 3, 'A'),
('014479', 4, 'A'),
('014479', 5, 'C'),
('014479', 6, 'B'),

-- Client 8857
('008857', 1, 'B'),
('008857', 2, 'C'),
('008857', 3, 'A'),
('008857', 4, 'B'),
('008857', 5, 'D'),
('008857', 6, 'C'),

-- Client P9274
('P9274', 1, 'A'),
('P9274', 2, 'C'),
('P9274', 3, 'A'),
('P9274', 4, 'A'),
('P9274', 5, 'C'),
('P9274', 6, 'B'),

-- Client 4107
('004107', 1, 'B'),
('004107', 2, 'C'),
('004107', 3, 'A'),
('004107', 4, 'A'),
('004107', 5, 'D'),
('004107', 6, 'C'),

-- Client P2478
('P2478', 1, 'B'),
('P2478', 2, 'C'),
('P2478', 3, 'A'),
('P2478', 4, 'A'),
('P2478', 5, 'D'),
('P2478', 6, 'A'),

-- Client 1389
('001389', 1, 'A'),
('001389', 2, 'C'),
('001389', 3, 'A'),
('001389', 4, 'A'),
('001389', 5, 'C'),
('001389', 6, 'B'),

-- Client 1713
('001713', 1, 'A'),
('001713', 2, 'C'),
('001713', 3, 'A'),
('001713', 4, 'A'),
('001713', 5, 'D'),
('001713', 6, 'C'),

-- Client 13018
('013018', 1, 'C'),
('013018', 2, 'B'),
('013018', 3, 'A'),
('013018', 4, 'B'),
('013018', 5, 'D'),
('013018', 6, 'C'),

-- Client 10040
('010040', 1, 'B'),
('010040', 2, 'C'),
('010040', 3, 'A'),
('010040', 4, 'B'),
('010040', 5, 'D'),
('010040', 6, 'A'),

-- Client 10405
('010405', 1, 'D'),
('010405', 2, 'C'),
('010405', 3, 'A'),
('010405', 4, 'B'),
('010405', 5, 'D'),
('010405', 6, 'C'),

-- Client 8848
('008848', 1, 'A'),
('008848', 2, 'C'),
('008848', 3, 'A'),
('008848', 4, 'A'),
('008848', 5, 'D'),
('008848', 6, 'A'),

-- Client 10266
('010266', 1, 'A'),
('010266', 2, 'C'),
('010266', 3, 'A'),
('010266', 4, 'A'),
('010266', 5, 'D'),
('010266', 6, 'B'),

-- Client 11586
('011586', 1, 'D'),
('011586', 2, 'A'),
('011586', 3, 'A'),
('011586', 4, 'B'),
('011586', 5, 'C'),
('011586', 6, 'A'),

-- Client 8774
('008774', 1, 'A'),
('008774', 2, 'C'),
('008774', 3, 'A'),
('008774', 4, 'A'),
('008774', 5, 'C'),
('008774', 6, 'B'),

-- Client 7091
('007091', 1, 'A'),
('007091', 2, 'C'),
('007091', 3, 'A'),
('007091', 4, 'B'),
('007091', 5, 'C'),
('007091', 6, 'D'),

-- Client 13300
('013300', 1, 'B'),
('013300', 2, 'A'),
('013300', 3, 'A'),
('013300', 4, 'A'),
('013300', 5, 'D'),
('013300', 6, 'B'),

-- Client 13206
('013206', 1, 'B'),
('013206', 2, 'C'),
('013206', 3, 'A'),
('013206', 4, 'A'),
('013206', 5, 'C'),
('013206', 6, 'B'),

-- Client 13434
('013434', 1, 'B'),
('013434', 2, 'B'),
('013434', 3, 'A'),
('013434', 4, 'A'),
('013434', 5, 'C'),
('013434', 6, 'D'),

-- Client 12890
('012890', 1, 'A'),
('012890', 2, 'B'),
('012890', 3, 'A'),
('012890', 4, 'A'),
('012890', 5, 'C'),
('012890', 6, 'B'),

-- Client 12544
('012544', 1, 'A'),
('012544', 2, 'C'),
('012544', 3, 'A'),
('012544', 4, 'B'),
('012544', 5, 'D'),
('012544', 6, 'A'),

-- Client 14280
('014280', 1, 'B'),
('014280', 2, 'B'),
('014280', 3, 'A'),
('014280', 4, 'A'),
('014280', 5, 'C'),
('014280', 6, 'B'),

-- Client 14044
('014044', 1, 'A'),
('014044', 2, 'A'),
('014044', 3, 'A'),
('014044', 4, 'A'),
('014044', 5, 'D'),
('014044', 6, 'A'),

-- Client 14038
('014038', 1, 'B'),
('014038', 2, 'B'),
('014038', 3, 'A'),
('014038', 4, 'B'),
('014038', 5, 'C'),
('014038', 6, 'D'),

-- Client 12331
('012331', 1, 'C'),
('012331', 2, 'A'),
('012331', 3, 'A'),
('012331', 4, 'B'),
('012331', 5, 'C'),
('012331', 6, 'B'),

-- Client R0711
('R0711', 1, 'B'),
('R0711', 2, 'B'),
('R0711', 3, 'A'),
('R0711', 4, 'A'),
('R0711', 5, 'C'),
('R0711', 6, 'B'),

-- Client 14289
('014289', 1, 'A'),
('014289', 2, 'A'),
('014289', 3, 'A'),
('014289', 4, 'A'),
('014289', 5, 'D'),
('014289', 6, 'C'),

-- Client 14808
('014808', 1, 'B'),
('014808', 2, 'A'),
('014808', 3, 'A'),
('014808', 4, 'B'),
('014808', 5, 'C'),
('014808', 6, 'A'),

-- Client 13110
('013110', 1, 'B'),
('013110', 2, 'A'),
('013110', 3, 'A'),
('013110', 4, 'A'),
('013110', 5, 'B'),
('013110', 6, 'A');




ALTER TABLE order_product
ADD COLUMN confidence INT DEFAULT 0,  -- 0 to 100 for confidence
ADD COLUMN is_instant_match TINYINT(1) DEFAULT 0,  -- 0 for false, 1 for true
ADD COLUMN is_manual_insert TINYINT(1) DEFAULT 0;  -- 0 for false, 1 for true


CREATE TABLE `mf_pricing_strategy` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `name` VARCHAR(255) NOT NULL,
    `slug` VARCHAR(255) NOT NULL UNIQUE,
    `created_at` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `updated_at` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Inserção de exemplos de estratégias de precificação
INSERT INTO `mf_pricing_strategy` (`id`, `name`, `slug`) VALUES
(1, 'Last Price', 'last-price'),
(2, 'Average Price', 'average-price');

ALTER TABLE `t_product_catalog`
ADD COLUMN `pricing_strategy_id` INT NOT NULL DEFAULT 1,
ADD CONSTRAINT `fk_product_catalog_pricing_strategy`
FOREIGN KEY (`pricing_strategy_id`) REFERENCES `mf_pricing_strategy`(`id`)
ON DELETE CASCADE
ON UPDATE CASCADE;

ALTER TABLE `mf_client_rating`
ADD COLUMN `created_at` TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
ADD COLUMN `created_by` VARCHAR(50) DEFAULT NULL,
ADD COLUMN `updated_at` TIMESTAMP DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
ADD COLUMN `updated_by` VARCHAR(50) DEFAULT NULL;


ALTER TABLE `mf_client`
ADD COLUMN `created_at` TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
ADD COLUMN `created_by` VARCHAR(50) DEFAULT NULL,
ADD COLUMN `updated_at` TIMESTAMP DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
ADD COLUMN `updated_by` VARCHAR(50) DEFAULT NULL;


-- Change foreign key for t_product_catalog
ALTER TABLE `order_product`
DROP FOREIGN KEY `fk_order_product_product_catalog_id`;

SET SQL_SAFE_UPDATES = 0;

DELETE FROM `order_product`
WHERE NOT EXISTS (
    SELECT 1 
    FROM `t_product_catalog` tpc
    WHERE tpc.`id` = `order_product`.`product_catalog_id` AND id > 0
) AND id > 0;

SET SQL_SAFE_UPDATES = 1;

ALTER TABLE `order_product`
ADD CONSTRAINT `fk_order_product_product_catalog_id`
FOREIGN KEY (`product_catalog_id`) REFERENCES `t_product_catalog` (`id`)
ON DELETE CASCADE ON UPDATE CASCADE;


-- Reference client code on the order
ALTER TABLE `order`
ADD COLUMN `client_code` varchar(50) DEFAULT NULL;

ALTER TABLE `order`
ADD CONSTRAINT `fk_order_client_code`
FOREIGN KEY (`client_code`) REFERENCES `mf_client` (`code`)
ON DELETE SET NULL;  -- Or other action like CASCADE, depending on your requirements


CREATE TABLE product_size (
    id INT AUTO_INCREMENT PRIMARY KEY,
    abbreviation VARCHAR(10) NOT NULL,
    name VARCHAR(50) NOT NULL,
    slug VARCHAR(50) NOT NULL
);

INSERT INTO product_size (id, abbreviation, name, slug)
VALUES 
    (1,'KG', 'Kilogram', 'kilogram'),
    (2,'MT', 'Meter', 'meter'),
    (3,'UN', 'Unit', 'unit'),
    (4,'M2', 'Square Meter', 'square-meter'),
    (5,'RL', 'Roll', 'roll'),
    (6,'ML', 'Milliliter', 'milliliter');

-- Add foreign key to the product size table
ALTER TABLE `order_product`
ADD COLUMN `product_size_id` int DEFAULT 1 NOT NULL AFTER `product_catalog_id`;

UPDATE `order_product`
SET `product_size_id` = 1 WHERE id > 0;  -- Assuming 1 is a valid ID in the product_size table

ALTER TABLE `order_product`
ADD CONSTRAINT `fk_order_product_product_size_id`
    FOREIGN KEY (`product_size_id`)
    REFERENCES `product_size` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE;

-- Remove the `quantity_unit` column
ALTER TABLE `order_product`
DROP COLUMN `quantity_unit`;

-- Modify `product_size_id` to set the default value to 1
ALTER TABLE `order_product`
ALTER COLUMN `product_size_id` SET DEFAULT 1;



--
-- TABLE transport
--
CREATE TABLE IF NOT EXISTS transport (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    slug VARCHAR(100) NOT NULL,  -- Slug for SEO-friendly URLs or identifiers
    description TEXT            -- Description of the transport type
);

-- Insert the 3 transport types
INSERT INTO transport (transport_type, slug, description)
VALUES 
    ('Small', 'small', 'Small transport suitable for light items'),
    ('Medium', 'medium', 'Medium transport for regular size items'),
    ('Large', 'large', 'Large transport for heavy and bulky items');




-- Add new columns to the 'order' table
ALTER TABLE `order`
ADD COLUMN `confirmed_by` VARCHAR(250) DEFAULT NULL,      -- Add column for who confirmed the order
ADD COLUMN `confirmed_at` DATETIME DEFAULT NULL;          -- Add column for when the order was confirmed

ALTER TABLE `order`
ADD COLUMN `transport_id` INT; -- Foreign key to transport table

-- Ensure the foreign key constraint to the 'transport' table is in place
ALTER TABLE `order`
ADD CONSTRAINT `fk_transport_id` FOREIGN KEY (`transport_id`) REFERENCES `transport` (`id`) ON DELETE SET NULL;

--
-- PRODUCT CONVERSIONS
--
CREATE TABLE IF NOT EXISTS `product_conversion` (
  `id` int NOT NULL AUTO_INCREMENT,
  `product_code` varchar(50) NOT NULL,
  `origin_unit_id` int NOT NULL,
  `end_unit_id` int NOT NULL,
  `rate` decimal(22,15) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `product_code` (`product_code`),
  KEY `origin_unit_id` (`origin_unit_id`),
  KEY `end_unit_id` (`end_unit_id`),
  CONSTRAINT `product_conversion_ibfk_1` FOREIGN KEY (`product_code`) REFERENCES `t_product_catalog` (`product_code`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `product_conversion_ibfk_2` FOREIGN KEY (`origin_unit_id`) REFERENCES `product_unit` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `product_conversion_ibfk_3` FOREIGN KEY (`end_unit_id`) REFERENCES `product_unit` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4614 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `order_product` (
  `id` int NOT NULL AUTO_INCREMENT,
  `order_token` varchar(250) NOT NULL,
  `product_catalog_id` int NOT NULL,
  `product_unit_id` int NOT NULL DEFAULT '1',
  `quantity` decimal(10,2) NOT NULL,
  `calculated_price` decimal(10,2) NOT NULL,
  `confidence` int DEFAULT '0',
  `is_instant_match` tinyint(1) DEFAULT '0',
  `is_manual_insert` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `fk_order_product_order_token_idx` (`order_token`),
  KEY `fk_order_product_product_catalog_id` (`product_catalog_id`),
  KEY `fk_order_product_product_size_id` (`product_unit_id`),
  CONSTRAINT `fk_order_product_order` FOREIGN KEY (`order_token`) REFERENCES `order` (`token`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_order_product_product_catalog_id` FOREIGN KEY (`product_catalog_id`) REFERENCES `t_product_catalog` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_order_product_product_size_id` FOREIGN KEY (`product_unit_id`) REFERENCES `product_unit` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=590 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Spam
INSERT INTO category VALUES (8, 'Spam', 'spam');

-- is_delivery
ALTER TABLE `order`
ADD COLUMN `is_delivery` TINYINT(1) NOT NULL DEFAULT '1' AFTER `token`;


-- add audits to products
ALTER TABLE `mf_product_catalog`
ADD COLUMN `created_at` DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
ADD COLUMN `created_by` VARCHAR(250) DEFAULT "lucas.remigio@engibots.com",
ADD COLUMN `updated_at` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
ADD COLUMN `updated_by` VARCHAR(250) DEFAULT NULL;

ALTER TABLE `t_product_catalog`
ADD COLUMN `created_at` DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
ADD COLUMN `created_by` VARCHAR(250) DEFAULT "lucas.remigio@engibots.com",
ADD COLUMN `updated_at` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
ADD COLUMN `updated_by` VARCHAR(250) DEFAULT NULL;

-- add more decimal places to prices
ALTER TABLE `t_product_catalog`
MODIFY COLUMN `price_pvp` DECIMAL(20,10) DEFAULT '0.0000000000',
MODIFY COLUMN `price_avg` DECIMAL(20,10) DEFAULT NULL,
MODIFY COLUMN `price_last` DECIMAL(20,10) DEFAULT NULL;

-- eliminate not needed table
DROP TABLE email_product;

ALTER TABLE email_forward ADD COLUMN message VARCHAR(1000) DEFAULT NULL;

-- create order ratings
CREATE TABLE `order_rating` (
  `order_token` VARCHAR(250) NOT NULL,
  `rating_type_id` int NOT NULL,
  `rating` char(1) NOT NULL,
  `updated_at` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `updated_by` varchar(50) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `created_by` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`order_token`,`rating_type_id`),
  KEY `rating_type_id` (`rating_type_id`),
  KEY `rating` (`rating`),
  KEY `idx_order_rating` (`order_token`,`rating_type_id`),
  CONSTRAINT `mf_order_rating_ibfk_1` FOREIGN KEY (`order_token`) REFERENCES `order` (`token`) ON DELETE CASCADE,
  CONSTRAINT `mf_order_rating_ibfk_2` FOREIGN KEY (`rating_type_id`) REFERENCES `mf_rating_type` (`id`) ON DELETE CASCADE,
  CONSTRAINT `mf_order_rating_ibfk_3` FOREIGN KEY (`rating`) REFERENCES `mf_rating_discount` (`rating`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

ALTER TABLE `order`
MODIFY COLUMN is_delivery TINYINT(1) NOT NULL DEFAULT '1';

-- Order Rating Change Request
CREATE TABLE IF NOT EXISTS `order_rating_change_request` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `order_token` VARCHAR(250) NOT NULL,
  `rating_type_id` INT NOT NULL,
  `new_rating` CHAR(1) NOT NULL,
  `status` ENUM('pending', 'accepted', 'rejected') NOT NULL DEFAULT 'pending',
  `requested_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `requested_by` VARCHAR(50) NOT NULL,
  `verified_at` TIMESTAMP NULL DEFAULT NULL,
  `verified_by` VARCHAR(50) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_order_rating` (`order_token`, `rating_type_id`),
  CONSTRAINT `ord_req_fk_order` FOREIGN KEY (`order_token`) REFERENCES `order` (`token`) ON DELETE CASCADE,
  CONSTRAINT `ord_req_fk_rating_type` FOREIGN KEY (`rating_type_id`) REFERENCES `mf_rating_type` (`id`) ON DELETE CASCADE,
  CONSTRAINT `ord_req_fk_rating_discount` FOREIGN KEY (`new_rating`) REFERENCES `mf_rating_discount` (`rating`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

ALTER TABLE order_rating_change_request
ADD KEY `idx_order_token` (`order_token`);

ALTER TABLE `order`
ADD KEY `idx_order_token` (`token`);

-- new filteredEmail status
INSERT INTO status(`id`, `description`) VALUES (18, "Pendente Aprovação Administrac");

-- New role for those who will approve the change requests
INSERT INTO user_role (`user_role_id`,`user_role`) VALUES(4, "SUPERVISOR");

ALTER TABLE order_product ADD COLUMN `price_discount` decimal(10,2) NOT NULL;

CREATE TABLE order_primavera_document (
	id int NOT NULL AUTO_INCREMENT,
	order_token VARCHAR(250) NOT NULL,
	name VARCHAR(100) NOT NULL,
	type VARCHAR(20) NOT NULL,
	series VARCHAR(20) NOT NULL,
	number VARCHAR(20) NOT NULL,
    
    PRIMARY KEY (`id`),
    
  CONSTRAINT `fk_order_primavera_document_order` FOREIGN KEY (`order_token`) REFERENCES `order` (`token`) ON DELETE CASCADE ON UPDATE CASCADE
)

ALTER TABLE order_primavera_document 
ADD COLUMN  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
ADD COLUMN created_by VARCHAR(100) NOT NULL;
-- CTT

CREATE TABLE `ctt_postal_code_update` (
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`date`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `ctt_district` (
  `dd` CHAR(2) NOT NULL,      -- District Code
  `name` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`DD`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `ctt_municipality` (
  `cc` CHAR(2) NOT NULL,      -- Council Code
  `dd` CHAR(2) NOT NULL,      -- District Code
  `name` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`CC`, `DD`),
  CONSTRAINT `postal_code_council_ibfk_1` FOREIGN KEY (`DD`) REFERENCES `ctt_district` (`DD`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE ctt_postal_code (
  id 		  INT          NOT NULL AUTO_INCREMENT,
  dd          CHAR(2)      NOT NULL,  -- Código do Distrito (sempre preenchido)
  cc          CHAR(2)      NOT NULL,  -- Código do Concelho (sempre preenchido)
  llll        CHAR(10)      NOT NULL,  -- Código da localidade (sempre preenchido)
  localidade  VARCHAR(255) NOT NULL,  -- Nome da localidade (sempre preenchido)
  art_cod     VARCHAR(50)  DEFAULT NULL, -- Código da Artéria
  art_tipo    VARCHAR(70)  DEFAULT NULL, -- Artéria - Tipo (ex.: Rua, Praça, etc)
  pri_prep    VARCHAR(10)  DEFAULT NULL, -- Primeira preposição
  art_titulo  VARCHAR(50)  DEFAULT NULL, -- Artéria - Titulo (ex.: Doutor, Eng.º, etc)
  seg_prep    VARCHAR(10)  DEFAULT NULL, -- Segunda preposição
  art_desig   VARCHAR(100) DEFAULT NULL, -- Artéria - Designação
  art_local   VARCHAR(100) DEFAULT NULL, -- Artéria - Informação do Local/Zona
  troco       VARCHAR(100) DEFAULT NULL, -- Descrição do troço
  porta       VARCHAR(50)  DEFAULT NULL, -- Número da porta do cliente (pode estar vazio para CP geográficos)
  cliente     VARCHAR(100) DEFAULT NULL, -- Nome do cliente (pode estar vazio para CP geográficos)
  cp4         CHAR(4)      NOT NULL,  -- N.º do código postal (sempre preenchido)
  cp3         CHAR(3)      NOT NULL,  -- Extensão do n.º do código postal (sempre preenchido)
  cpalf       VARCHAR(100) NOT NULL,  -- Designação Postal (sempre preenchido)
  
   PRIMARY KEY (`id`),
  CONSTRAINT `postal_code_ibfk_1` FOREIGN KEY (`dd`) REFERENCES `ctt_district` (`dd`),
  CONSTRAINT `postal_code_ibfk_2` FOREIGN KEY (`cc`, `dd`) REFERENCES `ctt_municipality` (`cc`, `dd`)

) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- add google maps info to order
ALTER TABLE `order`
  ADD COLUMN `maps_address` VARCHAR(150) DEFAULT NULL,
  ADD COLUMN `distance` INT DEFAULT NULL,
  ADD COLUMN `travel_time` INT DEFAULT NULL;

ALTER TABLE `masterferro_engimatrix`.`order`
ADD COLUMN observations TEXT NULL,
ADD COLUMN contact VARCHAR(255) NULL;

ALTER TABLE `masterferro_engimatrix`.`order_product`
MODIFY COLUMN `calculated_price` DECIMAL(12,4) NOT NULL,
MODIFY COLUMN `price_discount` DECIMAL(12,4) NOT NULL;

ALTER TABLE `masterferro_engimatrix`.`order_primavera_document`
ADD COLUMN invoice_html LONGTEXT NULL;

AlTER TABLE `masterferro_engimatrix`.`order`
ADD COLUMN `district` varchar(255) NULL,
ADD COLUMN `locality` varchar(255) NULL;

ALTER TABLE `ctt_postal_code`
  ADD KEY `idx_cp4_cp3` (`cp4`, `cp3`);

ALTER TABLE `order`
  ADD COLUMN created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
  ADD COLUMN created_by VARCHAR(255),
  ADD COLUMN updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  ADD COLUMN updated_by VARCHAR(255);


ALTER TABLE `order`
  ADD COLUMN adjudicated_at DATETIME NULL;
  
  
ALTER TABLE `order`
  ADD COLUMN `status_id` INT NOT NULL,
  ADD CONSTRAINT `order_status_id` FOREIGN KEY (`status_id`) REFERENCES `status` (`id`);

ALTER TABLE `order` 
  ADD COLUMN `resolved_by` varchar(250) DEFAULT NULL,
  ADD COLUMN `resolved_at` datetime DEFAULT NULL;

INSERT INTO status VALUES (22, 'Pendente Aprovação Crédito');
INSERT INTO status VALUES (23, 'Crédito Rejeitado');

ALTER TABLE masterferro_engimatrix.order_product
ADD COLUMN price_locked_at DATETIME NULL DEFAULT NULL
AFTER calculated_price;  -- or whatever column you want it to follow

CREATE TABLE `platform_settings` (
  `id` BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `setting_key` VARCHAR(100) NOT NULL COMMENT 'Unique identifier for the setting',
  `setting_value` VARCHAR(50) NULL COMMENT 'Value stored as text',
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` DATETIME NOT NULL 
     ON UPDATE CURRENT_TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  `updated_by` VARCHAR(255) NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_app_settings_key` (`setting_key`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_unicode_ci
  COMMENT='Global application settings as key/value pairs';

INSERT INTO `app_setting` (`setting_key`, `setting_value`)
VALUES ('quotation-expiration-time', '5');

-- TRANSFER THE PRODUCTS FROM MF TO T

ALTER TABLE `mf_product_catalog`
  -- add the full description field
  ADD COLUMN `description_full` varchar(100) DEFAULT NULL AFTER `height`,
  -- add the pricing strategy reference (defaulting to 1)
  ADD COLUMN `pricing_strategy_id` int NOT NULL DEFAULT '1' AFTER `nominal_dimension`,
  -- index for the new FK
  ADD KEY `fk_product_catalog_pricing_strategy` (`pricing_strategy_id`),
  -- foreign-key constraint to mf_pricing_strategy(id)
  ADD CONSTRAINT `fk_mf_product_catalog_pricing_strategy`
    FOREIGN KEY (`pricing_strategy_id`)
    REFERENCES `mf_pricing_strategy` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE;

ALTER TABLE `mf_product_catalog`
  MODIFY COLUMN `price_pvp`  DECIMAL(20,10) NOT NULL   DEFAULT '0.0000000000',
  MODIFY COLUMN `price_avg`  DECIMAL(20,10)           DEFAULT NULL,
  MODIFY COLUMN `price_last` DECIMAL(20,10)           DEFAULT NULL;

ALTER TABLE `t_product_catalog`
ADD COLUMN `is_pricing_ready` TINYINT(1) NOT NULL DEFAULT 0;

UPDATE `t_product_catalog`
SET `is_pricing_ready` = 1 WHERE id>0;

-- BACKUP!

INSERT INTO `t_product_catalog` ( `product_code`, `description`, `unit`, `stock_current`, `currency`, `price_pvp`, `price_avg`, `price_last`, `date_last_entry`, `date_last_exit`,
  `family_id`, `price_ref_market`, `type_id`, `material_id`, `shape_id`, `finishing_id`, `surface_id`, `length`, `width`, `height`, `description_full`, `thickness`, `diameter`,
  `nominal_dimension`, `pricing_strategy_id`, `created_at`, `created_by`, `updated_at`, `updated_by`, `is_pricing_ready`
)
SELECT
  mpc.`product_code`, mpc.`description`, mpc.`unit`, mpc.`stock_current`, mpc.`currency`, mpc.`price_pvp`, mpc.`price_avg`, mpc.`price_last`, mpc.`date_last_entry`,
  mpc.`date_last_exit`, mpc.`family_id`, mpc.`price_ref_market`, mpc.`type_id`, mpc.`material_id`, mpc.`shape_id`, mpc.`finishing_id`, mpc.`surface_id`, mpc.`length`,
  mpc.`width`, mpc.`height`, mpc.`description_full`, mpc.`thickness`, mpc.`diameter`, mpc.`nominal_dimension`, mpc.`pricing_strategy_id`, mpc.`created_at`, mpc.`created_by`,
  mpc.`updated_at`, mpc.`updated_by`, 0
FROM `mf_product_catalog` AS mpc
WHERE NOT EXISTS (
  SELECT 1
  FROM `t_product_catalog` AS tpc
  WHERE tpc.`product_code` = mpc.`product_code`
);

UPDATE `t_product_catalog`
SET `description_full` = `description`
WHERE `description_full` IS NULL AND id > 0;

--
-- Dumping events for database 'masterferro_engimatrix'
--

--
-- Dumping routines for database 'masterferro_engimatrix'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


UPDATE `masterferro_engimatrix`.`config` SET `value` = '180' WHERE (`config` = 'password_expiration_policy_days');

--
-- Dumping routines for database 'masterferro_engimatrix'
--
/*!50003 DROP FUNCTION IF EXISTS `GetLabelDescription` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `GetLabelDescription`(in_language CHAR(3), in_lblKey VARCHAR(500)) RETURNS varchar(255) CHARSET utf8mb4
    DETERMINISTIC
BEGIN
    DECLARE description VARCHAR(255);
        IF in_language = 'en' THEN
        SELECT descriptionEn  INTO description FROM translations WHERE lblKey LIKE in_lblKey LIMIT 1;
    ELSEIF in_language = 'pt' THEN
        SELECT descriptionPt as 'description' INTO description FROM translations WHERE lblKey LIKE in_lblKey LIMIT 1;
    ELSE
        SELECT descriptionEn as 'description' INTO description FROM translations WHERE lblKey LIKE in_lblKey LIMIT 1; -- ou escolha a língua padrão desejada
    END IF;

    RETURN description;
END ;;
DELIMITER ;
