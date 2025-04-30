
--
-- Table structure for table `alert`
--

DROP TABLE IF EXISTS `alert`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `alert` (
  `idalert` int NOT NULL AUTO_INCREMENT,
  `message` varchar(250) NOT NULL,
  `type` varchar(45) NOT NULL,
  `node` varchar(45) NOT NULL,
  `timestamp` datetime NOT NULL,
  PRIMARY KEY (`idalert`)
) ENGINE=InnoDB AUTO_INCREMENT=762 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Dumping data for table `script`
--

DROP TABLE IF EXISTS `script`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `script` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `description` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `cron_job` varchar(45) NOT NULL DEFAULT '* * 8 * * ?',
  `status` int NOT NULL DEFAULT '17',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`),
  KEY `status` (`status`),
  CONSTRAINT `script_ibfk_1` FOREIGN KEY (`status`) REFERENCES `status` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

LOCK TABLES `script` WRITE;
/*!40000 ALTER TABLE `script` DISABLE KEYS */;
INSERT INTO `script` VALUES (34,'CTT','Atualização Dados Clientes','* * 8 * * ?',1);
/*!40000 ALTER TABLE `script` ENABLE KEYS */;
UNLOCK TABLES;


-- Table structure for table `job`
--

DROP TABLE IF EXISTS `job`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `transactions`
--

DROP TABLE IF EXISTS `transactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transactions` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `status_id` int NOT NULL DEFAULT '9',
  `reference` varchar(100) DEFAULT NULL,
  `started` datetime DEFAULT NULL,
  `ended` datetime DEFAULT NULL,
  `exception` varchar(150) DEFAULT NULL,
  `queue_id` int NOT NULL,
  `input_data` longtext,
  `output_data` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_transactions_queue_id` (`queue_id`),
  KEY `fk_transactions_status_id` (`status_id`),
  CONSTRAINT `fk_transactions_queue_id` FOREIGN KEY (`queue_id`) REFERENCES `queues_script` (`ID`),
  CONSTRAINT `fk_transactions_status_id` FOREIGN KEY (`status_id`) REFERENCES `status` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `queues_script`
--

DROP TABLE IF EXISTS `queues_script`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `queues_script` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `name` varchar(250) DEFAULT NULL,
  `description` varchar(500) DEFAULT NULL,
  `autoRetry` tinyint(1) DEFAULT NULL,
  `numberRetry` int DEFAULT NULL,
  `status_id` int NOT NULL DEFAULT '1',
  `id_script` int NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `name` (`name`),
  KEY `fk_queues_script_status_id` (`status_id`),
  KEY `fk_queues_script_id_script` (`id_script`),
  CONSTRAINT `fk_queues_script_id_script` FOREIGN KEY (`id_script`) REFERENCES `script` (`id`),
  CONSTRAINT `fk_queues_script_status_id` FOREIGN KEY (`status_id`) REFERENCES `status` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;



--
-- Table structure for table `assets_script`
--

DROP TABLE IF EXISTS `assets_script`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `assets_script` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `description` varchar(500) DEFAULT NULL,
  `type` varchar(45) DEFAULT NULL,
  `user` varchar(45) DEFAULT NULL,
  `password` varchar(500) DEFAULT NULL,
  `text` longtext,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `description` (`description`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `triggers`
--

DROP TABLE IF EXISTS `triggers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `triggers` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `cron_expression` varchar(255) NOT NULL,
  `script_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `script_id` (`script_id`),
  CONSTRAINT `triggers_ibfk_1` FOREIGN KEY (`script_id`) REFERENCES `script` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `script_version`
--

DROP TABLE IF EXISTS `script_version`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `script_version` (
  `id_script` int NOT NULL,
  `url_script` varchar(150) NOT NULL,
  `main_file` varchar(150) NOT NULL,
  `date_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `version` int NOT NULL DEFAULT '1',
  `is_current_version` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id_script`,`url_script`,`date_time`,`version`),
  UNIQUE KEY `url_script` (`url_script`),
  CONSTRAINT `script_version_ibfk_1` FOREIGN KEY (`id_script`) REFERENCES `script` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;