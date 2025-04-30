SELECT 
    *
FROM
    masterferro_engimatrix.mf_product_catalog
WHERE
    description LIKE '%%';

DROP TABLE IF EXISTS `t_product_catalog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `t_product_catalog` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `product_code` VARCHAR(50),
    `description` VARCHAR(255) NOT NULL,
    `unit` VARCHAR(10),
    `stock_current` DECIMAL(10 , 2 ),
    `currency` VARCHAR(3) NOT NULL,
    `price_pvp` DECIMAL(10 , 2 ) DEFAULT 0.0,
    `price_avg` DECIMAL(10 , 2 ),
    `price_last` DECIMAL(10 , 2 ),
    `date_last_entry` DATE,
    `date_last_exit` DATE,
    `family_id` VARCHAR(10),
    `price_ref_market` DECIMAL(10 , 2 ),
    `type_id` INT,
    `material_id` INT,
    `shape_id` INT,
    `finishing_id` INT,
    `surface_id` INT,
    `length` DECIMAL(10 , 2 ),
    `width` DECIMAL(10 , 2 ),
    `height` DECIMAL(10 , 2 ),
    FOREIGN KEY (`family_id`)
        REFERENCES `mf_product_family` (`id`),
    FOREIGN KEY (`type_id`)
        REFERENCES `mf_product_type` (`id`),
    FOREIGN KEY (`material_id`)
        REFERENCES `mf_product_material` (`id`),
    FOREIGN KEY (`shape_id`)
        REFERENCES `mf_product_shape` (`id`),
    FOREIGN KEY (`finishing_id`)
        REFERENCES `mf_product_finishing` (`id`),
    FOREIGN KEY (`surface_id`)
        REFERENCES `mf_product_surface` (`id`),
    INDEX `idx_product_code` (`product_code`)
)  ENGINE=INNODB DEFAULT CHARSET=UTF8MB4 COLLATE = UTF8MB4_0900_AI_CI;
/*!40101 SET character_set_client = @saved_cs_client */;


ALTER TABLE t_product_catalog
ADD COLUMN description_full VARCHAR(100) DEFAULT NULL;



-- change description_full name to description_full
ALTER TABLE t_product_catalog
RENAME COLUMN description_lucas TO description_full;


ALTER TABLE mf_product_catalog
    ADD COLUMN `thickness` DECIMAL(10,2) DEFAULT NULL,      
    ADD COLUMN `diameter` DECIMAL(10,2)DEFAULT NULL;


DELETE FROM t_product_catalog 
WHERE
    description LIKE '%CANT%' AND id > 0;



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(204, '12001503', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 15x15x03', 'KG', 557.00, 'EUR', 1.87, 1.32, 1.33, '2024-03-28', '2024-06-07', 12, NULL, 2, 1, 16, NULL, 13, 6000, 15, 15, 3, NULL),
(205, '12002003', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 20x20x03', 'KG', 5401.00, 'EUR', 1.10, 0.73, 0.73, '2024-02-12', '2024-06-14', 12, NULL, 2, 1, 16, NULL, 13, 6000, 20, 20, 3, NULL),
(206, '12002503', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 25x25x03', 'KG', 4263.00, 'EUR', 1.10, 0.70, 0.70, '2024-05-29', '2024-06-14', 12, NULL, 2, 1, 16, NULL, 13, 6000, 25, 25, 3, NULL),
(207, '12002504', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 25x25x04', 'KG', 1074.00, 'EUR', 1.10, 0.77, 0.77, '2023-05-26', '2024-06-05', 12, NULL, 2, 1, 16, NULL, 13, 6000, 25, 25, 4, NULL),
(208, '12002505', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 25x25x05', 'KG', 765.00, 'EUR', 1.05, 0.70, 0.70, '2024-02-29', '2024-06-13', 12, NULL, 2, 1, 16, NULL, 13, 6000, 25, 25, 5, NULL),
(209, '12003003', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 30x30x03', 'KG', 2341.00, 'EUR', 1.05, 0.68, 0.68, '2024-05-08', '2024-06-14', 12, NULL, 2, 1, 16, NULL, 13, 6000, 30, 30, 3, NULL),
(210, '12003004', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 30x30x04', 'KG', 1604.00, 'EUR', 1.05, 0.69, 0.69, '2024-02-13', '2024-06-14', 12, NULL, 2, 1, 16, NULL, 13, 6000, 30, 30, 4, NULL),
(211, '12003005', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 30x30x05', 'KG', 1398.00, 'EUR', 1.05, 0.68, 0.68, '2024-05-10', '2024-06-14', 12, NULL, 2, 1, 16, NULL, 13, 6000, 30, 30, 5, NULL),
(212, '12003504', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 35x35x04', 'KG', 3434.00, 'EUR', 1.05, 0.67, 0.67, '2024-06-06', '2024-06-13', 12, NULL, 2, 1, 16, NULL, 13, 6000, 35, 35, 4, NULL),
(213, '12003505', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 35x35x05', 'KG', 5452.00, 'EUR', 1.05, 0.67, 0.67, '2024-06-06', '2024-06-14', 12, NULL, 2, 1, 16, NULL, 13, 6000, 35, 35, 5, NULL),
(214, '12003535', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 35x35x3,5', 'KG', 4612.00, 'EUR', 1.05, 0.68, 0.68, '2024-05-29', '2024-06-12', 12, NULL, 2, 1, 16, NULL, 13, 6000, 35, 35, 3.5, NULL),
(215, '12004004', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 40x40x04', 'KG', 1811.00, 'EUR', 1.01, 0.64, 0.65, '2024-05-08', '2024-06-14', 12, NULL, 2, 1, 16, NULL, 13, 6000, 40, 40, 4, NULL),
(216, '12004005', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 40x40x05', 'KG', 2519.00, 'EUR', 1.01, 0.64, 0.64, '2024-05-10', '2024-06-14', 12, NULL, 2, 1, 16, NULL, 13, 6000, 40, 40, 5, NULL),
(217, '12004505', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 45x45x05', 'KG', 2180.00, 'EUR', 1.01, 0.65, 0.64, '2024-04-30', '2024-06-14', 12, NULL, 2, 1, 16, NULL, 13, 6000, 45, 45, 5, NULL),
(218, '12005005', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 50x50x05', 'KG', 4062.00, 'EUR', 1.01, 0.64, 0.64, '2024-05-24', '2024-06-14', 12, NULL, 2, 1, 16, NULL, 13, 6000, 50, 50, 5, NULL),
(219, '12005006', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 50x50x06', 'KG', 1618.00, 'EUR', 1.01, 0.65, 0.65, '2024-03-21', '2024-06-06', 12, NULL, 2, 1, 16, NULL, 13, 6000, 50, 50, 6, NULL),
(220, '12005506', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 55x55x06', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 13, 6000, 55, 55, 6, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(221, '12006006', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 60x60x06', 'KG', 4356.00, 'EUR', 1.01, 0.64, 0.64, '2024-06-06', '2024-06-13', 12, NULL, 2, 1, 16, NULL, 13, 6000, 60, 60, 6, NULL),
(222, '12006507', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 65x65x07', 'KG', 0.00, 'EUR', 1.08, 0.99, 0.99, '2022-02-04', '2022-02-04', 12, NULL, 2, 1, 16, NULL, 13, 6000, 65, 65, 7, NULL),
(223, '12007007', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 70x70x07', 'KG', 4726.00, 'EUR', 1.08, 0.70, 0.69, '2024-05-24', '2024-06-05', 12, NULL, 2, 1, 16, NULL, 13, 6000, 70, 70, 7, NULL),
(224, '12007507', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 75x75x07', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 13, 6000, 75, 75, 7, NULL),
(225, '12007508', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 75x75x08', 'KG', 0.00, 'EUR', 1.08, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 13, 6000, 75, 75, 8, NULL),
(226, '12008008', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 80x80x08', 'KG', 4243.00, 'EUR', 1.08, 0.73, 0.71, '2024-02-15', '2024-06-06', 12, NULL, 2, 1, 16, NULL, 13, 6000, 80, 80, 8, NULL),
(227, '12009006', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 90x90x06', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 13, 6000, 90, 90, 6, NULL),
(228, '12009009', 'CANT.ABAS IGUAIS', 'CANT.ABAS IGUAIS 90x90x09', 'KG', 5788.00, 'EUR', 1.08, 0.75, 0.75, '2023-11-02', '2024-05-06', 12, NULL, 2, 1, 16, NULL, 13, 6000, 90, 90, 9, NULL),
(229, '12010010', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 100x100x10', 'KG', 2201.00, 'EUR', 1.08, 0.72, 0.71, '2024-02-15', '2024-06-06', 12, NULL, 2, 1, 16, NULL, 13, 6000, 100, 100, 10, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(230, '12010012', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 100x100x12', 'KG', 0.00, 'EUR', 1.08, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 13, 6000, 100, 100, 12, NULL),
(231, '12012010', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 120x120x10', 'KG', 7558.00, 'EUR', 1.08, 0.72, 0.71, '2024-05-08', '2024-06-13', 12, NULL, 2, 1, 16, NULL, 13, 6000, 120, 120, 10, NULL),
(232, '12012012', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 120x120x12', 'KG', 0.00, 'EUR', 1.08, 0.85, 0.85, '2022-04-22', '2023-07-21', 12, NULL, 2, 1, 16, NULL, 13, 6000, 120, 120, 12, NULL),
(233, '12012015', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 120x120x15', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 13, 6000, 120, 120, 15, NULL),
(234, '12015010', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 150x150x10', 'KG', 0.00, 'EUR', 1.23, 0.69, 0.69, '2020-06-01', '2020-06-02', 12, NULL, 2, 1, 16, NULL, 13, 6000, 150, 150, 10, NULL),
(235, '12015012', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 150x150x12', 'KG', 0.00, 'EUR', 1.23, 0.93, 0.93, '2023-07-27', '2023-07-28', 12, NULL, 2, 1, 16, NULL, 13, 6000, 150, 150, 12, NULL),
(236, '12015015', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 150x150x15', 'KG', 0.00, 'EUR', 1.23, 0.88, 0.88, '2023-11-17', '2024-01-31', 12, NULL, 2, 1, 16, NULL, 13, 6000, 150, 150, 15, NULL),
(237, '12015018', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 150x150x18', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 13, 6000, 150, 150, 18, NULL),
(238, '12018016', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 180x180x18', 'KG', 0.00, 'EUR', 1.24, 0.53, 0.53, '2018-01-19', '2018-01-22', 12, NULL, 2, 1, 16, NULL, 13, 6000, 180, 180, 18, NULL),
(239, '12020016', 'CANT.AB.IGUAIS', 'CANT.AB.IGUAIS 200x200x16', 'KG', 0.00, 'EUR', 1.24, 0.75, 0.75, '2021-07-09', '2021-07-13', 12, NULL, 2, 1, 16, NULL, 13, 6000, 200, 200, 16, NULL);

INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(240, '12020018', 'CANT. AB.IGUAIS', 'CANT. AB.IGUAIS 200x18', 'KG', 0.00, 'EUR', 1.34, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 13, 6000, 200, 200, 18, NULL),
(241, '12020020', 'CANT. AB.IGUAIS', 'CANT. AB.IGUAIS 200x20', 'KG', 0.00, 'EUR', 1.24, 1.05, 1.05, '2021-07-09', '2021-07-13', 12, NULL, 2, 1, 16, NULL, 13, 6000, 200, 200, 20, NULL),
(242, '12103020', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 30x20x3', 'KG', 0.00, 'EUR', 2.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 14, 6000, 30, 20, 3, NULL),
(243, '12104020', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 40x20x4', 'KG', 967.00, 'EUR', 2.17, 1.30, 1.30, '2024-05-28', '2024-06-03', 12, NULL, 2, 1, 16, NULL, 14, 6000, 40, 20, 4, NULL),
(244, '12104025', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 40x25x4', 'KG', 0.00, 'EUR', 1.96, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 14, 6000, 40, 25, 4, NULL),
(245, '12105030', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 50x30x5', 'KG', 259.00, 'EUR', 1.96, 1.03, 1.03, '2024-04-22', '2024-06-12', 12, NULL, 2, 1, 16, NULL, 14, 6000, 50, 30, 5, NULL),
(246, '12106030', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 60x30x5', 'KG', 1751.00, 'EUR', 1.96, 1.03, 1.03, '2024-02-14', '2024-05-09', 12, NULL, 2, 1, 16, NULL, 14, 6000, 60, 30, 5, NULL),
(247, '12106040', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 60x40x6', 'KG', 349.00, 'EUR', 1.96, 1.04, 1.04, '2023-12-29', '2024-06-07', 12, NULL, 2, 1, 16, NULL, 14, 6000, 60, 40, 6, NULL),
(248, '12106045', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 60x40x5', 'KG', 0.00, 'EUR', 1.96, 1.33, 1.33, '2022-12-16', NULL, 12, NULL, 2, 1, 16, NULL, 14, 6000, 60, 40, 5, NULL),
(249, '12107550', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 75x50x6', 'KG', 0.00, 'EUR', 1.96, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 14, 6000, 75, 50, 6, NULL),
(250, '12107551', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 80x60x7', 'KG', 0.00, 'EUR', 1.96, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 14, 6000, 80, 60, 7, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(251, '12108040', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 80x40x6', 'KG', 2511.00, 'EUR', 1.96, 1.10, 1.10, '2024-05-28', '2024-06-05', 12, NULL, 2, 1, 16, NULL, 14, 6000, 80, 40, 6, NULL),
(252, '12108060', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 100x50x10', 'KG', 0.00, 'EUR', 1.96, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 14, 6000, 100, 50, 10, NULL),
(253, '12110050', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 100x50x8', 'KG', 0.00, 'EUR', 1.96, 1.25, 1.25, '2023-09-25', '2024-06-06', 12, NULL, 2, 1, 16, NULL, 14, 6000, 100, 50, 8, NULL),
(254, '12110056', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 100x50x6', 'KG', 929.00, 'EUR', 1.96, 0.98, 0.98, '2024-03-13', '2024-03-18', 12, NULL, 2, 1, 16, NULL, 14, 6000, 100, 50, 6, NULL),
(255, '12112080', 'CANT.AB.DESIG.', 'CANT.AB.DESIG.120x80x10', 'KG', 0.00, 'EUR', 1.96, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 14, 6000, 120, 80, 10, NULL),
(256, '12113065', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 130x65x8', 'KG', 0.00, 'EUR', 1.96, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 2, 1, 16, NULL, 14, 6000, 130, 65, 8, NULL),
(257, '12115010', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 150x100x10', 'KG', 0.00, 'EUR', 1.96, 1.10, 1.10, '2023-08-30', '2023-08-31', 12, NULL, 2, 1, 16, NULL, 14, 6000, 150, 100, 10, NULL),
(258, '12120010', 'CANT.AB.DESIG.', 'CANT.AB.DESIG. 200x100x10', 'KG', 0.00, 'EUR', 1.96, 0.89, 0.89, '2019-06-21', '2019-06-19', 12, NULL, 2, 1, 16, NULL, 14, 6000, 200, 100, 10, NULL);

INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(274, '14000020', 'BARRA T', 'BARRA T 20x3', 'KG', 4479.00, 'EUR', 1.40, 0.64, 0.50, '2022-12-12', '2024-04-11', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 20, 3, NULL, NULL),
(275, '14000025', 'BARRA T', 'BARRA T 25x3,5', 'KG', 3490.00, 'EUR', 1.33, 0.79, 0.79, '2021-10-04', '2024-05-24', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 25, 3.5, NULL, NULL),
(276, '14000030', 'BARRA T', 'BARRA T 30x4', 'KG', 6234.00, 'EUR', 1.32, 0.77, 0.76, '2024-02-08', '2024-06-12', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 30, 4, NULL, NULL),
(277, '14000035', 'BARRA T', 'BARRA T 35x4,5', 'KG', 1705.00, 'EUR', 1.32, 0.83, 0.83, '2024-05-02', '2024-06-14', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 35, 4.5, NULL, NULL),
(278, '14000040', 'BARRA T', 'BARRA T 40x5', 'KG', 4182.00, 'EUR', 1.32, 0.78, 0.76, '2024-02-08', '2024-06-14', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 40, 5, NULL, NULL),
(279, '14000045', 'BARRA T', 'BARRA T 45x5,5', 'KG', 2885.00, 'EUR', 1.38, 0.81, 0.82, '2022-04-14', '2024-04-22', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 45, 5.5, NULL, NULL),
(280, '14000050', 'BARRA T', 'BARRA T 50x6', 'KG', 6790.00, 'EUR', 1.38, 0.80, 0.79, '2024-05-21', '2024-06-14', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 50, 6, NULL, NULL),
(281, '14000060', 'BARRA T', 'BARRA T 60x7', 'KG', 1362.00, 'EUR', 1.38, 0.86, 0.87, '2024-05-02', '2024-06-07', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 60, 7, NULL, NULL),
(282, '14000070', 'BARRA T', 'BARRA T 70x8', 'KG', 4670.00, 'EUR', 1.38, 0.85, 0.82, '2024-05-24', '2024-06-06', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 70, 8, NULL, NULL),
(283, '14000080', 'BARRA T', 'BARRA T 80x9', 'KG', 4116.00, 'EUR', 1.38, 0.81, 0.81, '2024-05-03', '2024-05-10', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 80, 9, NULL, NULL),
(284, '14000100', 'BARRA T', 'BARRA T 100x11', 'KG', 0.00, 'EUR', 1.56, 1.29, 1.29, '2022-10-14', '2023-04-18', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 100, 11, NULL, NULL),
(285, '14000120', 'BARRA T', 'BARRA T 120x13', 'KG', 0.00, 'EUR', 1.71, 0.89, 0.89, '2019-07-22', '2019-07-26', 12, NULL, 2, 1, 5, NULL, NULL, 6000, 120, 13, NULL, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(286, '15003015', 'BARRA UPN', 'BARRA UPN 30x15x4', 'KG', 405.00, 'EUR', 1.52, 0.85, 0.85, '2023-12-11', '2024-05-29', 12, NULL, 2, 1, 3, NULL, NULL, 6000, 30, 15, 4, NULL),
(287, '15004020', 'BARRA UPN', 'BARRA UPN 40x20x5', 'KG', 7257.00, 'EUR', 1.20, 0.74, 0.73, '2024-05-21', '2024-05-29', 12, NULL, 2, 1, 3, NULL, NULL, 6000, 40, 20, 5, NULL),
(288, '15004035', 'BARRA UPN', 'BARRA UPN 40x35x5', 'KG', 957.00, 'EUR', 1.52, 1.00, 0.90, '2024-01-25', '2024-06-06', 12, NULL, 2, 1, 3, NULL, NULL, 6000, 40, 35, 5, NULL),
(289, '15005025', 'BARRA UPN', 'BARRA UPN 50x25x6', 'KG', 6704.00, 'EUR', 1.20, 0.74, 0.72, '2024-05-21', '2024-06-12', 12, NULL, 2, 1, 3, NULL, NULL, 6000, 50, 25, 6, NULL),
(290, '15005038', 'BARRA UPN', 'BARRA UPN 50x38x5', 'KG', 778.00, 'EUR', 1.52, 0.80, 0.80, '2024-03-13', '2024-05-24', 12, NULL, 2, 1, 3, NULL, NULL, 6000, 50, 38, 5, NULL),
(291, '15006030', 'BARRA UPN', 'BARRA UPN 60x30x6', 'KG', 5793.00, 'EUR', 1.20, 0.72, 0.70, '2024-05-21', '2024-06-14', 12, NULL, 2, 1, 3, NULL, NULL, 6000, 60, 30, 6, NULL),
(292, '15006542', 'BARRA UPN', 'BARRA UPN 65x42x5,5', 'KG', 10741.00, 'EUR', 1.20, 0.69, 0.68, '2024-05-21', '2024-06-11', 12, NULL, 2, 1, 3, NULL, NULL, 6000, 65, 42, 5.5, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(293, '15008080', 'BARRA UPN', 'BARRA UPN 80 C/6.1MT', 'KG', 6656.00, 'EUR', 1.03, 0.65, 0.64, '2024-05-24', '2024-06-14', 13, NULL, 2, 1, 3, NULL, NULL, 6100, 80, NULL, NULL, NULL),
(294, '15008081', 'BARRA UPN', 'BARRA UPN 80 C/12.1MT', 'KG', 15.00, 'EUR', 1.03, 0.66, 0.66, '2024-03-11', '2024-06-03', 13, NULL, 2, 1, 3, NULL, NULL, 12100, 80, NULL, NULL, NULL),
(295, '15100100', 'BARRA UPN', 'BARRA UPN 100 C/6.1MT', 'KG', 2527.00, 'EUR', 1.03, 0.64, 0.63, '2024-04-08', '2024-06-12', 13, NULL, 2, 1, 3, NULL, NULL, 6100, 100, NULL, NULL, NULL),
(296, '15100101', 'BARRA UPN', 'BARRA UPN 100 C/12.1MT', 'KG', 3594.00, 'EUR', 1.03, 0.65, 0.65, '2024-05-08', '2024-06-07', 13, NULL, 2, 1, 3, NULL, NULL, 12100, 100, NULL, NULL, NULL),
(297, '15120120', 'BARRA UPN', 'BARRA UPN 120 C/6.1MT', 'KG', 3804.00, 'EUR', 1.03, 0.65, 0.65, '2024-05-31', '2024-06-14', 13, NULL, 2, 1, 3, NULL, NULL, 6100, 120, NULL, NULL, NULL),
(298, '15120121', 'BARRA UPN', 'BARRA UPN 120 C/12.1MT', 'KG', 4216.00, 'EUR', 1.03, 0.64, 0.64, '2024-06-03', '2024-06-05', 13, NULL, 2, 1, 3, NULL, NULL, 12100, 120, NULL, NULL, NULL),
(299, '15140140', 'BARRA UPN', 'BARRA UPN 140 C/6.1MT', 'KG', 5719.00, 'EUR', 1.15, 0.72, 0.72, '2024-06-03', '2024-06-13', 13, NULL, 2, 1, 3, NULL, NULL, 6100, 140, NULL, NULL, NULL),
(300, '15140141', 'BARRA UPN', 'BARRA UPN 140 C/12.1MT', 'KG', 1931.00, 'EUR', 1.15, 0.71, 0.71, '2024-04-02', '2024-05-02', 13, NULL, 2, 1, 3, NULL, NULL, 12100, 140, NULL, NULL, NULL),
(301, '15160160', 'BARRA UPN', 'BARRA UPN 160 C/6.1MT', 'KG', 4922.00, 'EUR', 1.15, 0.72, 0.72, '2024-05-21', '2024-06-12', 13, NULL, 2, 1, 3, NULL, NULL, 6100, 160, NULL, NULL, NULL);

INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(302, '15160161', 'BARRA UPN', 'BARRA UPN 160 C/12.1MT', 'KG', 10223.00, 'EUR', 1.15, 0.74, 0.75, '2024-05-14', '2024-06-11', 13, NULL, 2, 1, 3, NULL, NULL, 12100, 160, NULL, NULL, NULL),
(303, '15160168', 'BARRA UPN', 'BARRA UPN 160 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 2, 1, 3, NULL, NULL, 8100, 160, NULL, NULL, NULL),
(304, '15160169', 'BARRA UPN', 'BARRA UPN 160 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 0.53, 0.53, '2017-10-10', '2018-10-19', 13, NULL, 2, 1, 3, NULL, NULL, 9100, 160, NULL, NULL, NULL),
(305, '15160175', 'BARRA UPN', 'BARRA UPN 160 C/15.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 2, 1, 3, NULL, NULL, 15100, 160, NULL, NULL, NULL),
(306, '15180180', 'BARRA UPN', 'BARRA UPN 180 C/6.1MT', 'KG', 405.00, 'EUR', 1.15, 0.73, 0.72, '2019-06-06', '2024-06-11', 13, NULL, 2, 1, 3, NULL, NULL, 6100, 180, NULL, NULL, NULL),
(307, '15180181', 'BARRA UPN', 'BARRA UPN 180 C/12.1MT', 'KG', 6385.00, 'EUR', 1.15, 0.73, 0.71, '2024-05-24', '2024-06-07', 13, NULL, 2, 1, 3, NULL, NULL, 12100, 180, NULL, NULL, NULL),
(308, '15180185', 'BARRA UPN', 'BARRA UPN 180 C/15.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 2, 1, 3, NULL, NULL, 15100, 180, NULL, NULL, NULL),
(309, '15200200', 'BARRA UPN', 'BARRA UPN 200 C/6.1MT', 'KG', 126.00, 'EUR', 1.15, 0.75, 0.76, '2024-04-23', '2024-06-13', 13, NULL, 2, 1, 3, NULL, NULL, 6100, 200, NULL, NULL, NULL),
(310, '15200201', 'BARRA UPN', 'BARRA UPN 200 C/12.1MT', 'KG', 4111.00, 'EUR', 1.15, 0.75, 0.75, '2024-06-07', '2024-06-05', 13, NULL, 2, 1, 3, NULL, NULL, 12100, 200, NULL, NULL, NULL),
(311, '15200208', 'BARRA UPN', 'BARRA UPN 200 C/10.11MT', 'KG', 0.00, 'EUR', 1.15, 0.64, 0.64, '2017-10-10', '2018-05-17', 13, NULL, 2, 1, 3, NULL, NULL, 10110, 200, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(312, '15200215', 'BARRA UPN', 'BARRA UPN 200 C/15.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 2, 1, 3, NULL, NULL, 15100, 200, NULL, NULL, NULL),
(313, '15200220', 'BARRA UPN', 'BARRA UPN 220 C/6.1MT', 'KG', 0.00, 'EUR', 1.15, 0.76, 0.74, '2022-07-26', '2024-06-07', 13, NULL, 2, 1, 3, NULL, NULL, 6100, 220, NULL, NULL, NULL),
(314, '15200221', 'BARRA UPN', 'BARRA UPN 220 C/12.1MT', 'KG', 355.00, 'EUR', 1.15, 0.76, 0.76, '2024-05-07', '2024-05-13', 13, NULL, 2, 1, 3, NULL, NULL, 12100, 220, NULL, NULL, NULL),
(315, '15200240', 'BARRA UPN', 'BARRA UPN 240 C/6.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 1.00, '2022-01-24', '2024-06-06', 13, NULL, 2, 1, 3, NULL, NULL, 6100, 240, NULL, NULL, NULL),
(316, '15200241', 'BARRA UPN', 'BARRA UPN 240 C/12.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.77, '2024-04-10', '2024-04-18', 13, NULL, 2, 1, 3, NULL, NULL, 12100, 240, NULL, NULL, NULL),
(317, '15200244', 'BARRA UPN', 'BARRA UPN 260 C/14.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 2, 1, 3, NULL, NULL, 14100, 260, NULL, NULL, NULL),
(318, '15200245', 'BARRA UPN', 'BARRA UPN 240 C/15.1MT', 'KG', 0.00, 'EUR', 1.17, 0.74, 0.74, '2021-04-19', '2021-04-26', 13, NULL, 2, 1, 3, NULL, NULL, 15100, 240, NULL, NULL, NULL),
(319, '15200250', 'BARRA UPN', 'BARRA UPN 240 C/10.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 2, 1, 3, NULL, NULL, 10100, 240, NULL, NULL, NULL),
(320, '15200260', 'BARRA UPN', 'BARRA UPN 260 C/6.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.77, '2023-09-12', '2023-09-18', 13, NULL, 2, 1, 3, NULL, NULL, 6100, 260, NULL, NULL, NULL),
(321, '15200261', 'BARRA UPN', 'BARRA UPN 260 C/12.1MT', 'KG', 0.00, 'EUR', 1.17, 0.79, 0.79, '2024-01-08', '2024-01-09', 13, NULL, 2, 1, 3, NULL, NULL, 12100, 260, NULL, NULL, NULL),
(322, '15200265', 'BARRA UPN', 'BARRA UPN 260 C/15.1MT', 'KG', 0.00, 'EUR', 1.17, 1.01, 1.01, '2022-04-22', NULL, 13, NULL, 2, 1, 3, NULL, NULL, 15100, 260, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('10023508', 'VARAO LISO', 'VARAO LISO A 235 NL 8', 'KG', 2628.00, 'EUR', 1.07, 0.69, 0.69, '2024-06-06', '2024-06-12', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 8),
('10023510', 'VARAO LISO', 'VARAO LISO A 235 NL 10', 'KG', 282.00, 'EUR', 1.04, 0.68, 0.68, '2024-05-10', '2024-06-14', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 10),
('10023512', 'VARAO LISO', 'VARAO LISO A 235 NL 12', 'KG', 3512.00, 'EUR', 1.02, 0.67, 0.67, '2024-05-10', '2024-06-14', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 12),
('10023514', 'VARAO LISO', 'VARAO LISO A 235 NL 14', 'KG', 0.00, 'EUR', 1.02, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 14),
('10023516', 'VARAO LISO', 'VARAO LISO A 235 NL 16', 'KG', 354.00, 'EUR', 1.01, 0.67, 0.66, '2024-04-30', '2024-06-12', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 16),
('10023520', 'VARAO LISO', 'VARAO LISO A 235 NL 20', 'KG', 2112.00, 'EUR', 1.01, 0.66, 0.66, '2024-04-30', '2024-06-14', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 20),
('10023525', 'VARAO LISO', 'VARAO LISO A 235 NL 25', 'KG', 1121.00, 'EUR', 1.01, 0.68, 0.68, '2024-02-23', '2024-06-11', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 25),
('10023530', 'VARAO LISO', 'VARAO LISO A 235 NL 30', 'KG', 1428.00, 'EUR', 1.05, 0.67, 0.67, '2023-11-30', '2024-06-13', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 30),
('10023532', 'VARAO LISO', 'VARAO LISO A 235 NL 32', 'KG', 1992.00, 'EUR', 1.05, 0.69, 0.69, '2024-01-26', '2024-05-13', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 32),
('10023540', 'VARAO LISO', 'VARAO LISO A 235 NL 40', 'KG', 2028.00, 'EUR', 1.01, 0.66, 0.66, '2024-04-30', '2024-05-31', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 40),
('10023545', 'VARAO LISO', 'VARAO LISO A 235 NL 45', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 45);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('10023550', 'VARAO LISO', 'VARAO LISO A 235 NL 50', 'KG', 0.00, 'EUR', 1.33, 0.73, 0.73, '2023-08-01', '2023-08-03', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 50),
('10023555', 'VARAO LISO', 'VARAO LISO A 235 NL 55', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 55),
('10023560', 'VARAO LISO', 'VARAO LISO A 235 NL 60', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 60),
('10023565', 'VARAO LISO', 'VARAO LISO A 235 NL 65', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 65),
('10023570', 'VARAO LISO', 'VARÃO LISO A 235 NL 70', 'KG', 0.00, 'EUR', 1.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 70),
('10023575', 'VARAO LISO', 'VARAO LISO A 235 NL 75', 'KG', 0.00, 'EUR', 1.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 75),
('10023580', 'VARAO LISO', 'VARÃO LISO A 235 NL 80', 'KG', 0.00, 'EUR', 1.00, 1.00, 1.00, '2022-01-27', '2022-01-28', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 80),
('10023590', 'VARAO LISO', 'VARAO LISO A 235 NL 100', 'KG', 0.00, 'EUR', 1.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, 6000, NULL, NULL, 235, 100);

INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('10040006', 'VARAO NERV', 'VARAO NERV A 400 NR 6', 'KG', 35073.00, 'EUR', 1.34, 0.70, 0.73, '2024-03-07', '2024-06-13', 1, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 400, 6),
('10040008', 'VARAO NERV', 'VARAO NERV A 400 NR 8', 'KG', 40686.00, 'EUR', 1.07, 0.65, 0.65, '2023-11-24', '2024-06-13', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 400, 8),
('10040010', 'VARAO NERV', 'VARAO NERV A 400 NR 10', 'KG', 76072.00, 'EUR', 1.04, 0.62, 0.61, '2024-05-10', '2024-06-13', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 400, 10),
('10040012', 'VARAO NERV', 'VARAO NERV A 400 NR 12', 'KG', 34244.00, 'EUR', 1.02, 0.61, 0.61, '2024-05-10', '2024-06-13', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 400, 12),
('10040016', 'VARAO NERV', 'VARAO NERV A 400 NR 16', 'KG', 44976.00, 'EUR', 1.01, 0.60, 0.60, '2024-05-10', '2024-06-13', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 400, 16),
('10040020', 'VARAO NERV', 'VARAO NERV A 400 NR 20', 'KG', 24551.00, 'EUR', 1.01, 0.60, 0.61, '2020-05-22', '2024-06-12', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 400, 20),
('10040025', 'VARAO NERV', 'VARAO NERV A 400 NR 25', 'KG', 12112.00, 'EUR', 1.02, 0.62, 0.62, '2020-05-19', '2024-05-23', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 400, 25),
('10040032', 'VARAO NERV', 'VARAO NERV A 400 NR 32', 'KG', 385.00, 'EUR', 1.05, 0.64, 0.64, '2019-02-21', '2020-12-11', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 400, 32),
('10040040', 'VARAO NERV', 'VARAO NERV A 400 NR 40', 'KG', 0.00, 'EUR', 0.94, 0.00, 0.00, '1900-01-01', '1900-01-01', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 400, 40);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('10050006', 'VARAO NERV', 'VARAO NERV A 500 NR 6', 'KG', 0.00, 'EUR', 1.35, 0.69, 0.69, '2024-06-04', '2024-05-28', 1, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 500, 6),
('10050008', 'VARAO NERV', 'VARAO NERV A 500 NR 8', 'KG', 3200.00, 'EUR', 1.08, 0.65, 0.65, '2024-06-12', '2024-06-03', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 500, 8),
('10050010', 'VARAO NERV', 'VARAO NERV A 500 NR 10', 'KG', 0.00, 'EUR', 1.05, 0.63, 0.63, '2024-06-12', '2024-06-13', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 500, 10),
('10050012', 'VARAO NERV', 'VARAO NERV A 500 NR 12', 'KG', 0.00, 'EUR', 1.03, 0.61, 0.61, '2024-06-11', '2024-06-13', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 500, 12),
('10050016', 'VARAO NERV', 'VARAO NERV A 500 NR 16', 'KG', 0.00, 'EUR', 1.02, 0.61, 0.61, '2024-06-11', '2024-06-13', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 500, 16),
('10050020', 'VARAO NERV', 'VARAO NERV A 500 NR 20', 'KG', -1980.00, 'EUR', 1.02, 0.59, 0.59, '2024-05-15', '2024-06-14', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 500, 20),
('10050025', 'VARAO NERV', 'VARAO NERV A 500 NR 25', 'KG', -9290.00, 'EUR', 1.03, 0.60, 0.62, '2024-06-13', '2024-06-14', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 500, 25),
('10050032', 'VARAO NERV', 'VARAO NERV A 500 NR 32', 'KG', 0.00, 'EUR', 1.06, 0.64, 0.64, '2023-11-15', '2023-11-16', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 500, 32),
('10050040', 'VARAO NERV', 'VARAO NERV A 500 NR 40', 'KG', 0.00, 'EUR', 0.89, 0.00, 0.00, '1900-01-01', '1900-01-01', 11, NULL, 1, 1, 11, NULL, 2, 6000, NULL, NULL, 500, 40);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16000080', 'VIGA IPN', 'VIGA IPN 80 C/6.1MT', 'KG', 888.00, 'EUR', 1.03, 0.66, 0.67, '2021-10-07', '2024-06-14', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 80, NULL, NULL),
('16000081', 'VIGA IPN', 'VIGA IPN 80 C/12.1MT', 'KG', 2232.00, 'EUR', 1.03, 0.66, 0.66, '2024-03-13', '2023-07-24', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 80, NULL, NULL),
('16000100', 'VIGA IPN', 'VIGA IPN 100 C/6.1MT', 'KG', 1976.00, 'EUR', 1.03, 0.65, 0.72, '2021-10-04', '2024-06-05', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 100, NULL, NULL),
('16000101', 'VIGA IPN', 'VIGA IPN 100 C/12.1MT', 'KG', 2735.00, 'EUR', 1.03, 0.65, 0.65, '2023-11-20', '2024-03-18', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 100, NULL, NULL),
('16000120', 'VIGA IPN', 'VIGA IPN 120 C/6.1MT', 'KG', 870.00, 'EUR', 1.03, 0.65, 0.85, '2021-10-07', '2024-06-03', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 120, NULL, NULL),
('16000121', 'VIGA IPN', 'VIGA IPN 120 C/12.1MT', 'KG', 4472.00, 'EUR', 1.03, 0.65, 0.65, '2023-11-20', '2023-06-19', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 120, NULL, NULL),
('16000140', 'VIGA IPN', 'VIGA IPN 140 C/6.1MT', 'KG', 0.00, 'EUR', 1.15, 0.78, 0.78, '2020-03-30', '2024-03-08', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 140, NULL, NULL),
('16000141', 'VIGA IPN', 'VIGA IPN 140 C/12.1MT', 'KG', 0.00, 'EUR', 1.15, 0.78, 0.75, '2023-08-09', '2023-09-01', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 140, NULL, NULL),
('16000159', 'VIGA IPN', 'VIGA IPN 160 C/4.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 4100, NULL, 160, NULL, NULL),
('16000160', 'VIGA IPN', 'VIGA IPN 160 C/6.1MT', 'KG', 330.00, 'EUR', 1.15, 0.76, 0.75, '2023-11-07', '2024-05-24', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 160, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16000161', 'VIGA IPN', 'VIGA IPN 160 C/12.1MT', 'KG', 864.00, 'EUR', 1.15, 0.76, 0.76, '2024-05-20', '2023-03-15', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 160, NULL, NULL),
('16000167', 'VIGA IPN', 'VIGA IPN 160 C/7.5MT', 'KG', 0.00, 'EUR', 1.15, 1.01, 1.01, '2022-04-22', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 7500, NULL, 160, NULL, NULL),
('16000180', 'VIGA IPN', 'VIGA IPN 180 C/6.1MT', 'KG', 0.00, 'EUR', 1.15, 0.88, 0.88, '2022-09-09', '2024-03-18', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 180, NULL, NULL),
('16000181', 'VIGA IPN', 'VIGA IPN 180 C/12.1MT', 'KG', 0.00, 'EUR', 1.15, 0.88, 0.88, '2023-02-27', '2024-03-18', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 180, NULL, NULL),
('16000200', 'VIGA IPN', 'VIGA IPN 200 C/6.1MT', 'KG', -161.00, 'EUR', 1.15, 0.76, 0.76, '2024-04-04', '2024-06-13', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 200, NULL, NULL),
('16000201', 'VIGA IPN', 'VIGA IPN 200 C/12.1MT', 'KG', 304.00, 'EUR', 1.15, 0.76, 0.76, '2024-04-04', '2024-04-10', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 200, NULL, NULL),
('16000214', 'VIGA IPN', 'VIGA IPN 200 C/14MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 14000, NULL, 200, NULL, NULL),
('16000215', 'VIGA IPN', 'VIGA IPN 200 C/15MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 15000, NULL, 200, NULL, NULL),
('16000220', 'VIGA IPN', 'VIGA IPN 220 C/6.1MT', 'KG', 0.00, 'EUR', 1.15, 0.90, 0.88, '2021-10-20', '2021-10-22', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 220, NULL, NULL),
('16000221', 'VIGA IPN', 'VIGA IPN 220 C/12.1MT', 'KG', 0.00, 'EUR', 1.15, 0.88, 1.01, '2021-10-25', '2019-12-11', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 220, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16000222', 'VIGA IPN', 'VIGA IPN 220 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 9100, NULL, 220, NULL, NULL),
('16000225', 'VIGA IPN', 'VIGA IPN 220 C/15.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 15100, NULL, 220, NULL, NULL),
('16000240', 'VIGA IPN', 'VIGA IPN 240 C/6.1MT', 'KG', 0.00, 'EUR', 1.23, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 240, NULL, NULL),
('16000241', 'VIGA IPN', 'VIGA IPN 240 C/12.1MT', 'KG', 0.00, 'EUR', 1.23, 0.60, 0.60, '2019-09-23', '2019-09-24', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 240, NULL, NULL),
('16000260', 'VIGA IPN', 'VIGA IPN 260 C/6.1MT', 'KG', 0.00, 'EUR', 1.23, 1.29, 1.29, '2023-04-13', '2023-04-14', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 260, NULL, NULL),
('16000261', 'VIGA IPN', 'VIGA IPN 260 C/12.1MT', 'KG', 0.00, 'EUR', 1.23, 0.60, 0.60, '2019-09-23', '2019-09-24', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 260, NULL, NULL),
('16000280', 'VIGA IPN', 'VIGA IPN 280 C/6.1MT', 'KG', 0.00, 'EUR', 1.23, 0.60, 0.60, '2019-09-23', '2019-09-24', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 280, NULL, NULL),
('16000281', 'VIGA IPN', 'VIGA IPN 280 C/12.1MT', 'KG', 0.00, 'EUR', 1.23, 0.79, 0.79, '2021-02-15', '2021-02-19', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 280, NULL, NULL),
('16000300', 'VIGA IPN', 'VIGA IPN 300 C/6.1MT', 'KG', 0.00, 'EUR', 1.23, 1.29, 1.29, '2023-04-05', '2023-04-13', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 300, NULL, NULL),
('16000301', 'VIGA IPN', 'VIGA IPN 300 C/12.1MT', 'KG', 0.00, 'EUR', 1.23, 1.16, 1.16, '2022-09-23', '2022-09-27', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 300, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16000302', 'VIGA IPN', 'VIGA IPN 300 C/15.1MT', 'KG', 0.00, 'EUR', 1.23, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 15100, NULL, 300, NULL, NULL),
('16000320', 'VIGA IPN', 'VIGA IPN 320 C/6.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 320, NULL, NULL),
('16000321', 'VIGA IPN', 'VIGA IPN 320 C/12.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 320, NULL, NULL),
('16000340', 'VIGA IPN', 'VIGA IPN 340 C/6.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 340, NULL, NULL),
('16000341', 'VIGA IPN', 'VIGA IPN 340 C/12.1MT', 'KG', 0.00, 'EUR', 1.30, 0.82, 0.82, '2021-01-27', '2021-02-02', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 340, NULL, NULL),
('16000360', 'VIGA IPN', 'VIGA IPN 360', 'KG', 0.00, 'EUR', 1.30, 0.61, 0.61, '2020-01-22', '2020-01-22', 13, NULL, 3, 1, 6, NULL, NULL, NULL, NULL, 360, NULL, NULL),
('16000380', 'VIGA IPN', 'VIGA IPN 380 C/6.1MT', 'KG', 0.00, 'EUR', 1.30, 0.78, 0.78, '2021-03-11', '2021-03-11', 13, NULL, 3, 1, 6, NULL, NULL, 6100, NULL, 380, NULL, NULL),
('16000381', 'VIGA IPN', 'VIGA IPN 380 C/12.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, 12100, NULL, 380, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16000400', 'VIGA IPN', 'VIGA IPN 400', 'KG', 0.00, 'EUR', 1.30, 1.38, 1.38, '2022-09-23', '2022-09-27', 13, NULL, 3, 1, 6, NULL, NULL, NULL, 400, NULL, NULL, NULL),
('16000450', 'VIGA IPN', 'VIGA IPN 450', 'KG', 0.00, 'EUR', 1.30, 1.03, 1.03, '2021-10-12', '2021-10-22', 13, NULL, 3, 1, 6, NULL, NULL, NULL, 450, NULL, NULL, NULL),
('16000500', 'VIGA IPN', 'VIGA IPN 500', 'KG', 0.00, 'EUR', 1.30, 0.53, 0.53, '2018-03-01', '2018-03-16', 13, NULL, 3, 1, 6, NULL, NULL, NULL, 500, NULL, NULL, NULL),
('16000550', 'VIGA IPN', 'VIGA IPN 550', 'KG', 0.00, 'EUR', 1.54, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, NULL, 550, NULL, NULL, NULL),
('16000600', 'VIGA IPN', 'VIGA IPN 600', 'KG', 0.00, 'EUR', 1.54, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 6, NULL, NULL, NULL, 600, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010100', 'VIGA HEB', 'VIGA HEB 100', 'KG', 0.00, 'EUR', 1.15, 0.73, 0.53, '2021-03-26', '2018-11-30', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 100, NULL, NULL, NULL),
('16010106', 'VIGA HEB', 'VIGA HEB 100 C/6.1MT', 'KG', 1125.00, 'EUR', 1.15, 0.72, 0.74, '2020-06-09', '2024-06-11', 13, NULL, 3, 1, 7, NULL, 6100, NULL, 100, NULL, NULL),
('16010108', 'VIGA HEB', 'VIGA HEB 100 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 8100, NULL, 100, NULL, NULL),
('16010109', 'VIGA HEB', 'VIGA HEB 100 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 9100, NULL, 100, NULL, NULL),
('16010110', 'VIGA HEB', 'VIGA HEB 100 C/10.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 10100, NULL, 100, NULL, NULL),
('16010112', 'VIGA HEB', 'VIGA HEB 100 C/12.1MT', 'KG', 9622.00, 'EUR', 1.15, 0.72, 0.72, '2024-06-03', '2024-05-02', 13, NULL, 3, 1, 7, NULL, 12100, NULL, 100, NULL, NULL),
('16010114', 'VIGA HEB', 'VIGA HEB 100 C/14.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 14100, NULL, 100, NULL, NULL),
('16010115', 'VIGA HEB', 'VIGA HEB 100 C/15.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 15100, NULL, 100, NULL, NULL),
('16010116', 'VIGA HEB', 'VIGA HEB 100 C/16.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 16100, NULL, 100, NULL, NULL),
('16010120', 'VIGA HEB', 'VIGA HEB 120', 'KG', 157.40, 'EUR', 1.15, 0.96, 0.95, '2023-12-13', '2023-08-11', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 120, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010126', 'VIGA HEB', 'VIGA HEB 120 C/6.1MT', 'KG', 489.00, 'EUR', 1.15, 0.75, 0.76, '2020-06-09', '2024-06-03', 13, NULL, 3, 1, 7, NULL, 6100, NULL, 120, NULL, NULL),
('16010127', 'VIGA HEB', 'VIGA HEB 120 C/7.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 7100, NULL, 120, NULL, NULL),
('16010128', 'VIGA HEB', 'VIGA HEB 120 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 8100, NULL, 120, NULL, NULL),
('16010129', 'VIGA HEB', 'VIGA HEB 120 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 9100, NULL, 120, NULL, NULL),
('16010130', 'VIGA HEB', 'VIGA HEB 120 C/10.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 10100, NULL, 120, NULL, NULL),
('16010132', 'VIGA HEB', 'VIGA HEB 120 C/12.1MT', 'KG', 12268.00, 'EUR', 1.15, 0.72, 0.72, '2024-06-07', '2024-06-03', 13, NULL, 3, 1, 7, NULL, 12100, NULL, 120, NULL, NULL),
('16010134', 'VIGA HEB', 'VIGA HEB 120 C/14.1MT', 'KG', 0.00, 'EUR', 1.15, 0.88, 0.88, '2023-02-13', '2023-02-13', 13, NULL, 3, 1, 7, NULL, 14100, NULL, 120, NULL, NULL),
('16010135', 'VIGA HEB', 'VIGA HEB 120 C/15.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 15100, NULL, 120, NULL, NULL),
('16010136', 'VIGA HEB', 'VIGA HEB 120 C/16.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 16100, NULL, 120, NULL, NULL),
('16010140', 'VIGA HEB', 'VIGA HEB 140', 'KG', 0.00, 'EUR', 1.15, 0.52, 0.00, '2020-07-13', '2021-11-25', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 140, NULL, NULL, NULL);

INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010146', 'VIGA HEB', 'VIGA HEB 140 C/6.1MT', 'KG', 618.00, 'EUR', 1.15, 0.73, 0.74, '2020-09-16', '2024-06-11', 13, NULL, 3, 1, 7, NULL, 6100, NULL, 140, NULL, NULL),
('16010148', 'VIGA HEB', 'VIGA HEB 140 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.57, 0.57, '2021-04-28', '2021-04-29', 13, NULL, 3, 1, 7, NULL, 8100, NULL, 140, NULL, NULL),
('16010149', 'VIGA HEB', 'VIGA HEB 140 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 0.53, 0.00, '2020-06-04', '2020-06-04', 13, NULL, 3, 1, 7, NULL, 9100, NULL, 140, NULL, NULL),
('16010150', 'VIGA HEB', 'VIGA HEB 140 C/10.1MT', 'KG', 0.00, 'EUR', 1.15, 0.55, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, 10100, NULL, 140, NULL, NULL),
('16010152', 'VIGA HEB', 'VIGA HEB 140 C/12.1MT', 'KG', 5292.00, 'EUR', 1.15, 0.73, 0.72, '2024-05-31', '2024-05-20', 13, NULL, 3, 1, 7, NULL, 12100, NULL, 140, NULL, NULL),
('16010154', 'VIGA HEB', 'VIGA HEB 140 C/14.1MT', 'KG', 0.00, 'EUR', 1.15, 0.78, 0.78, '2023-12-18', '2023-12-19', 13, NULL, 3, 1, 7, NULL, 14100, NULL, 140, NULL, NULL),
('16010155', 'VIGA HEB', 'VIGA HEB 140 C/15.1MT', 'KG', 0.00, 'EUR', 1.15, 0.53, 0.53, '2020-06-04', '2020-06-04', 13, NULL, 3, 1, 7, NULL, 15100, NULL, 140, NULL, NULL),
('16010156', 'VIGA HEB', 'VIGA HEB 140 C/16.1MT', 'KG', 0.00, 'EUR', 1.15, 0.53, 0.53, '2020-06-04', '2020-06-04', 13, NULL, 3, 1, 7, NULL, 16100, NULL, 140, NULL, NULL),
('16010160', 'VIGA HEB', 'VIGA HEB 160', 'KG', 0.00, 'EUR', 1.15, 0.53, 0.53, '2022-04-11', '2022-04-11', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 160, NULL, NULL);

INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010166', 'VIGA HEB', 'VIGA HEB 160 C/6.1MT', 'KG', 260.00, 'EUR', 1.15, 0.72, 0.75, '2020-09-16', '2024-05-31', 13, NULL, 3, 1, 7, NULL, NULL, 6100, NULL, 160, NULL, NULL),
('16010167', 'VIGA HEB', 'VIGA HEB 160 C/7.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 7100, NULL, 160, NULL, NULL),
('16010168', 'VIGA HEB', 'VIGA HEB 160 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.72, 0.00, '2021-04-13', '2021-04-14', 13, NULL, 3, 1, 7, NULL, NULL, 8100, NULL, 160, NULL, NULL),
('16010169', 'VIGA HEB', 'VIGA HEB 160 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 1.29, 0.00, '2022-05-26', '2022-05-27', 13, NULL, 3, 1, 7, NULL, NULL, 9100, NULL, 160, NULL, NULL),
('16010170', 'VIGA HEB', 'VIGA HEB 160 C/10.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 10100, NULL, 160, NULL, NULL),
('16010172', 'VIGA HEB', 'VIGA HEB 160 C/12.1MT', 'KG', 7198.00, 'EUR', 1.15, 0.75, 0.75, '2024-05-28', '2024-05-22', 13, NULL, 3, 1, 7, NULL, NULL, 12100, NULL, 160, NULL, NULL),
('16010174', 'VIGA HEB', 'VIGA HEB 160 C/14.1MT', 'KG', 0.00, 'EUR', 1.15, 0.88, 0.88, '2022-12-07', '2022-12-07', 13, NULL, 3, 1, 7, NULL, NULL, 14100, NULL, 160, NULL, NULL),
('16010175', 'VIGA HEB', 'VIGA HEB 160 C/15.1MT', 'KG', 0.00, 'EUR', 1.15, 0.59, 1.29, '2022-05-26', '2019-09-12', 13, NULL, 3, 1, 7, NULL, NULL, 15100, NULL, 160, NULL, NULL),
('16010176', 'VIGA HEB', 'VIGA HEB 160 C/16.1MT', 'KG', 0.00, 'EUR', 1.15, 0.50, 0.50, '2020-07-22', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, NULL, 160, NULL, NULL),
('16010180', 'VIGA HEB', 'VIGA HEB 180', 'KG', 0.00, 'EUR', 1.15, 0.93, 0.93, '2022-08-05', '2022-08-09', 13, NULL, 3, 1, 7, NULL, NULL, NULL, NULL, 180, NULL, NULL);

INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010183', 'VIGA HEB', 'VIGA HEB 180 C/3.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 3100, NULL, 180, NULL, NULL),
('16010186', 'VIGA HEB', 'VIGA HEB 180 C/6.1MT', 'KG', -626.00, 'EUR', 1.15, 0.73, 0.73, '2020-10-27', '2024-06-14', 13, NULL, 3, 1, 7, NULL, NULL, 6100, NULL, 180, NULL, NULL),
('16010188', 'VIGA HEB', 'VIGA HEB 180 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.77, 0.00, '1900-01-01', '2023-06-29', 13, NULL, 3, 1, 7, NULL, NULL, 8100, NULL, 180, NULL, NULL),
('16010189', 'VIGA HEB', 'VIGA HEB 180 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 9100, NULL, 180, NULL, NULL),
('16010190', 'VIGA HEB', 'VIGA HEB 180 C/10.1MT', 'KG', 518.00, 'EUR', 1.15, 0.77, 0.77, '2024-02-20', '2023-09-29', 13, NULL, 3, 1, 7, NULL, NULL, 10100, NULL, 180, NULL, NULL),
('16010192', 'VIGA HEB', 'VIGA HEB 180 C/12.1MT', 'KG', 4953.00, 'EUR', 1.15, 0.73, 0.72, '2024-05-21', '2024-04-29', 13, NULL, 3, 1, 7, NULL, NULL, 12100, NULL, 180, NULL, NULL),
('16010194', 'VIGA HEB', 'VIGA HEB 180 C/14.1MT', 'KG', 0.00, 'EUR', 1.15, 0.77, 0.77, '2024-02-20', '2024-02-23', 13, NULL, 3, 1, 7, NULL, NULL, 14100, NULL, 180, NULL, NULL),
('16010195', 'VIGA HEB', 'VIGA HEB 180 C/15.1MT', 'KG', 0.00, 'EUR', 1.15, 0.77, 0.77, '2024-02-20', '2024-02-23', 13, NULL, 3, 1, 7, NULL, NULL, 15100, NULL, 180, NULL, NULL),
('16010196', 'VIGA HEB', 'VIGA HEB 180 C/16.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, NULL, 180, NULL, NULL),
('16010198', 'VIGA HEB', 'VIGA HEB 180 C/18.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 18100, NULL, 180, NULL, NULL),
('16010200', 'VIGA HEB', 'VIGA HEB 200', 'KG', 0.00, 'EUR', 1.17, 0.86, 0.55, '2019-06-12', '2023-06-12', 13, NULL, 3, 1, 7, NULL, NULL, NULL, NULL, 200, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010205', 'VIGA HEB', 'VIGA HEB 200 C/5.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 5100, NULL, 200, NULL, NULL),
('16010206', 'VIGA HEB', 'VIGA HEB 200 C/6.1MT', 'KG', 393.00, 'EUR', 1.17, 0.74, 0.75, '2020-10-27', '2024-05-24', 13, NULL, 3, 1, 7, NULL, NULL, 6100, NULL, 200, NULL, NULL),
('16010207', 'VIGA HEB', 'VIGA HEB 200 C/7.5MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 7500, NULL, 200, NULL, NULL),
('16010208', 'VIGA HEB', 'VIGA HEB 200 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.55, '2020-10-30', '2024-02-21', 13, NULL, 3, 1, 7, NULL, NULL, 8100, NULL, 200, NULL, NULL),
('16010209', 'VIGA HEB', 'VIGA HEB 200 C/9.1MT', 'KG', 0.00, 'EUR', 1.17, 0.51, 0.00, NULL, '2019-06-13', 13, NULL, 3, 1, 7, NULL, NULL, 9100, NULL, 200, NULL, NULL),
('16010210', 'VIGA HEB', 'VIGA HEB 200 C/10.1MT', 'KG', 1856.00, 'EUR', 1.17, 0.76, 0.76, '2024-05-21', '2024-05-23', 13, NULL, 3, 1, 7, NULL, NULL, 10100, NULL, 200, NULL, NULL),
('16010212', 'VIGA HEB', 'VIGA HEB 200 C/12.1MT', 'KG', 8893.00, 'EUR', 1.17, 0.74, 0.73, '2024-05-21', '2024-06-11', 13, NULL, 3, 1, 7, NULL, NULL, 12100, NULL, 200, NULL, NULL),
('16010214', 'VIGA HEB', 'VIGA HEB 200 C/14.1MT', 'KG', 0.00, 'EUR', 1.17, 0.78, 0.78, '2024-02-20', '2024-04-23', 13, NULL, 3, 1, 7, NULL, NULL, 14100, NULL, 200, NULL, NULL),
('16010215', 'VIGA HEB', 'VIGA HEB 200 C/15.1MT', 'KG', 0.00, 'EUR', 1.17, 0.90, 0.91, '2023-04-03', '2023-04-03', 13, NULL, 3, 1, 7, NULL, NULL, 15100, NULL, 200, NULL, NULL),
('16010216', 'VIGA HEB', 'VIGA HEB 200 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.51, 0.93, '2023-03-22', '2020-12-14', 13, NULL, 3, 1, 7, NULL, NULL, 16100, NULL, 200, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010220', 'VIGA HEB', 'VIGA HEB 220', 'KG', 0.00, 'EUR', 1.17, 0.56, 0.00, '2020-04-15', '2020-04-15', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 220, NULL, NULL, NULL),
('16010226', 'VIGA HEB', 'VIGA HEB 220 C/6.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.76, '2024-03-18', '2024-03-19', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 220, NULL, NULL, NULL),
('16010227', 'VIGA HEB', 'VIGA HEB 220 C/7.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 7100, 220, NULL, NULL, NULL),
('16010228', 'VIGA HEB', 'VIGA HEB 220 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 0.97, 0.00, NULL, '2022-11-16', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 220, NULL, NULL, NULL),
('16010229', 'VIGA HEB', 'VIGA HEB 220 C/9.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 9100, 220, NULL, NULL, NULL),
('16010230', 'VIGA HEB', 'VIGA HEB 220 C/10.1MT', 'KG', 0.00, 'EUR', 1.17, 0.50, 0.50, '2020-07-27', '2018-07-11', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 220, NULL, NULL, NULL),
('16010232', 'VIGA HEB', 'VIGA HEB 220 C/12.1MT', 'KG', 0.00, 'EUR', 1.17, 0.78, 0.78, '2024-02-20', '2024-03-06', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 220, NULL, NULL, NULL),
('16010234', 'VIGA HEB', 'VIGA HEB 220 C/14.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.77, '2024-04-24', '2024-04-29', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 220, NULL, NULL, NULL),
('16010235', 'VIGA HEB', 'VIGA HEB 220 C/15.1MT', 'KG', 0.00, 'EUR', 1.17, 0.63, 0.63, '2018-07-03', '2018-07-05', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 220, NULL, NULL, NULL),
('16010236', 'VIGA HEB', 'VIGA HEB 220 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.66, 0.66, '2019-01-18', '2019-01-22', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 220, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010240', 'VIGA HEB', 'VIGA HEB 240', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 240, NULL, NULL, NULL),
('16010241', 'VIGA HEB', 'VIGA HEB 240 C/4.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 4100, 240, NULL, NULL, NULL),
('16010246', 'VIGA HEB', 'VIGA HEB 240 C/6.1MT', 'KG', 0.00, 'EUR', 1.18, 0.77, 0.77, '2024-06-03', '2024-06-04', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 240, NULL, NULL, NULL),
('16010247', 'VIGA HEB', 'VIGA HEB 240 C/7.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 7100, 240, NULL, NULL, NULL),
('16010248', 'VIGA HEB', 'VIGA HEB 240 C/8.1MT', 'KG', 0.00, 'EUR', 1.18, 0.66, 0.55, '2020-11-24', '2023-02-08', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 240, NULL, NULL, NULL),
('16010249', 'VIGA HEB', 'VIGA HEB 240 C/9.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 9100, 240, NULL, NULL, NULL),
('16010250', 'VIGA HEB', 'VIGA HEB 240 C/10.1MT', 'KG', 0.00, 'EUR', 1.18, 0.81, 0.81, '2023-07-05', '2023-07-07', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 240, NULL, NULL, NULL),
('16010252', 'VIGA HEB', 'VIGA HEB 240 C/12.1MT', 'KG', 0.00, 'EUR', 1.18, 0.77, 0.77, '2024-06-03', '2024-04-03', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 240, NULL, NULL, NULL),
('16010254', 'VIGA HEB', 'VIGA HEB 240 C/14.1MT', 'KG', 0.00, 'EUR', 1.18, 0.66, 0.91, '2023-02-06', '2019-12-30', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 240, NULL, NULL, NULL),
('16010255', 'VIGA HEB', 'VIGA HEB 240 C/15.1MT', 'KG', 0.00, 'EUR', 1.18, 0.74, 0.74, '2021-04-19', '2021-04-26', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 240, NULL, NULL, NULL),
('16010256', 'VIGA HEB', 'VIGA HEB 240 C/16.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 240, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010260', 'VIGA HEB', 'VIGA HEB 260', 'KG', 0.00, 'EUR', 1.18, 0.79, 0.79, '2018-07-18', '2018-07-05', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 260, NULL, NULL, NULL),
('16010266', 'VIGA HEB', 'VIGA HEB 260 C/6.1MT', 'KG', 0.00, 'EUR', 1.18, 0.80, 0.82, '2023-10-12', '2024-05-16', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 260, NULL, NULL, NULL),
('16010268', 'VIGA HEB', 'VIGA HEB 260 C/8.1MT', 'KG', 0.00, 'EUR', 1.18, 0.81, 0.81, '2023-08-10', '2023-09-04', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 260, NULL, NULL, NULL),
('16010269', 'VIGA HEB', 'VIGA HEB 260 C/9.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 9100, 260, NULL, NULL, NULL),
('16010270', 'VIGA HEB', 'VIGA HEB 260 C/10.1MT', 'KG', 0.00, 'EUR', 1.18, 1.03, 1.03, '2022-10-28', '2022-10-31', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 260, NULL, NULL, NULL),
('16010272', 'VIGA HEB', 'VIGA HEB 260 C/12.1MT', 'KG', 0.00, 'EUR', 1.18, 0.80, 0.80, '2024-01-25', '2024-01-26', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 260, NULL, NULL, NULL),
('16010274', 'VIGA HEB', 'VIGA HEB 260 C/14.1MT', 'KG', 0.00, 'EUR', 1.18, 0.80, 0.80, '2023-08-16', '2023-09-04', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 260, NULL, NULL, NULL),
('16010275', 'VIGA HEB', 'VIGA HEB 260 C/15.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 260, NULL, NULL, NULL),
('16010276', 'VIGA HEB', 'VIGA HEB 260 C/16.1MT', 'KG', 0.00, 'EUR', 1.18, 0.81, 0.81, '2023-07-05', '2023-07-07', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 260, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010280', 'VIGA HEB', 'VIGA HEB 280', 'KG', 0.00, 'EUR', 1.30, 0.55, 0.55, '2018-02-10', '2018-02-12', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 280, NULL, NULL, NULL),
('16010286', 'VIGA HEB', 'VIGA HEB 280 C/6.1MT', 'KG', 0.00, 'EUR', 1.30, 0.63, 0.63, '2018-06-30', '2018-12-04', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 280, NULL, NULL, NULL),
('16010288', 'VIGA HEB', 'VIGA HEB 280 C/8.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 280, NULL, NULL, NULL),
('16010290', 'VIGA HEB', 'VIGA HEB 280 C/10.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 280, NULL, NULL, NULL),
('16010292', 'VIGA HEB', 'VIGA HEB 280 C/12.1MT', 'KG', 0.00, 'EUR', 1.30, 0.93, 0.93, '2023-03-10', '2023-03-14', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 280, NULL, NULL, NULL),
('16010294', 'VIGA HEB', 'VIGA HEB 280 C/14.1MT', 'KG', 0.00, 'EUR', 1.30, 0.61, 0.61, '2019-06-27', '2019-07-29', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 280, NULL, NULL, NULL),
('16010295', 'VIGA HEB', 'VIGA HEB 280 C/15.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 280, NULL, NULL, NULL),
('16010296', 'VIGA HEB', 'VIGA HEB 280 C/16.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 280, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010300', 'VIGA HEB', 'VIGA HEB 300', 'KG', 0.00, 'EUR', 1.30, 0.86, 0.86, '2023-07-26', '2023-07-27', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 300, NULL, NULL, NULL),
('16010306', 'VIGA HEB', 'VIGA HEB 300 C/6.1MT', 'KG', 0.00, 'EUR', 1.30, 0.79, 0.79, '2024-04-17', '2024-04-23', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 300, NULL, NULL, NULL),
('16010308', 'VIGA HEB', 'VIGA HEB 300 C/8.1MT', 'KG', 0.00, 'EUR', 1.30, 0.69, 0.69, '2018-03-28', '2018-04-04', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 300, NULL, NULL, NULL),
('16010309', 'VIGA HEB', 'VIGA HEB 300 C/9.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 9100, 300, NULL, NULL, NULL),
('16010310', 'VIGA HEB', 'VIGA HEB 300 C/10.1MT', 'KG', 0.00, 'EUR', 1.30, 0.78, 0.78, '2024-05-29', '2024-06-04', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 300, NULL, NULL, NULL),
('16010312', 'VIGA HEB', 'VIGA HEB 300 C/12.1MT', 'KG', 0.00, 'EUR', 1.30, 0.77, 0.77, '2023-11-23', '2023-11-24', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 300, NULL, NULL, NULL),
('16010314', 'VIGA HEB', 'VIGA HEB 300 C/14.1MT', 'KG', 0.00, 'EUR', 1.30, 0.64, 0.64, '2018-05-02', '2018-05-04', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 300, NULL, NULL, NULL),
('16010315', 'VIGA HEB', 'VIGA HEB 300 C/15.1MT', 'KG', 0.00, 'EUR', 1.30, 1.15, 1.15, '2022-06-20', '2022-06-22', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 300, NULL, NULL, NULL),
('16010316', 'VIGA HEB', 'VIGA HEB 300 C/16.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 300, NULL, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010320', 'VIGA HEB', 'VIGA HEB 320', 'KG', 0.00, 'EUR', 1.30, 0.56, 0.56, '2020-10-07', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 320, NULL, NULL, NULL),
('16010326', 'VIGA HEB', 'VIGA HEB 320 C/6.1MT', 'KG', 0.00, 'EUR', 1.30, 1.03, 1.03, '2022-11-25', '2022-11-25', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 320, NULL, NULL, NULL),
('16010328', 'VIGA HEB', 'VIGA HEB 320 C/8.1MT', 'KG', 0.00, 'EUR', 1.30, 0.56, 0.00, NULL, '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 320, NULL, NULL, NULL),
('16010330', 'VIGA HEB', 'VIGA HEB 320 C/10.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 320, NULL, NULL, NULL),
('16010332', 'VIGA HEB', 'VIGA HEB 320 C/12.1MT', 'KG', 0.00, 'EUR', 1.30, 0.60, 0.60, '2019-12-17', '2019-12-17', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 320, NULL, NULL, NULL),
('16010334', 'VIGA HEB', 'VIGA HEB 320 C/14.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 320, NULL, NULL, NULL),
('16010335', 'VIGA HEB', 'VIGA HEB 320 C/15.1MT', 'KG', 0.00, 'EUR', 1.30, 0.89, 0.89, '2021-05-24', '2021-05-27', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 320, NULL, NULL, NULL),
('16010336', 'VIGA HEB', 'VIGA HEB 320 C/16.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 320, NULL, NULL, NULL),
('16010338', 'VIGA HEB', 'VIGA HEB 320 C/18.1MT', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 18100, 320, NULL, NULL, NULL);

INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010340', 'VIGA HEB', 'VIGA HEB 340', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 340, NULL, NULL, NULL),
('16010346', 'VIGA HEB', 'VIGA HEB 340 C/6.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 340, NULL, NULL, NULL),
('16010348', 'VIGA HEB', 'VIGA HEB 340 C/8.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 340, NULL, NULL, NULL),
('16010350', 'VIGA HEB', 'VIGA HEB 340 C/10.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 340, NULL, NULL, NULL),
('16010352', 'VIGA HEB', 'VIGA HEB 340 C/12.1MT', 'KG', 0.00, 'EUR', 1.43, 1.02, 1.02, '2023-03-22', '2023-03-24', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 340, NULL, NULL, NULL),
('16010354', 'VIGA HEB', 'VIGA HEB 340 C/14.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 340, NULL, NULL, NULL),
('16010355', 'VIGA HEB', 'VIGA HEB 340 C/15.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 340, NULL, NULL, NULL),
('16010356', 'VIGA HEB', 'VIGA HEB 340 C/16.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 340, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010360', 'VIGA HEB', 'VIGA HEB 360', 'KG', 0.00, 'EUR', 1.43, 0.75, 0.75, '2018-01-17', '2018-01-18', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 360, NULL, NULL, NULL),
('16010366', 'VIGA HEB', 'VIGA HEB 360 C/6.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 360, NULL, NULL, NULL),
('16010368', 'VIGA HEB', 'VIGA HEB 360 C/8.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 360, NULL, NULL, NULL),
('16010370', 'VIGA HEB', 'VIGA HEB 360 C/10.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 360, NULL, NULL, NULL),
('16010372', 'VIGA HEB', 'VIGA HEB 360 C/12.1MT', 'KG', 0.00, 'EUR', 1.43, 0.96, 0.96, '2021-11-18', '2021-11-22', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 360, NULL, NULL, NULL),
('16010374', 'VIGA HEB', 'VIGA HEB 360 C/14.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 360, NULL, NULL, NULL),
('16010375', 'VIGA HEB', 'VIGA HEB 360 C/15.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 360, NULL, NULL, NULL),
('16010376', 'VIGA HEB', 'VIGA HEB 360 C/16.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 360, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010400', 'VIGA HEB', 'VIGA HEB 400', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 400, NULL, NULL, NULL),
('16010406', 'VIGA HEB', 'VIGA HEB 400 C/6.1MT', 'KG', 0.00, 'EUR', 1.43, 0.67, 0.67, '2018-08-30', '2018-09-11', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 400, NULL, NULL, NULL),
('16010408', 'VIGA HEB', 'VIGA HEB 400 C/8.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 400, NULL, NULL, NULL),
('16010410', 'VIGA HEB', 'VIGA HEB 400 C/10.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 400, NULL, NULL, NULL),
('16010412', 'VIGA HEB', 'VIGA HEB 400 C/12.1MT', 'KG', 0.00, 'EUR', 1.43, 0.67, 0.67, '2018-08-30', '2018-09-11', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 400, NULL, NULL, NULL),
('16010414', 'VIGA HEB', 'VIGA HEB 400 C/14.1MT', 'KG', 0.00, 'EUR', 1.43, 0.64, 0.64, '2020-12-18', '2021-01-04', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 400, NULL, NULL, NULL),
('16010415', 'VIGA HEB', 'VIGA HEB 400 C/15.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 400, NULL, NULL, NULL),
('16010416', 'VIGA HEB', 'VIGA HEB 400 C/16.1MT', 'KG', 0.00, 'EUR', 1.43, 0.69, 0.69, '2018-03-11', '2018-03-15', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 400, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010450', 'VIGA HEB', 'VIGA HEB 450', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 450, NULL, NULL, NULL),
('16010451', 'VIGA HEB', 'VIGA HEB 450 C/7.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 7100, 450, NULL, NULL, NULL),
('16010456', 'VIGA HEB', 'VIGA HEB 450 C/6.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 450, NULL, NULL, NULL),
('16010458', 'VIGA HEB', 'VIGA HEB 450 C/8.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 450, NULL, NULL, NULL),
('16010460', 'VIGA HEB', 'VIGA HEB 450 C/10.1MT', 'KG', 0.00, 'EUR', 1.47, 0.77, 0.77, '2021-06-04', '2021-06-07', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 450, NULL, NULL, NULL),
('16010462', 'VIGA HEB', 'VIGA HEB 450 C/12.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 450, NULL, NULL, NULL),
('16010464', 'VIGA HEB', 'VIGA HEB 450 C/14.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 450, NULL, NULL, NULL),
('16010465', 'VIGA HEB', 'VIGA HEB 450 C/15.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 450, NULL, NULL, NULL),
('16010466', 'VIGA HEB', 'VIGA HEB 450 C/16.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 450, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010500', 'VIGA HEB', 'VIGA HEB 500', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 500, NULL, NULL, NULL),
('16010506', 'VIGA HEB', 'VIGA HEB 500 C/6.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 500, NULL, NULL, NULL),
('16010508', 'VIGA HEB', 'VIGA HEB 500 C/8.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 500, NULL, NULL, NULL),
('16010510', 'VIGA HEB', 'VIGA HEB 500 C/10.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 500, NULL, NULL, NULL),
('16010512', 'VIGA HEB', 'VIGA HEB 500 C/12.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 500, NULL, NULL, NULL),
('16010514', 'VIGA HEB', 'VIGA HEB 500 C/14.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 500, NULL, NULL, NULL),
('16010515', 'VIGA HEB', 'VIGA HEB 500 C/15.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 500, NULL, NULL, NULL),
('16010516', 'VIGA HEB', 'VIGA HEB 500 C/16.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 500, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010550', 'VIGA HEB', 'VIGA HEB 550', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 550, NULL, NULL, NULL),
('16010556', 'VIGA HEB', 'VIGA HEB 550 C/6.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 550, NULL, NULL, NULL),
('16010558', 'VIGA HEB', 'VIGA HEB 550 C/8.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 550, NULL, NULL, NULL),
('16010560', 'VIGA HEB', 'VIGA HEB 550 C/10.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 550, NULL, NULL, NULL),
('16010562', 'VIGA HEB', 'VIGA HEB 550 C/12.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 550, NULL, NULL, NULL),
('16010564', 'VIGA HEB', 'VIGA HEB 550 C/14.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 550, NULL, NULL, NULL),
('16010565', 'VIGA HEB', 'VIGA HEB 550 C/15.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 550, NULL, NULL, NULL),
('16010566', 'VIGA HEB', 'VIGA HEB 550 C/16.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 550, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010600', 'VIGA HEB', 'VIGA HEB 600', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 600, NULL, NULL, NULL),
('16010606', 'VIGA HEB', 'VIGA HEB 600 C/6.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 600, NULL, NULL, NULL),
('16010608', 'VIGA HEB', 'VIGA HEB 600 C/8.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 600, NULL, NULL, NULL),
('16010610', 'VIGA HEB', 'VIGA HEB 600 C/10.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 600, NULL, NULL, NULL),
('16010612', 'VIGA HEB', 'VIGA HEB 600 C/12.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 600, NULL, NULL, NULL),
('16010614', 'VIGA HEB', 'VIGA HEB 600 C/14.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 600, NULL, NULL, NULL),
('16010615', 'VIGA HEB', 'VIGA HEB 600 C/15.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 600, NULL, NULL, NULL),
('16010616', 'VIGA HEB', 'VIGA HEB 600 C/16.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 600, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16010650', 'VIGA HEB', 'VIGA HEB 650', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, NULL, 650, NULL, NULL, NULL),
('16010656', 'VIGA HEB', 'VIGA HEB 650 C/6.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 6100, 650, NULL, NULL, NULL),
('16010658', 'VIGA HEB', 'VIGA HEB 650 C/8.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 8100, 650, NULL, NULL, NULL),
('16010660', 'VIGA HEB', 'VIGA HEB 650 C/10.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 10100, 650, NULL, NULL, NULL),
('16010662', 'VIGA HEB', 'VIGA HEB 650 C/12.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 12100, 650, NULL, NULL, NULL),
('16010664', 'VIGA HEB', 'VIGA HEB 650 C/14.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 14100, 650, NULL, NULL, NULL),
('16010665', 'VIGA HEB', 'VIGA HEB 650 C/15.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 15100, 650, NULL, NULL, NULL),
('16010666', 'VIGA HEB', 'VIGA HEB 650 C/16.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 16100, 650, NULL, NULL, NULL),
('16010718', 'VIGA HEB', 'VIGA HEB 700 C/18.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 7, NULL, NULL, 18100, 700, NULL, NULL, NULL);


-- HEA



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011100', 'VIGA HEA', 'VIGA HEA 100', 'KG', 0.00, 'EUR', 1.17, 0.72, 0.58, '2017-10-10', '2020-09-11', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 100, NULL, NULL, NULL),
('16011106', 'VIGA HEA', 'VIGA HEA 100 C/6.1MT', 'KG', 306.00, 'EUR', 1.17, 0.76, 0.75, '2021-01-28', '2024-05-31', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 100, NULL, NULL, NULL),
('16011108', 'VIGA HEA', 'VIGA HEA 100 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 100, NULL, NULL, NULL),
('16011110', 'VIGA HEA', 'VIGA HEA 100 C/10.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 100, NULL, NULL, NULL),
('16011112', 'VIGA HEA', 'VIGA HEA 100 C/12.1MT', 'KG', 3403.00, 'EUR', 1.17, 0.76, 0.77, '2024-05-21', '2024-06-13', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 100, NULL, NULL, NULL),
('16011114', 'VIGA HEA', 'VIGA HEA 100 C/14.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 100, NULL, NULL, NULL),
('16011115', 'VIGA HEA', 'VIGA HEA 100 C/15.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 100, NULL, NULL, NULL),
('16011116', 'VIGA HEA', 'VIGA HEA 100 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 100, NULL, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011120', 'VIGA HEA', 'VIGA HEA 120', 'KG', 130.80, 'EUR', 1.17, 1.13, 1.13, '2023-12-13', '2024-02-20', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 120, NULL, NULL, NULL),
('16011126', 'VIGA HEA', 'VIGA HEA 120 C/6.1MT', 'KG', 355.00, 'EUR', 1.17, 0.74, 0.75, '2021-02-26', '2024-05-22', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 120, NULL, NULL, NULL),
('16011128', 'VIGA HEA', 'VIGA HEA 120 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 120, NULL, NULL, NULL),
('16011129', 'VIGA HEA', 'VIGA HEA 120 C/9.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.77, '2023-11-09', '2023-11-09', 13, NULL, 3, 1, 8, NULL, NULL, 9100, 120, NULL, NULL, NULL),
('16011130', 'VIGA HEA', 'VIGA HEA 120 C/10.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 120, NULL, NULL, NULL),
('16011132', 'VIGA HEA', 'VIGA HEA 120 C/12.1MT', 'KG', 6253.00, 'EUR', 1.17, 0.73, 0.73, '2024-06-07', '2024-06-13', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 120, NULL, NULL, NULL),
('16011134', 'VIGA HEA', 'VIGA HEA 120 C/14.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.77, '2023-11-07', '2023-12-20', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 120, NULL, NULL, NULL),
('16011135', 'VIGA HEA', 'VIGA HEA 120 C/15.1MT', 'KG', 0.00, 'EUR', 1.17, 0.78, 0.78, '2024-02-01', '2024-02-15', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 120, NULL, NULL, NULL),
('16011136', 'VIGA HEA', 'VIGA HEA 120 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 120, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011140', 'VIGA HEA', 'VIGA HEA 140', 'KG', 0.00, 'EUR', 1.17, 1.05, 1.05, '2022-06-30', '2022-07-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 140, NULL, NULL, NULL),
('16011145', 'VIGA HEA', 'VIGA HEA 140 C/5.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 5100, 140, NULL, NULL, NULL),
('16011146', 'VIGA HEA', 'VIGA HEA 140 C/6.1MT', 'KG', 302.00, 'EUR', 1.17, 0.75, 0.77, '2022-04-22', '2024-05-28', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 140, NULL, NULL, NULL),
('16011148', 'VIGA HEA', 'VIGA HEA 140 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 140, NULL, NULL, NULL),
('16011149', 'VIGA HEA', 'VIGA HEA 140 C/9.10MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 9100, 140, NULL, NULL, NULL),
('16011150', 'VIGA HEA', 'VIGA HEA 140 C/10.1MT', 'KG', 0.00, 'EUR', 1.17, 0.64, 0.64, '2018-01-16', '2018-01-17', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 140, NULL, NULL, NULL),
('16011152', 'VIGA HEA', 'VIGA HEA 140 C/12.1MT', 'KG', 11652.00, 'EUR', 1.17, 0.73, 0.73, '2024-05-31', '2024-05-07', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 140, NULL, NULL, NULL),
('16011154', 'VIGA HEA', 'VIGA HEA 140 C/14.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.77, '2024-05-14', '2024-05-20', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 140, NULL, NULL, NULL),
('16011155', 'VIGA HEA', 'VIGA HEA 140 C/15.1MT', 'KG', 0.00, 'EUR', 1.17, 0.90, 0.90, '2022-11-29', '2022-11-30', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 140, NULL, NULL, NULL),
('16011156', 'VIGA HEA', 'VIGA HEA 140 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 140, NULL, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011160', 'VIGA HEA', 'VIGA HEA 160', 'KG', 0.00, 'EUR', 1.17, 0.93, 0.00, '2021-12-04', '2022-06-17', 13, NULL, 3, 1, 8, NULL, NULL, 160, 160, 160, NULL, NULL),
('16011164', 'VIGA HEA', 'VIGA HEA 160 c/4.1 MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 4100, 160, 160, NULL, NULL),
('16011166', 'VIGA HEA', 'VIGA HEA 160 C/6.1MT', 'KG', 187.00, 'EUR', 1.17, 0.75, 0.84, '2019-12-10', '2024-06-14', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 160, 160, NULL, NULL),
('16011167', 'VIGA HEA', 'VIGA HEA 160 C/7.1MT', 'KG', 0.00, 'EUR', 1.17, 0.96, 0.00, '2022-01-10', '2022-01-18', 13, NULL, 3, 1, 8, NULL, NULL, 7100, 160, 160, NULL, NULL),
('16011168', 'VIGA HEA', 'VIGA HEA 160 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.90, '2024-03-11', '2024-05-15', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 160, 160, NULL, NULL),
('16011169', 'VIGA HEA', 'VIGA HEA 160 C/9.1MT', 'KG', 0.00, 'EUR', 1.17, 1.00, 0.00, '2022-02-04', '2022-02-04', 13, NULL, 3, 1, 8, NULL, NULL, 9100, 160, 160, NULL, NULL),
('16011170', 'VIGA HEA', 'VIGA HEA 160 C/10.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.76, '2024-03-22', '2024-03-26', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 160, 160, NULL, NULL),
('16011172', 'VIGA HEA', 'VIGA HEA 160 C/12.1MT', 'KG', 2940.00, 'EUR', 1.17, 0.75, 0.74, '2024-03-18', '2024-06-06', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 160, 160, NULL, NULL),
('16011174', 'VIGA HEA', 'VIGA HEA 160 C/14.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.77, '2024-05-10', '2024-03-26', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 160, 160, NULL, NULL),
('16011175', 'VIGA HEA', 'VIGA HEA 160 C/15.1MT', 'KG', 0.00, 'EUR', 1.17, 0.95, 1.00, '2022-02-04', '2020-12-07', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 160, 160, NULL, NULL),
('16011176', 'VIGA HEA', 'VIGA HEA 160 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.92, 0.92, '2023-03-22', NULL, 13, NULL, 3, 1, 8, NULL, NULL, 16100, 160, 160, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011180', 'VIGA HEA', 'VIGA HEA 180', 'KG', 217.00, 'EUR', 1.17, 0.77, 0.00, NULL, NULL, 13, NULL, 3, 1, 8, NULL, NULL, 180, 180, 180, NULL, NULL),
('16011185', 'VIGA HEA', 'VIGA HEA 180 C/5.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 5100, 180, 180, NULL, NULL),
('16011186', 'VIGA HEA', 'VIGA HEA 180 C/6.1MT', 'KG', 2.00, 'EUR', 1.17, 0.75, 0.77, '2017-10-10', '2024-06-11', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 180, 180, NULL, NULL),
('16011187', 'VIGA HEA', 'VIGA HEA 180 C/7.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 7100, 180, 180, NULL, NULL),
('16011188', 'VIGA HEA', 'VIGA HEA 180 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 1.06, 0.00, '2022-07-08', '2022-07-12', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 180, 180, NULL, NULL),
('16011190', 'VIGA HEA', 'VIGA HEA 180 C/10.1MT', 'KG', 0.00, 'EUR', 1.17, 0.80, 0.77, '2023-10-03', '2023-09-29', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 180, 180, NULL, NULL),
('16011192', 'VIGA HEA', 'VIGA HEA 180 C/12.1MT', 'KG', 4291.00, 'EUR', 1.17, 0.75, 0.76, '2024-06-07', '2024-06-06', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 180, 180, NULL, NULL),
('16011194', 'VIGA HEA', 'VIGA HEA 180 C/14.1MT', 'KG', 0.00, 'EUR', 1.17, 0.78, 0.79, '2024-01-16', '2024-01-18', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 180, 180, NULL, NULL),
('16011195', 'VIGA HEA', 'VIGA HEA 180 C/15.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.77, '2023-10-03', '2023-09-29', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 180, 180, NULL, NULL),
('16011196', 'VIGA HEA', 'VIGA HEA 180 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 180, 180, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011200', 'VIGA HEA', 'VIGA HEA 200', 'KG', 0.00, 'EUR', 1.18, 0.01, 0.00, '2020-01-14', '2020-01-16', 13, NULL, 3, 1, 8, NULL, NULL, 200, 200, 200, NULL, NULL),
('16011205', 'VIGA HEA', 'VIGA HEA 200 C/5.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 5100, 200, 200, NULL, NULL),
('16011206', 'VIGA HEA', 'VIGA HEA 200 C/6.1MT', 'KG', 0.00, 'EUR', 1.18, 0.76, 0.79, '2020-06-09', '2024-06-05', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 200, 200, NULL, NULL),
('16011207', 'VIGA HEA', 'VIGA HEA 200 C/7.1MT', 'KG', 0.00, 'EUR', 1.18, 0.77, 0.77, '2023-11-25', '2024-01-04', 13, NULL, 3, 1, 8, NULL, NULL, 7100, 200, 200, NULL, NULL),
('16011208', 'VIGA HEA', 'VIGA HEA 200 C/8.1MT', 'KG', 0.00, 'EUR', 1.18, 0.79, 0.79, '2024-03-11', '2024-03-12', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 200, 200, NULL, NULL),
('16011209', 'VIGA HEA', 'VIGA HEA 200 C/9.1MT', 'KG', 0.00, 'EUR', 1.18, 0.78, 0.52, '2020-07-13', '2023-09-14', 13, NULL, 3, 1, 8, NULL, NULL, 9100, 200, 200, NULL, NULL),
('16011210', 'VIGA HEA', 'VIGA HEA 200 C/10.1MT', 'KG', 0.00, 'EUR', 1.18, 0.98, 0.93, '2023-01-30', '2023-01-30', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 200, 200, NULL, NULL),
('16011212', 'VIGA HEA', 'VIGA HEA 200 C/12.1MT', 'KG', 6651.00, 'EUR', 1.18, 0.76, 0.74, '2024-05-21', '2024-05-15', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 200, 200, NULL, NULL),
('16011214', 'VIGA HEA', 'VIGA HEA 200 C/14.1MT', 'KG', 596.00, 'EUR', 1.18, 0.78, 0.78, '2024-03-07', '2024-02-19', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 200, 200, NULL, NULL),
('16011215', 'VIGA HEA', 'VIGA HEA 200 C/15.1MT', 'KG', 0.00, 'EUR', 1.18, 0.78, 0.78, '2023-10-30', '2023-11-21', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 200, 200, NULL, NULL),
('16011216', 'VIGA HEA', 'VIGA HEA 200 C/16.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 1.00, '2022-02-09', NULL, 13, NULL, 3, 1, 8, NULL, NULL, 16100, 200, 200, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011220', 'VIGA HEA', 'VIGA HEA 220', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, NULL, '2023-04-28', 13, NULL, 3, 1, 8, NULL, NULL, 220, 220, 220, NULL, NULL),
('16011226', 'VIGA HEA', 'VIGA HEA 220 C/6.1MT', 'KG', 309.00, 'EUR', 1.18, 0.78, 0.78, '2024-06-12', '2024-06-11', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 220, 220, NULL, NULL),
('16011228', 'VIGA HEA', 'VIGA HEA 220 C/9.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, NULL, NULL, 13, NULL, 3, 1, 8, NULL, NULL, 9100, 220, 220, NULL, NULL),
('16011229', 'VIGA HEA', 'VIGA HEA 220 C/8.1MT', 'KG', 0.00, 'EUR', 1.18, 0.78, 0.68, '2018-09-14', '2024-04-04', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 220, 220, NULL, NULL),
('16011230', 'VIGA HEA', 'VIGA HEA 220 C/10.1MT', 'KG', 0.00, 'EUR', 1.18, 0.75, 0.75, '2024-05-10', '2024-05-15', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 220, 220, NULL, NULL),
('16011232', 'VIGA HEA', 'VIGA HEA 220 C/12.1MT', 'KG', 2.00, 'EUR', 1.18, 0.74, 0.74, '2023-10-27', '2024-05-03', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 220, 220, NULL, NULL),
('16011234', 'VIGA HEA', 'VIGA HEA 220 C/14.1MT', 'KG', 2137.00, 'EUR', 1.18, 0.78, 0.78, '2024-06-12', NULL, 13, NULL, 3, 1, 8, NULL, NULL, 14100, 220, 220, NULL, NULL),
('16011235', 'VIGA HEA', 'VIGA HEA 220 C/15.1MT', 'KG', 3051.00, 'EUR', 1.18, 0.78, 0.78, '2024-06-12', NULL, 13, NULL, 3, 1, 8, NULL, NULL, 15100, 220, 220, NULL, NULL),
('16011236', 'VIGA HEA', 'VIGA HEA 220 C/16.1MT', 'KG', 0.00, 'EUR', 1.18, 0.78, 0.78, '2024-03-08', '2021-10-25', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 220, 220, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011240', 'VIGA HEA', 'VIGA HEA 240', 'KG', 0.00, 'EUR', 1.20, 0.55, 0.55, '2020-09-29', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 240, 240, NULL, NULL),
('16011246', 'VIGA HEA', 'VIGA HEA 240 C/6.1MT', 'KG', 0.00, 'EUR', 1.20, 0.79, 0.78, '2023-12-05', '2024-04-18', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 240, 240, NULL, NULL),
('16011248', 'VIGA HEA', 'VIGA HEA 240 C/8.1MT', 'KG', 0.00, 'EUR', 1.20, 0.84, 0.94, '2022-11-23', '2024-03-12', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 240, 240, NULL, NULL),
('16011249', 'VIGA HEA', 'VIGA HEA 240 C/9.1MT', 'KG', 0.00, 'EUR', 1.20, 0.94, 0.94, '2023-01-23', '2023-01-24', 13, NULL, 3, 1, 8, NULL, NULL, 9100, 240, 240, NULL, NULL),
('16011250', 'VIGA HEA', 'VIGA HEA 240 C/10.1MT', 'KG', 0.00, 'EUR', 1.20, 0.79, 0.79, '2024-03-08', '2024-03-12', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 240, 240, NULL, NULL),
('16011252', 'VIGA HEA', 'VIGA HEA 240 C/12.1MT', 'KG', 0.00, 'EUR', 1.20, 0.79, 0.79, '2024-04-17', '2024-02-08', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 240, 240, NULL, NULL),
('16011254', 'VIGA HEA', 'VIGA HEA 240 C/14.1MT', 'KG', 0.00, 'EUR', 1.20, 0.94, 0.79, '2024-03-07', '2023-04-03', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 240, 240, NULL, NULL),
('16011255', 'VIGA HEA', 'VIGA HEA 240 C/15.1MT', 'KG', 0.00, 'EUR', 1.20, 0.55, 0.55, '2020-09-22', '2019-01-02', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 240, 240, NULL, NULL),
('16011256', 'VIGA HEA', 'VIGA HEA 240 C/16.1MT', 'KG', 0.00, 'EUR', 1.20, 0.79, 0.79, '2024-03-08', '2021-10-25', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 240, 240, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011260', 'VIGA HEA', 'VIGA HEA 260', 'KG', 0.00, 'EUR', 1.20, 0.50, 0.50, '2018-06-22', '2018-06-19', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 260, 260, NULL, NULL),
('16011266', 'VIGA HEA', 'VIGA HEA 260 C/6.1MT', 'KG', 416.00, 'EUR', 1.20, 0.78, 0.79, '2021-02-12', '2024-02-20', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 260, 260, NULL, NULL),
('16011267', 'VIGA HEA', 'VIGA HEA 260 C/7.1MT', 'KG', 0.00, 'EUR', 1.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 7100, 260, 260, NULL, NULL),
('16011268', 'VIGA HEA', 'VIGA HEA 260 C/8.1MT', 'KG', 0.00, 'EUR', 1.20, 0.60, 0.60, '2019-10-28', '2019-11-08', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 260, 260, NULL, NULL),
('16011269', 'VIGA HEA', 'VIGA HEA 260 C/9.1MT', 'KG', 0.00, 'EUR', 1.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 9100, 260, 260, NULL, NULL),
('16011270', 'VIGA HEA', 'VIGA HEA 260 C/10.1MT', 'KG', 0.00, 'EUR', 1.20, 0.67, 0.67, '2018-09-14', '2018-09-17', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 260, 260, NULL, NULL),
('16011272', 'VIGA HEA', 'VIGA HEA 260 C/12.1MT', 'KG', 0.00, 'EUR', 1.20, 0.78, 0.81, '2024-01-04', '2023-12-06', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 260, 260, NULL, NULL),
('16011274', 'VIGA HEA', 'VIGA HEA 260 C/14.1MT', 'KG', 0.00, 'EUR', 1.20, 0.55, 0.55, '2018-04-15', '2018-04-26', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 260, 260, NULL, NULL),
('16011275', 'VIGA HEA', 'VIGA HEA 260 C/15.1MT', 'KG', 0.00, 'EUR', 1.20, 0.58, 0.58, '2019-10-03', '2019-10-03', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 260, 260, NULL, NULL),
('16011276', 'VIGA HEA', 'VIGA HEA 260 C/16.1MT', 'KG', 0.00, 'EUR', 1.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 260, 260, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011280', 'VIGA HEA', 'VIGA HEA 280', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 280, 280, NULL, NULL),
('16011286', 'VIGA HEA', 'VIGA HEA 280 C/6.1MT', 'KG', 467.00, 'EUR', 1.31, 0.92, 0.58, '2020-12-07', '2023-02-14', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 280, 280, NULL, NULL),
('16011288', 'VIGA HEA', 'VIGA HEA 280 C/8.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 280, 280, NULL, NULL),
('16011289', 'VIGA HEA', 'VIGA HEA 280 C/9.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 9100, 280, 280, NULL, NULL),
('16011290', 'VIGA HEA', 'VIGA HEA 280 C/10.1MT', 'KG', 0.00, 'EUR', 1.31, 0.79, 0.79, '2023-11-23', '2023-11-24', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 280, 280, NULL, NULL),
('16011292', 'VIGA HEA', 'VIGA HEA 280 C/12.1MT', 'KG', 0.00, 'EUR', 1.31, 0.92, 0.92, '2023-02-13', '2023-02-14', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 280, 280, NULL, NULL),
('16011294', 'VIGA HEA', 'VIGA HEA 280 C/14.1MT', 'KG', 0.00, 'EUR', 1.31, 0.92, 0.92, '2023-02-13', '2023-02-14', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 280, 280, NULL, NULL),
('16011295', 'VIGA HEA', 'VIGA HEA 280 C/15.1MT', 'KG', 0.00, 'EUR', 1.31, 0.79, 0.80, '2023-11-23', '2023-11-24', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 280, 280, NULL, NULL),
('16011296', 'VIGA HEA', 'VIGA HEA 280 C/16.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 280, 280, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011300', 'VIGA HEA', 'VIGA HEA 300', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 300, 300, NULL, NULL),
('16011305', 'VIGA HEA', 'VIGA HEA 300 C/5.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 5100, 300, 300, NULL, NULL),
('16011306', 'VIGA HEA', 'VIGA HEA 300 C/6.1MT', 'KG', 0.00, 'EUR', 1.31, 0.55, 0.54, '2020-09-21', '2019-11-05', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 300, 300, NULL, NULL),
('16011307', 'VIGA HEA', 'VIGA HEA 300 C/7.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 7100, 300, 300, NULL, NULL),
('16011308', 'VIGA HEA', 'VIGA HEA 300 C/8.1MT', 'KG', 0.00, 'EUR', 1.31, 1.04, 1.04, '2021-09-17', '2021-10-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 300, 300, NULL, NULL),
('16011309', 'VIGA HEA', 'VIGA HEA 300 C/9.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 9100, 300, 300, NULL, NULL),
('16011310', 'VIGA HEA', 'VIGA HEA 300 C/10.1MT', 'KG', 0.00, 'EUR', 1.31, 0.58, 0.58, '2017-10-10', '2017-11-17', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 300, 300, NULL, NULL),
('16011312', 'VIGA HEA', 'VIGA HEA 300 C/12.1MT', 'KG', 0.00, 'EUR', 1.31, 0.94, 0.94, '2023-01-30', '2023-01-31', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 300, 300, NULL, NULL),
('16011314', 'VIGA HEA', 'VIGA HEA 300 C/14.1MT', 'KG', 0.00, 'EUR', 1.31, 0.54, 0.00, NULL, '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 300, 300, NULL, NULL),
('16011315', 'VIGA HEA', 'VIGA HEA 300 C/15.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 300, 300, NULL, NULL),
('16011316', 'VIGA HEA', 'VIGA HEA 300 C/16.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 300, 300, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011320', 'VIGA HEA', 'VIGA HEA 320', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 320, 320, NULL, NULL),
('16011326', 'VIGA HEA', 'VIGA HEA 320 C/6.1MT', 'KG', 0.00, 'EUR', 1.31, 0.59, 0.59, '2020-04-01', '2020-04-06', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 320, 320, NULL, NULL),
('16011328', 'VIGA HEA', 'VIGA HEA 320 C/8.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 320, 320, NULL, NULL),
('16011329', 'VIGA HEA', 'VIGA HEA 320 C/9.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 9100, 320, 320, NULL, NULL),
('16011330', 'VIGA HEA', 'VIGA HEA 320 C/10.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 320, 320, NULL, NULL),
('16011332', 'VIGA HEA', 'VIGA HEA 320 C/12.1MT', 'KG', 0.00, 'EUR', 1.31, 0.59, 0.59, '2020-04-02', '2020-04-06', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 320, 320, NULL, NULL),
('16011334', 'VIGA HEA', 'VIGA HEA 320 C/14.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 320, 320, NULL, NULL),
('16011335', 'VIGA HEA', 'VIGA HEA 320 C/15.1MT', 'KG', 0.00, 'EUR', 1.31, 0.68, 0.68, '2018-08-06', '2018-08-08', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 320, 320, NULL, NULL),
('16011336', 'VIGA HEA', 'VIGA HEA 320 C/16.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 320, 320, NULL, NULL),
('16011338', 'VIGA HEA', 'VIGA HEA 320 C/18.1MT', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 18100, 320, 320, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011340', 'VIGA HEA', 'VIGA HEA 340', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 340, 340, NULL, NULL),
('16011346', 'VIGA HEA', 'VIGA HEA 340 C/6.1MT', 'KG', 0.00, 'EUR', 1.43, 0.71, 0.71, '2018-03-19', '2018-03-21', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 340, 340, NULL, NULL),
('16011348', 'VIGA HEA', 'VIGA HEA 340 C/8.1MT', 'KG', 0.00, 'EUR', 1.43, 0.60, 0.60, '2020-09-24', '2020-12-07', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 340, 340, NULL, NULL),
('16011350', 'VIGA HEA', 'VIGA HEA 340 C/10.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 340, 340, NULL, NULL),
('16011351', 'VIGA HEA', 'VIGA HEA 340 C/11.1MT', 'KG', 0.00, 'EUR', 1.43, 1.00, 1.00, '2021-06-11', '2021-06-22', 13, NULL, 3, 1, 8, NULL, NULL, 11100, 340, 340, NULL, NULL),
('16011352', 'VIGA HEA', 'VIGA HEA 340 C/12.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 340, 340, NULL, NULL),
('16011354', 'VIGA HEA', 'VIGA HEA 340 C/14.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 340, 340, NULL, NULL),
('16011355', 'VIGA HEA', 'VIGA HEA 340 C/15.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 340, 340, NULL, NULL),
('16011356', 'VIGA HEA', 'VIGA HEA 340 C/16.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 340, 340, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011360', 'VIGA HEA', 'VIGA HEA 360', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 360, 360, NULL, NULL),
('16011366', 'VIGA HEA', 'VIGA HEA 360 C/6.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 360, 360, NULL, NULL),
('16011368', 'VIGA HEA', 'VIGA HEA 360 C/8.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 360, 360, NULL, NULL),
('16011370', 'VIGA HEA', 'VIGA HEA 360 C/10.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 360, 360, NULL, NULL),
('16011372', 'VIGA HEA', 'VIGA HEA 360 C/12.1MT', 'KG', 0.00, 'EUR', 1.43, 1.00, 1.00, '2023-04-14', '2023-04-14', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 360, 360, NULL, NULL),
('16011374', 'VIGA HEA', 'VIGA HEA 360 C/14.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 360, 360, NULL, NULL),
('16011375', 'VIGA HEA', 'VIGA HEA 360 C/15.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 360, 360, NULL, NULL),
('16011376', 'VIGA HEA', 'VIGA HEA 360 C/16.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, NULL, NULL, 13, NULL, 3, 1, 8, NULL, NULL, 16100, 360, 360, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011400', 'VIGA HEA', 'VIGA HEA 400', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 400, 400, NULL, NULL),
('16011406', 'VIGA HEA', 'VIGA HEA 400 C/6.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 400, 400, NULL, NULL),
('16011408', 'VIGA HEA', 'VIGA HEA 400 C/8.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 400, 400, NULL, NULL),
('16011410', 'VIGA HEA', 'VIGA HEA 400 C/10.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 400, 400, NULL, NULL),
('16011412', 'VIGA HEA', 'VIGA HEA 400 C/12.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 400, 400, NULL, NULL),
('16011414', 'VIGA HEA', 'VIGA HEA 400 C/14.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 400, 400, NULL, NULL),
('16011415', 'VIGA HEA', 'VIGA HEA 400 C/15.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 400, 400, NULL, NULL),
('16011416', 'VIGA HEA', 'VIGA HEA 400 C/16.1MT', 'KG', 0.00, 'EUR', 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 400, 400, NULL, NULL),
('16011450', 'VIGA HEA', 'VIGA HEA 450', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 450, 450, NULL, NULL),
('16011456', 'VIGA HEA', 'VIGA HEA 450 C/6.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 450, 450, NULL, NULL),
('16011458', 'VIGA HEA', 'VIGA HEA 450 C/8.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 450, 450, NULL, NULL),
('16011460', 'VIGA HEA', 'VIGA HEA 450 C/10.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 450, 450, NULL, NULL),
('16011462', 'VIGA HEA', 'VIGA HEA 450 C/12.1MT', 'KG', 0.00, 'EUR', 1.47, 1.02, 1.02, '2023-02-06', '2023-02-07', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 450, 450, NULL, NULL),
('16011464', 'VIGA HEA', 'VIGA HEA 450 C/14.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 450, 450, NULL, NULL),
('16011465', 'VIGA HEA', 'VIGA HEA 450 C/15.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 450, 450, NULL, NULL),
('16011466', 'VIGA HEA', 'VIGA HEA 450 C/16.1MT', 'KG', 0.00, 'EUR', 1.47, 0.68, 0.68, '2018-07-22', '2018-07-27', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 450, 450, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011500', 'VIGA HEA', 'VIGA HEA 500', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 500, 500, NULL, NULL),
('16011506', 'VIGA HEA', 'VIGA HEA 500 C/6.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 500, 500, NULL, NULL),
('16011508', 'VIGA HEA', 'VIGA HEA 500 C/8.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 500, 500, NULL, NULL),
('16011510', 'VIGA HEA', 'VIGA HEA 500 C/10.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 500, 500, NULL, NULL),
('16011512', 'VIGA HEA', 'VIGA HEA 500 C/12.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 500, 500, NULL, NULL),
('16011514', 'VIGA HEA', 'VIGA HEA 500 C/14.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 500, 500, NULL, NULL),
('16011515', 'VIGA HEA', 'VIGA HEA 500 C/15.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 500, 500, NULL, NULL),
('16011516', 'VIGA HEA', 'VIGA HEA 500 C/16.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 500, 500, NULL, NULL),
('16011550', 'VIGA HEA', 'VIGA HEA 550', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 550, 550, NULL, NULL),
('16011556', 'VIGA HEA', 'VIGA HEA 550 C/6.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 550, 550, NULL, NULL),
('16011558', 'VIGA HEA', 'VIGA HEA 550 C/8.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 550, 550, NULL, NULL),
('16011560', 'VIGA HEA', 'VIGA HEA 550 C/10.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 550, 550, NULL, NULL),
('16011562', 'VIGA HEA', 'VIGA HEA 550 C/12.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 550, 550, NULL, NULL),
('16011564', 'VIGA HEA', 'VIGA HEA 550 C/14.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 550, 550, NULL, NULL),
('16011565', 'VIGA HEA', 'VIGA HEA 550 C/15.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 550, 550, NULL, NULL),
('16011566', 'VIGA HEA', 'VIGA HEA 550 C/16.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 550, 550, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16011600', 'VIGA HEA', 'VIGA HEA 600', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, NULL, 600, 600, NULL, NULL),
('16011606', 'VIGA HEA', 'VIGA HEA 600 C/6.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 6100, 600, 600, NULL, NULL),
('16011608', 'VIGA HEA', 'VIGA HEA 600 C/8.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 8100, 600, 600, NULL, NULL),
('16011610', 'VIGA HEA', 'VIGA HEA 600 C/10.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 10100, 600, 600, NULL, NULL),
('16011612', 'VIGA HEA', 'VIGA HEA 600 C/12.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 12100, 600, 600, NULL, NULL),
('16011614', 'VIGA HEA', 'VIGA HEA 600 C/14.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 14100, 600, 600, NULL, NULL),
('16011615', 'VIGA HEA', 'VIGA HEA 600 C/15.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 15100, 600, 600, NULL, NULL),
('16011616', 'VIGA HEA', 'VIGA HEA 600 C/16.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 16100, 600, 600, NULL, NULL),
('16011924', 'VIGA HEA', 'VIGA HEA 900 C/24.1MT', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 8, NULL, NULL, 24100, 900, 900, NULL, NULL);



-- HEM 

INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16012100', 'VIGA HEM', 'VIGA HEM 100', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 100, 100, NULL, NULL),
('16012106', 'VIGA HEM', 'VIGA HEM 100 C/6.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 100, 100, NULL, NULL),
('16012108', 'VIGA HEM', 'VIGA HEM 100 C/8.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 100, 100, NULL, NULL),
('16012110', 'VIGA HEM', 'VIGA HEM 100 C/10.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 100, 100, NULL, NULL),
('16012112', 'VIGA HEM', 'VIGA HEM 100 C/12.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 100, 100, NULL, NULL),
('16012114', 'VIGA HEM', 'VIGA HEM 100 C/14.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 100, 100, NULL, NULL),
('16012115', 'VIGA HEM', 'VIGA HEM 100 C/15.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 100, 100, NULL, NULL),
('16012116', 'VIGA HEM', 'VIGA HEM 100 C/16.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 100, 100, NULL, NULL),
('16012120', 'VIGA HEM', 'VIGA HEM 120', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 120, 120, NULL, NULL),
('16012126', 'VIGA HEM', 'VIGA HEM 120 C/6.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 120, 120, NULL, NULL),
('16012128', 'VIGA HEM', 'VIGA HEM 120 C/8.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 120, 120, NULL, NULL),
('16012130', 'VIGA HEM', 'VIGA HEM 120 C/10.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 120, 120, NULL, NULL),
('16012132', 'VIGA HEM', 'VIGA HEM 120 C/12.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 120, 120, NULL, NULL),
('16012134', 'VIGA HEM', 'VIGA HEM 120 C/14.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 120, 120, NULL, NULL),
('16012135', 'VIGA HEM', 'VIGA HEM 120 C/15.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 120, 120, NULL, NULL),
('16012136', 'VIGA HEM', 'VIGA HEM 120 C/16.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 120, 120, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16012140', 'VIGA HEM', 'VIGA HEM 140', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 140, 140, NULL, NULL),
('16012146', 'VIGA HEM', 'VIGA HEM 140 C/6.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 140, 140, NULL, NULL),
('16012148', 'VIGA HEM', 'VIGA HEM 140 C/8.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 140, 140, NULL, NULL),
('16012150', 'VIGA HEM', 'VIGA HEM 140 C/10.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 140, 140, NULL, NULL),
('16012152', 'VIGA HEM', 'VIGA HEM 140 C/12.1MT', 'KG', 0.00, 'EUR', 1.12, 1.10, 1.10, '2022-07-22', '2022-07-26', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 140, 140, NULL, NULL),
('16012154', 'VIGA HEM', 'VIGA HEM 140 C/14.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 140, 140, NULL, NULL),
('16012155', 'VIGA HEM', 'VIGA HEM 140 C/15.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 140, 140, NULL, NULL),
('16012156', 'VIGA HEM', 'VIGA HEM 140 C/16.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 140, 140, NULL, NULL),
('16012160', 'VIGA HEM', 'VIGA HEM 160', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 160, 160, NULL, NULL),
('16012166', 'VIGA HEM', 'VIGA HEM 160 C/6.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 160, 160, NULL, NULL),
('16012168', 'VIGA HEM', 'VIGA HEM 160 C/8.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 160, 160, NULL, NULL),
('16012170', 'VIGA HEM', 'VIGA HEM 160 C/10.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 160, 160, NULL, NULL),
('16012172', 'VIGA HEM', 'VIGA HEM 160 C/12.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 160, 160, NULL, NULL),
('16012174', 'VIGA HEM', 'VIGA HEM 160 C/14.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 160, 160, NULL, NULL),
('16012175', 'VIGA HEM', 'VIGA HEM 160 C/15.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 160, 160, NULL, NULL),
('16012176', 'VIGA HEM', 'VIGA HEM 160 C/16.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 160, 160, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16012180', 'VIGA HEM', 'VIGA HEM 180', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 180, 180, NULL, NULL),
('16012186', 'VIGA HEM', 'VIGA HEM 180 C/6.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 180, 180, NULL, NULL),
('16012188', 'VIGA HEM', 'VIGA HEM 180 C/8.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 180, 180, NULL, NULL),
('16012190', 'VIGA HEM', 'VIGA HEM 180 C/10.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 180, 180, NULL, NULL),
('16012192', 'VIGA HEM', 'VIGA HEM 180 C/12.1MT', 'KG', 0.00, 'EUR', 1.12, 0.85, 0.85, '2021-01-13', '2021-01-14', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 180, 180, NULL, NULL),
('16012194', 'VIGA HEM', 'VIGA HEM 180 C/14.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 180, 180, NULL, NULL),
('16012195', 'VIGA HEM', 'VIGA HEM 180 C/15.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 180, 180, NULL, NULL),
('16012196', 'VIGA HEM', 'VIGA HEM 180 C/16.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 180, 180, NULL, NULL),
('16012200', 'VIGA HEM', 'VIGA HEM 200', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 200, 200, NULL, NULL),
('16012206', 'VIGA HEM', 'VIGA HEM 200 C/6.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 200, 200, NULL, NULL),
('16012208', 'VIGA HEM', 'VIGA HEM 200 C/8.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 200, 200, NULL, NULL),
('16012210', 'VIGA HEM', 'VIGA HEM 200 C/10.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 200, 200, NULL, NULL),
('16012212', 'VIGA HEM', 'VIGA HEM 200 C/12.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 200, 200, NULL, NULL),
('16012214', 'VIGA HEM', 'VIGA HEM 200 C/14.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 200, 200, NULL, NULL),
('16012215', 'VIGA HEM', 'VIGA HEM 200 C/15.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 200, 200, NULL, NULL),
('16012216', 'VIGA HEM', 'VIGA HEM 200 C/16.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 200, 200, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16012220', 'VIGA HEM', 'VIGA HEM 220', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 220, 220, NULL, NULL),
('16012226', 'VIGA HEM', 'VIGA HEM 220 C/6.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 220, 220, NULL, NULL),
('16012228', 'VIGA HEM', 'VIGA HEM 220 C/8.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 220, 220, NULL, NULL),
('16012230', 'VIGA HEM', 'VIGA HEM 220 C/10.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 220, 220, NULL, NULL),
('16012232', 'VIGA HEM', 'VIGA HEM 220 C/12.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 220, 220, NULL, NULL),
('16012234', 'VIGA HEM', 'VIGA HEM 220 C/14.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 220, 220, NULL, NULL),
('16012235', 'VIGA HEM', 'VIGA HEM 220 C/15.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 220, 220, NULL, NULL),
('16012236', 'VIGA HEM', 'VIGA HEM 220 C/16.1MT', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 220, 220, NULL, NULL),
('16012240', 'VIGA HEM', 'VIGA HEM 240', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 240, 240, NULL, NULL),
('16012246', 'VIGA HEM', 'VIGA HEM 240 C/6.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 240, 240, NULL, NULL),
('16012248', 'VIGA HEM', 'VIGA HEM 240 C/8.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 240, 240, NULL, NULL),
('16012250', 'VIGA HEM', 'VIGA HEM 240 C/10.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 240, 240, NULL, NULL),
('16012252', 'VIGA HEM', 'VIGA HEM 240 C/12.1MT', 'KG', 0.00, 'EUR', 1.14, 0.66, 0.66, '2019-11-08', '2019-12-31', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 240, 240, NULL, NULL),
('16012254', 'VIGA HEM', 'VIGA HEM 240 C/14.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 240, 240, NULL, NULL),
('16012255', 'VIGA HEM', 'VIGA HEM 240 C/15.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 240, 240, NULL, NULL),
('16012256', 'VIGA HEM', 'VIGA HEM 240 C/16.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 240, 240, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16012260', 'VIGA HEM', 'VIGA HEM 260', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 260, 260, NULL, NULL),
('16012266', 'VIGA HEM', 'VIGA HEM 260 C/6.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 260, 260, NULL, NULL),
('16012268', 'VIGA HEM', 'VIGA HEM 260 C/8.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 260, 260, NULL, NULL),
('16012270', 'VIGA HEM', 'VIGA HEM 260 C/10.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 260, 260, NULL, NULL),
('16012272', 'VIGA HEM', 'VIGA HEM 260 C/12.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 260, 260, NULL, NULL),
('16012274', 'VIGA HEM', 'VIGA HEM 260 C/14.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 260, 260, NULL, NULL),
('16012275', 'VIGA HEM', 'VIGA HEM 260 C/15.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 260, 260, NULL, NULL),
('16012276', 'VIGA HEM', 'VIGA HEM 260 C/16.1MT', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 260, 260, NULL, NULL),
('16012280', 'VIGA HEM', 'VIGA HEM 280', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 280, 280, NULL, NULL),
('16012286', 'VIGA HEM', 'VIGA HEM 280 C/6.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 280, 280, NULL, NULL),
('16012288', 'VIGA HEM', 'VIGA HEM 280 C/8.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 280, 280, NULL, NULL),
('16012290', 'VIGA HEM', 'VIGA HEM 280 C/10.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 280, 280, NULL, NULL),
('16012292', 'VIGA HEM', 'VIGA HEM 280 C/12.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 280, 280, NULL, NULL),
('16012294', 'VIGA HEM', 'VIGA HEM 280 C/14.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 280, 280, NULL, NULL),
('16012295', 'VIGA HEM', 'VIGA HEM 280 C/15.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 280, 280, NULL, NULL),
('16012296', 'VIGA HEM', 'VIGA HEM 280 C/16.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 280, 280, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16012300', 'VIGA HEM', 'VIGA HEM 300', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 300, 300, NULL, NULL),
('16012306', 'VIGA HEM', 'VIGA HEM 300 C/6.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 300, 300, NULL, NULL),
('16012308', 'VIGA HEM', 'VIGA HEM 300 C/8.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 300, 300, NULL, NULL),
('16012310', 'VIGA HEM', 'VIGA HEM 300 C/10.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 300, 300, NULL, NULL),
('16012312', 'VIGA HEM', 'VIGA HEM 300 C/12.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 300, 300, NULL, NULL),
('16012314', 'VIGA HEM', 'VIGA HEM 300 C/14.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 300, 300, NULL, NULL),
('16012315', 'VIGA HEM', 'VIGA HEM 300 C/15.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 300, 300, NULL, NULL),
('16012316', 'VIGA HEM', 'VIGA HEM 300 C/16.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 300, 300, NULL, NULL),
('16012320', 'VIGA HEM', 'VIGA HEM 320', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 320, 320, NULL, NULL),
('16012326', 'VIGA HEM', 'VIGA HEM 320 C/6.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 320, 320, NULL, NULL),
('16012328', 'VIGA HEM', 'VIGA HEM 320 C/8.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 320, 320, NULL, NULL),
('16012330', 'VIGA HEM', 'VIGA HEM 320 C/10.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 320, 320, NULL, NULL),
('16012332', 'VIGA HEM', 'VIGA HEM 320 C/12.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 320, 320, NULL, NULL),
('16012334', 'VIGA HEM', 'VIGA HEM 320 C/14.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 320, 320, NULL, NULL),
('16012335', 'VIGA HEM', 'VIGA HEM 320 C/15.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 320, 320, NULL, NULL),
('16012336', 'VIGA HEM', 'VIGA HEM 320 C/16.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 320, 320, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16012340', 'VIGA HEM', 'VIGA HEM 340', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 340, 340, NULL, NULL),
('16012346', 'VIGA HEM', 'VIGA HEM 340 C/6.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 340, 340, NULL, NULL),
('16012348', 'VIGA HEM', 'VIGA HEM 340 C/8.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 340, 340, NULL, NULL),
('16012350', 'VIGA HEM', 'VIGA HEM 340 C/10.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 340, 340, NULL, NULL),
('16012352', 'VIGA HEM', 'VIGA HEM 340 C/12.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 340, 340, NULL, NULL),
('16012354', 'VIGA HEM', 'VIGA HEM 340 C/14.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 340, 340, NULL, NULL),
('16012355', 'VIGA HEM', 'VIGA HEM 340 C/15.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 340, 340, NULL, NULL),
('16012356', 'VIGA HEM', 'VIGA HEM 340 C/16.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 340, 340, NULL, NULL),
('16012360', 'VIGA HEM', 'VIGA HEM 360', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 360, 360, NULL, NULL),
('16012366', 'VIGA HEM', 'VIGA HEM 360 C/6.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 360, 360, NULL, NULL),
('16012368', 'VIGA HEM', 'VIGA HEM 360 C/8.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 8100, 360, 360, NULL, NULL),
('16012370', 'VIGA HEM', 'VIGA HEM 360 C/10.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 360, 360, NULL, NULL),
('16012372', 'VIGA HEM', 'VIGA HEM 360 C/12.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 360, 360, NULL, NULL),
('16012374', 'VIGA HEM', 'VIGA HEM 360 C/14.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 360, 360, NULL, NULL),
('16012375', 'VIGA HEM', 'VIGA HEM 360 C/15.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 360, 360, NULL, NULL),
('16012376', 'VIGA HEM', 'VIGA HEM 360 C/16.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 360, 360, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16012400', 'VIGA HEM', 'VIGA HEM 400', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, NULL, 400, 400, NULL, NULL),
('16012406', 'VIGA HEM', 'VIGA HEM 400 C/6.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 6100, 400, 400, NULL, NULL),
('16012408', 'VIGA HEM', 'VIGA HEM 450 C/11.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 11100, 450, 450, NULL, NULL),
('16012410', 'VIGA HEM', 'VIGA HEM 400 C/10.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 10100, 400, 400, NULL, NULL),
('16012412', 'VIGA HEM', 'VIGA HEM 400 C/12.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 12100, 400, 400, NULL, NULL),
('16012414', 'VIGA HEM', 'VIGA HEM 400 C/14.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 14100, 400, 400, NULL, NULL),
('16012415', 'VIGA HEM', 'VIGA HEM 400 C/15.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 15100, 400, 400, NULL, NULL),
('16012416', 'VIGA HEM', 'VIGA HEM 400 C/16.1MT', 'KG', 0.00, 'EUR', 1.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 9, NULL, NULL, 16100, 400, 400, NULL, NULL);




-- IPE 

INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16020080', 'VIGA IPE', 'VIGA IPE 80', 'KG', 0.00, 'EUR', 1.03, 0.68, 0.68, '2024-05-07', '2024-05-08', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 80, 80, NULL, NULL),
('16020100', 'VIGA IPE', 'VIGA IPE 100', 'KG', 0.00, 'EUR', 1.03, 0.28, 0.28, '2024-05-28', '2024-05-29', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 100, 100, NULL, NULL),
('16020106', 'VIGA IPE', 'VIGA IPE 100 C/6.1MT', 'KG', 0.00, 'EUR', 1.03, 0.65, 0.65, '2021-10-11', '2024-06-04', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 100, 100, NULL, NULL),
('16020108', 'VIGA IPE', 'VIGA IPE 100 C/8.1MT', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 100, 100, NULL, NULL),
('16020109', 'VIGA IPE', 'VIGA IPE 100 C/9.1MT', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, '1900-01-01', '2018-12-13', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 100, 100, NULL, NULL),
('16020110', 'VIGA IPE', 'VIGA IPE 100 C/10.1MT', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 100, 100, NULL, NULL),
('16020112', 'VIGA IPE', 'VIGA IPE 100 C/12.1MT', 'KG', 5356.00, 'EUR', 1.03, 0.65, 0.65, '2024-05-08', '2024-06-05', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 100, 100, NULL, NULL),
('16020114', 'VIGA IPE', 'VIGA IPE 100 C/14.1MT', 'KG', 0.00, 'EUR', 1.03, 0.59, 0.59, '2019-07-22', '2019-07-24', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 100, 100, NULL, NULL),
('16020115', 'VIGA IPE', 'VIGA IPE 100 C/15.1MT', 'KG', 0.00, 'EUR', 1.03, 0.62, 0.62, '2018-12-18', '2021-10-06', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 100, 100, NULL, NULL),
('16020116', 'VIGA IPE', 'VIGA IPE 100 C/16.1MT', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 100, 100, NULL, NULL),
('16020120', 'VIGA IPE', 'VIGA IPE 120', 'KG', 74.88, 'EUR', 1.03, 0.93, 0.25, '2019-09-26', '2023-05-05', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 120, 120, NULL, NULL),
('16020126', 'VIGA IPE', 'VIGA IPE 120 C/6.1MT', 'KG', 704.00, 'EUR', 1.03, 0.64, 0.68, '2020-10-09', '2024-06-14', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 120, 120, NULL, NULL),
('16020128', 'VIGA IPE', 'VIGA IPE 120 C/8.1MT', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 120, 120, NULL, NULL),
('16020130', 'VIGA IPE', 'VIGA IPE 120 C/10.1MT', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 120, 120, NULL, NULL),
('16020132', 'VIGA IPE', 'VIGA IPE 120 C/12.1MT', 'KG', 8408.00, 'EUR', 1.03, 0.64, 0.64, '2024-06-04', '2024-06-04', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 120, 120, NULL, NULL),
('16020134', 'VIGA IPE', 'VIGA IPE 120 C/14.1MT', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 120, 120, NULL, NULL),
('16020135', 'VIGA IPE', 'VIGA IPE 120 C/15.1MT', 'KG', 0.00, 'EUR', 1.03, 0.51, 0.51, '2020-06-02', '2020-06-03', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 120, 120, NULL, NULL),
('16020136', 'VIGA IPE', 'VIGA IPE 120 C/16.1MT', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 120, 120, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16020140', 'VIGA IPE', 'VIGA IPE 140', 'KG', 2098.25, 'EUR', 1.15, 0.87, 0.92, '2023-12-13', '2024-04-24', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 140, 140, NULL, NULL),
('16020146', 'VIGA IPE', 'VIGA IPE 140 C/6.1MT', 'KG', 237.00, 'EUR', 1.15, 0.72, 0.74, '2022-09-30', '2024-06-12', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 140, 140, NULL, NULL),
('16020147', 'VIGA IPE', 'VIGA IPE 140 C/7.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 7100, 140, 140, NULL, NULL),
('16020148', 'VIGA IPE', 'VIGA IPE 140 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.74, 0.00, NULL, '2024-03-01', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 140, 140, NULL, NULL),
('16020150', 'VIGA IPE', 'VIGA IPE 140 C/10.1MT', 'KG', 0.00, 'EUR', 1.15, 0.72, 0.72, '2021-04-28', NULL, 13, NULL, 3, 1, 10, NULL, NULL, 10100, 140, 140, NULL, NULL),
('16020152', 'VIGA IPE', 'VIGA IPE 140 C/12.1MT', 'KG', 6683.00, 'EUR', 1.15, 0.72, 0.72, '2024-05-24', '2024-06-13', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 140, 140, NULL, NULL),
('16020154', 'VIGA IPE', 'VIGA IPE 140 C/14.1MT', 'KG', 182.00, 'EUR', 1.15, 0.74, 0.74, '2023-12-04', '2023-12-06', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 140, 140, NULL, NULL),
('16020155', 'VIGA IPE', 'VIGA IPE 140 C/15.1MT', 'KG', 0.00, 'EUR', 1.15, 0.76, 0.76, '2024-04-23', '2021-05-27', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 140, 140, NULL, NULL),
('16020156', 'VIGA IPE', 'VIGA IPE 140 C/16.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 140, 140, NULL, NULL),
('16020160', 'VIGA IPE', 'VIGA IPE 160', 'KG', 604.20, 'EUR', 1.15, 0.89, 0.89, '2022-12-10', '2024-05-24', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 160, 160, NULL, NULL),
('16020163', 'VIGA IPE', 'VIGA IPE 160 C/3.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 3100, 160, 160, NULL, NULL),
('16020164', 'VIGA IPE', 'VIGA IPE 160 C/4.1MT', 'KG', 0.00, 'EUR', 1.15, 0.50, 0.00, NULL, '2020-12-07', 13, NULL, 3, 1, 10, NULL, NULL, 4100, 160, 160, NULL, NULL),
('16020166', 'VIGA IPE', 'VIGA IPE 160 C/6.1MT', 'KG', 141.00, 'EUR', 1.15, 0.75, 0.75, '2022-09-30', '2024-06-13', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 160, 160, NULL, NULL),
('16020167', 'VIGA IPE', 'VIGA IPE 160 C/7.1MT', 'KG', 0.00, 'EUR', 1.15, 0.73, 0.73, '2023-11-22', '2023-12-18', 13, NULL, 3, 1, 10, NULL, NULL, 7100, 160, 160, NULL, NULL),
('16020168', 'VIGA IPE', 'VIGA IPE 160 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.77, 0.56, '2019-12-18', '2024-03-26', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 160, 160, NULL, NULL),
('16020169', 'VIGA IPE', 'VIGA IPE 160 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 1.36, 0.00, '2022-05-13', '2022-05-13', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 160, 160, NULL, NULL),
('16020170', 'VIGA IPE', 'VIGA IPE 160 C/10.1MT', 'KG', 0.00, 'EUR', 1.15, 0.78, 0.78, '2024-01-30', '2024-02-05', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 160, 160, NULL, NULL),
('16020172', 'VIGA IPE', 'VIGA IPE 160 C/12.1MT', 'KG', 3740.00, 'EUR', 1.15, 0.75, 0.78, '2024-01-23', '2024-06-07', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 160, 160, NULL, NULL),
('16020174', 'VIGA IPE', 'VIGA IPE 160 C/14.1MT', 'KG', 1338.00, 'EUR', 1.15, 0.77, 0.73, '2023-10-27', '2024-05-20', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 160, 160, NULL, NULL),
('16020175', 'VIGA IPE', 'VIGA IPE 160 C/15.1MT', 'KG', 232.00, 'EUR', 1.15, 0.76, 0.76, '2024-05-29', '2024-05-31', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 160, 160, NULL, NULL),
('16020176', 'VIGA IPE', 'VIGA IPE 160 C/16.1MT', 'KG', 0.00, 'EUR', 1.15, 0.91, 0.91, '2023-02-22', '2023-02-28', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 160, 160, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16020180', 'VIGA IPE', 'VIGA IPE 180', 'KG', 0.00, 'EUR', 1.15, 0.51, 0.56, '2020-09-07', '2021-06-25', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 180, 180, NULL, NULL),
('16020186', 'VIGA IPE', 'VIGA IPE 180 C/6.1MT', 'KG', 0.00, 'EUR', 1.15, 0.73, 0.77, '2020-10-27', '2024-06-04', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 180, 180, NULL, NULL),
('16020187', 'VIGA IPE', 'VIGA IPE 180 C/7.1MT', 'KG', 0.00, 'EUR', 1.15, 0.79, 0.76, '2024-05-28', '2024-05-31', 13, NULL, 3, 1, 10, NULL, NULL, 7100, 180, 180, NULL, NULL),
('16020188', 'VIGA IPE', 'VIGA IPE 180 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.76, 1.02, '2022-11-10', '2024-05-31', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 180, 180, NULL, NULL),
('16020189', 'VIGA IPE', 'VIGA IPE 180 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 0.86, 0.86, '2023-12-15', '2024-05-08', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 180, 180, NULL, NULL),
('16020190', 'VIGA IPE', 'VIGA IPE 180 C/10.1MT', 'KG', 0.00, 'EUR', 1.15, 0.76, 0.76, '2023-09-07', '2023-09-14', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 180, 180, NULL, NULL),
('16020192', 'VIGA IPE', 'VIGA IPE 180 C/12.1MT', 'KG', 13170.00, 'EUR', 1.15, 0.73, 0.72, '2024-04-10', '2024-05-27', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 180, 180, NULL, NULL),
('16020194', 'VIGA IPE', 'VIGA IPE 180 C/14.1MT', 'KG', 262.00, 'EUR', 1.15, 0.76, 0.76, '2024-05-28', '2024-02-05', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 180, 180, NULL, NULL),
('16020195', 'VIGA IPE', 'VIGA IPE 180 C/15.1MT', 'KG', 568.00, 'EUR', 1.15, 0.86, 0.86, '2023-02-23', '2024-05-27', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 180, 180, NULL, NULL),
('16020196', 'VIGA IPE', 'VIGA IPE 180 C/16.1MT', 'KG', 0.00, 'EUR', 1.15, 1.03, 0.76, '2024-05-28', '2022-10-03', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 180, 180, NULL, NULL),
('16020200', 'VIGA IPE', 'VIGA IPE 200', 'KG', 0.00, 'EUR', 1.15, 0.91, 0.91, '2023-07-18', '2023-07-18', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 200, 200, NULL, NULL),
('16020203', 'VIGA IPE', 'VIGA IPE 200 C/3.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 3100, 200, 200, NULL, NULL),
('16020205', 'VIGA IPE', 'VIGA IPE 200 C/5.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 5100, 200, 200, NULL, NULL),
('16020206', 'VIGA IPE', 'VIGA IPE 200 C/6.1MT', 'KG', 827.00, 'EUR', 1.15, 0.85, 0.76, '2020-09-18', '2024-06-13', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 200, 200, NULL, NULL),
('16020207', 'VIGA IPE', 'VIGA IPE 200 C/7.5MT', 'KG', 0.00, 'EUR', 1.15, 0.66, 0.66, '2021-11-24', '2021-11-24', 13, NULL, 3, 1, 10, NULL, NULL, 7500, 200, 200, NULL, NULL),
('16020208', 'VIGA IPE', 'VIGA IPE 200 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.75, 0.75, '2023-12-27', '2024-06-04', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 200, 200, NULL, NULL),
('16020209', 'VIGA IPE', 'VIGA IPE 200 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 0.93, 0.58, '2019-09-30', '2024-06-05', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 200, 200, NULL, NULL),
('16020210', 'VIGA IPE', 'VIGA IPE 200 C/10.1MT', 'KG', 225.00, 'EUR', 1.15, 0.76, 0.78, '2024-01-16', '2024-03-07', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 200, 200, NULL, NULL),
('16020212', 'VIGA IPE', 'VIGA IPE 200 C/12.1MT', 'KG', 10441.00, 'EUR', 1.15, 0.73, 0.72, '2024-06-07', '2024-06-13', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 200, 200, NULL, NULL),
('16020214', 'VIGA IPE', 'VIGA IPE 200 C/14.1MT', 'KG', 7894.00, 'EUR', 1.15, 0.75, 0.74, '2024-03-12', '2024-05-31', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 200, 200, NULL, NULL),
('16020215', 'VIGA IPE', 'VIGA IPE 200 C/15.1MT', 'KG', 678.00, 'EUR', 1.15, 0.93, 0.93, '2023-03-23', '2024-05-24', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 200, 200, NULL, NULL),
('16020216', 'VIGA IPE', 'VIGA IPE 200 C/16.1MT', 'KG', 61.00, 'EUR', 1.15, 0.76, 0.76, '2024-06-04', '2024-04-11', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 200, 200, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16020220', 'VIGA IPE', 'VIGA IPE 220', 'KG', 0.00, 'EUR', 1.15, 0.57, 0.00, NULL, '2018-11-09', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 220, 220, NULL, NULL),
('16020223', 'VIGA IPE', 'VIGA IPE 220 C/3.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 3100, 220, 220, NULL, NULL),
('16020225', 'VIGA IPE', 'VIGA IPE 220 C/5.1MT', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 5100, 220, 220, NULL, NULL),
('16020226', 'VIGA IPE', 'VIGA IPE 220 C/6.1MT', 'KG', 320.00, 'EUR', 1.15, 0.76, 0.74, '2017-10-10', '2024-06-04', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 220, 220, NULL, NULL),
('16020227', 'VIGA IPE', 'VIGA IPE 220 C/7.1MT', 'KG', 0.00, 'EUR', 1.15, 0.97, 0.00, '2022-10-14', '2022-10-17', 13, NULL, 3, 1, 10, NULL, NULL, 7100, 220, 220, NULL, NULL),
('16020228', 'VIGA IPE', 'VIGA IPE 220 C/8.1MT', 'KG', 0.00, 'EUR', 1.15, 0.80, 0.69, '2017-10-10', '2024-03-21', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 220, 220, NULL, NULL),
('16020229', 'VIGA IPE', 'VIGA IPE 220 C/9.1MT', 'KG', 0.00, 'EUR', 1.15, 0.53, 0.00, '2021-12-09', '2021-12-09', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 220, 220, NULL, NULL),
('16020230', 'VIGA IPE', 'VIGA IPE 220 C/10.1MT', 'KG', 0.00, 'EUR', 1.15, 0.91, 0.91, '2023-04-14', '2023-08-21', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 220, 220, NULL, NULL),
('16020232', 'VIGA IPE', 'VIGA IPE 220 C/12.1MT', 'KG', 5684.00, 'EUR', 1.15, 0.73, 0.72, '2024-06-04', '2024-06-13', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 220, 220, NULL, NULL),
('16020233', 'VIGA IPE', 'VIGA IPE 220 C/13.1MT', 'KG', 0.00, 'EUR', 1.15, 0.97, 0.97, '2021-09-23', '2021-09-24', 13, NULL, 3, 1, 10, NULL, NULL, 13100, 220, 220, NULL, NULL),
('16020234', 'VIGA IPE', 'VIGA IPE 220 C/14.1MT', 'KG', 4810.00, 'EUR', 1.15, 0.80, 0.80, '2023-10-27', '2024-03-12', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 220, 220, NULL, NULL),
('16020235', 'VIGA IPE', 'VIGA IPE 220 C/15.1MT', 'KG', 1584.00, 'EUR', 1.15, 0.99, 0.99, '2022-11-02', '2024-02-16', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 220, 220, NULL, NULL),
('16020236', 'VIGA IPE', 'VIGA IPE 220 C/16.1MT', 'KG', 0.00, 'EUR', 1.15, 0.90, 0.90, '2023-04-14', '2023-04-17', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 220, 220, NULL, NULL);



INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16020240', 'VIGA IPE', 'VIGA IPE 240', 'KG', 0.00, 'EUR', 1.17, 0.56, 0.56, '2019-09-26', '2019-09-30', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 240, 240, NULL, NULL),
('16020245', 'VIGA IPE', 'VIGA IPE 240 C/5.1MT', 'KG', 0.00, 'EUR', 1.17, 0.57, 0.57, '2019-12-20', '2019-12-27', 13, NULL, 3, 1, 10, NULL, NULL, 5100, 240, 240, NULL, NULL),
('16020246', 'VIGA IPE', 'VIGA IPE 240 C/6.1MT', 'KG', 0.00, 'EUR', 1.17, 0.74, 0.77, '2017-10-10', '2024-06-04', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 240, 240, NULL, NULL),
('16020247', 'VIGA IPE', 'VIGA IPE 240 C/7.5MT', 'KG', 0.00, 'EUR', 1.17, 0.90, 0.00, '2022-01-10', '2022-01-18', 13, NULL, 3, 1, 10, NULL, NULL, 7500, 240, 240, NULL, NULL),
('16020248', 'VIGA IPE', 'VIGA IPE 240 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 0.78, 0.78, '2023-06-29', '2024-05-14', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 240, 240, NULL, NULL),
('16020249', 'VIGA IPE', 'VIGA IPE 240 C/9.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.57, '2019-03-25', '2024-04-04', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 240, 240, NULL, NULL),
('16020250', 'VIGA IPE', 'VIGA IPE 240 C/10.1MT', 'KG', 927.00, 'EUR', 1.17, 0.79, 0.79, '2024-01-19', '2024-03-06', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 240, 240, NULL, NULL),
('16020251', 'VIGA IPE', 'VIGA IPE 240 C/11.1MT', 'KG', 0.00, 'EUR', 1.17, 0.68, 0.68, '2020-12-23', '2023-08-02', 13, NULL, 3, 1, 10, NULL, NULL, 11100, 240, 240, NULL, NULL),
('16020252', 'VIGA IPE', 'VIGA IPE 240 C/12.1MT', 'KG', 2213.00, 'EUR', 1.17, 0.74, 0.73, '2024-05-31', '2024-03-28', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 240, 240, NULL, NULL),
('16020254', 'VIGA IPE', 'VIGA IPE 240 C/14.1MT', 'KG', 4328.00, 'EUR', 1.17, 0.75, 0.75, '2024-01-31', '2024-05-09', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 240, 240, NULL, NULL),
('16020255', 'VIGA IPE', 'VIGA IPE 240 C/15.1MT', 'KG', 5099.00, 'EUR', 1.17, 0.77, 0.75, '2024-01-31', '2024-05-09', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 240, 240, NULL, NULL),
('16020256', 'VIGA IPE', 'VIGA IPE 240 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.78, 0.78, '2024-05-13', '2021-06-15', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 240, 240, NULL, NULL);


INSERT INTO t_product_catalog (
    product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('16020270', 'VIGA IPE', 'VIGA IPE 270', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 270, 270, NULL, NULL),
('16020275', 'VIGA IPE', 'VIGA IPE 270 C/5.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 5100, 270, 270, NULL, NULL),
('16020276', 'VIGA IPE', 'VIGA IPE 270 C/6.1MT', 'KG', 442.00, 'EUR', 1.17, 0.75, 0.83, '2024-01-09', '2024-05-14', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 270, 270, NULL, NULL),
('16020277', 'VIGA IPE', 'VIGA IPE 270 C/7.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.00, NULL, '2024-03-28', 13, NULL, 3, 1, 10, NULL, NULL, 7100, 270, 270, NULL, NULL),
('16020278', 'VIGA IPE', 'VIGA IPE 270 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 0.74, 0.81, '2023-03-16', '2024-06-05', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 270, 270, NULL, NULL),
('16020279', 'VIGA IPE', 'VIGA IPE 270 C/9.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.89, '2017-10-10', '2024-05-07', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 270, 270, NULL, NULL),
('16020280', 'VIGA IPE', 'VIGA IPE 270 C/10.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.77, '2024-05-13', '2024-05-14', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 270, 270, NULL, NULL),
('16020281', 'VIGA IPE', 'VIGA IPE 270 C/11.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 11100, 270, 270, NULL, NULL),
('16020282', 'VIGA IPE', 'VIGA IPE 270 C/12.1MT', 'KG', 6990.00, 'EUR', 1.17, 0.74, 0.73, '2024-05-24', '2024-05-14', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 270, 270, NULL, NULL),
('16020283', 'VIGA IPE', 'VIGA IPE 270 C/13.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 13100, 270, 270, NULL, NULL),
('16020284', 'VIGA IPE', 'VIGA IPE 270 C/14.1MT', 'KG', 4041.00, 'EUR', 1.17, 0.74, 0.73, '2024-04-12', '2024-06-13', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 270, 270, NULL, NULL),
('16020285', 'VIGA IPE', 'VIGA IPE 270 C/15.1MT', 'KG', 6543.00, 'EUR', 1.17, 0.76, 0.75, '2024-02-14', '2023-11-27', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 270, 270, NULL, NULL),
('16020286', 'VIGA IPE', 'VIGA IPE 270 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.95, 0.90, '2023-02-17', '2023-02-22', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 270, 270, NULL, NULL),
('16020288', 'VIGA IPE', 'VIGA IPE 270 C/18.1MT', 'KG', 0.00, 'EUR', 1.15, 340.52, 0.52, '2020-07-24', '2020-05-08', 13, NULL, 3, 1, 10, NULL, NULL, 18100, 270, 270, NULL, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1067', '16020300', 'VIGA IPE 300', 'KG', 0.00, 'EUR', 1.17, 0.56, 0.00, '2020-12-22', '2020-12-07', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 300, 300, NULL, NULL),
('1068', '16020306', 'VIGA IPE 300 C/6.1MT', 'KG', 771.00, 'EUR', 1.17, 0.78, 0.77, '2024-02-12', '2024-05-23', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 300, 300, NULL, NULL),
('1069', '16020307', 'VIGA IPE 300 C/7.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.00, NULL, '2023-10-27', 13, NULL, 3, 1, 10, NULL, NULL, 7100, 300, 300, NULL, NULL),
('1070', '16020308', 'VIGA IPE 300 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 0.75, 0.77, '2024-02-27', '2024-03-28', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 300, 300, NULL, NULL),
('1071', '16020309', 'VIGA IPE 300 C/9.1MT', 'KG', 0.00, 'EUR', 1.17, 0.86, 0.86, '2019-01-24', '2024-04-02', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 300, 300, NULL, NULL),
('1072', '16020310', 'VIGA IPE 300 C/10.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.76, '2023-12-11', '2023-12-12', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 300, 300, NULL, NULL),
('1073', '16020312', 'VIGA IPE 300 C/12.1MT', 'KG', 3571.00, 'EUR', 1.17, 0.73, 0.73, '2024-04-26', '2024-06-11', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 300, 300, NULL, NULL),
('1074', '16020313', 'VIGA IPE 300 C/13.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.76, '2024-05-29', '2024-05-31', 13, NULL, 3, 1, 10, NULL, NULL, 13100, 300, 300, NULL, NULL),
('1075', '16020314', 'VIGA IPE 300 C/14.1MT', 'KG', 11305.00, 'EUR', 1.17, 0.73, 0.73, '2024-04-12', '2024-04-24', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 300, 300, NULL, NULL),
('1076', '16020315', 'VIGA IPE 300 C/15.1MT', 'KG', 3823.00, 'EUR', 1.17, 0.86, 0.86, '2023-03-23', '2023-06-27', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 300, 300, NULL, NULL),
('1077', '16020316', 'VIGA IPE 300 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.77, '2024-05-21', '2024-05-23', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 300, 300, NULL, NULL),
('1078', '16020318', 'VIGA IPE 300 C/18.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 18100, 300, 300, NULL, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1067', '16020300', 'VIGA IPE', 'VIGA IPE 300', 'KG', 0.00, 'EUR', 1.17, 0.56, 0.00, '2020-12-22', '2020-12-07', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 300, 300, NULL, NULL),
('1068', '16020306', 'VIGA IPE', 'VIGA IPE 300 C/6.1MT', 'KG', 771.00, 'EUR', 1.17, 0.78, 0.77, '2024-02-12', '2024-05-23', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 300, 300, NULL, NULL),
('1069', '16020307', 'VIGA IPE', 'VIGA IPE 300 C/7.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.00, NULL, '2023-10-27', 13, NULL, 3, 1, 10, NULL, NULL, 7100, 300, 300, NULL, NULL),
('1070', '16020308', 'VIGA IPE', 'VIGA IPE 300 C/8.1MT', 'KG', 0.00, 'EUR', 1.17, 0.75, 0.77, '2024-02-27', '2024-03-28', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 300, 300, NULL, NULL),
('1071', '16020309', 'VIGA IPE', 'VIGA IPE 300 C/9.1MT', 'KG', 0.00, 'EUR', 1.17, 0.86, 0.86, '2019-01-24', '2024-04-02', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 300, 300, NULL, NULL),
('1072', '16020310', 'VIGA IPE', 'VIGA IPE 300 C/10.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.76, '2023-12-11', '2023-12-12', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 300, 300, NULL, NULL),
('1073', '16020312', 'VIGA IPE', 'VIGA IPE 300 C/12.1MT', 'KG', 3571.00, 'EUR', 1.17, 0.73, 0.73, '2024-04-26', '2024-06-11', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 300, 300, NULL, NULL),
('1074', '16020313', 'VIGA IPE', 'VIGA IPE 300 C/13.1MT', 'KG', 0.00, 'EUR', 1.17, 0.76, 0.76, '2024-05-29', '2024-05-31', 13, NULL, 3, 1, 10, NULL, NULL, 13100, 300, 300, NULL, NULL),
('1075', '16020314', 'VIGA IPE', 'VIGA IPE 300 C/14.1MT', 'KG', 11305.00, 'EUR', 1.17, 0.73, 0.73, '2024-04-12', '2024-04-24', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 300, 300, NULL, NULL),
('1076', '16020315', 'VIGA IPE', 'VIGA IPE 300 C/15.1MT', 'KG', 3823.00, 'EUR', 1.17, 0.86, 0.86, '2023-03-23', '2023-06-27', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 300, 300, NULL, NULL),
('1077', '16020316', 'VIGA IPE', 'VIGA IPE 300 C/16.1MT', 'KG', 0.00, 'EUR', 1.17, 0.77, 0.77, '2024-05-21', '2024-05-23', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 300, 300, NULL, NULL),
('1078', '16020318', 'VIGA IPE', 'VIGA IPE 300 C/18.1MT', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 18100, 300, 300, NULL, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1079', '16020330', 'VIGA IPE', 'VIGA IPE 330', 'KG', 0.00, 'EUR', 1.18, 0.52, 0.00, '2020-11-11', NULL, 13, NULL, 3, 1, 10, NULL, NULL, NULL, 330, 330, NULL, NULL),
('1080', '16020333', 'VIGA IPE', 'VIGA IPE 330 C/3.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 3100, 330, 330, NULL, NULL),
('1081', '16020336', 'VIGA IPE', 'VIGA IPE 330 C/6.1MT', 'KG', 600.00, 'EUR', 1.18, 0.76, 0.91, '2022-07-18', '2024-05-03', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 330, 330, NULL, NULL),
('1082', '16020337', 'VIGA IPE', 'VIGA IPE 330 C/7.1MT', 'KG', 0.00, 'EUR', 1.18, 1.14, 1.14, '2022-09-19', '2022-09-20', 13, NULL, 3, 1, 10, NULL, NULL, 7100, 330, 330, NULL, NULL),
('1083', '16020338', 'VIGA IPE', 'VIGA IPE 330 C/8.1MT', 'KG', 0.00, 'EUR', 1.18, 0.76, 0.62, '2019-12-19', '2024-03-28', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 330, 330, NULL, NULL),
('1084', '16020339', 'VIGA IPE', 'VIGA IPE 330 C/9.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 330, 330, NULL, NULL),
('1085', '16020340', 'VIGA IPE', 'VIGA IPE 330 C/10.1MT', 'KG', 0.00, 'EUR', 1.18, 0.77, 0.77, '2023-12-07', '2023-12-12', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 330, 330, NULL, NULL),
('1086', '16020342', 'VIGA IPE', 'VIGA IPE 330 C/12.1MT', 'KG', 4753.00, 'EUR', 1.18, 0.73, 0.73, '2024-04-02', '2024-05-03', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 330, 330, NULL, NULL),
('1087', '16020343', 'VIGA IPE', 'VIGA IPE 330 C/13.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 13100, 330, 330, NULL, NULL),
('1088', '16020344', 'VIGA IPE', 'VIGA IPE 330 C/14.1MT', 'KG', 0.00, 'EUR', 1.18, 0.76, 0.76, '2024-03-26', '2021-07-19', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 330, 330, NULL, NULL),
('1089', '16020345', 'VIGA IPE', 'VIGA IPE 330 C/15.1MT', 'KG', 0.00, 'EUR', 1.18, 0.75, 0.75, '2021-03-23', '2021-03-25', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 330, 330, NULL, NULL),
('1090', '16020346', 'VIGA IPE', 'VIGA IPE 330 C/16.1MT', 'KG', 0.00, 'EUR', 1.18, 0.68, 0.68, '2018-12-10', '2018-12-11', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 330, 330, NULL, NULL),
('1091', '16020348', 'VIGA IPE', 'VIGA IPE 330 C/18.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 18100, 330, 330, NULL, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1092', '16020360', 'VIGA IPE', 'VIGA IPE 360', 'KG', 0.00, 'EUR', 1.18, 0.58, 0.59, '2017-10-10', '2017-10-18', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 360, 360, NULL, NULL),
('1093', '16020362', 'VIGA IPE', 'VIGA IPE 360 C/6.1MT', 'KG', 349.00, 'EUR', 1.18, 0.75, 0.77, '2023-01-26', '2024-06-07', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 360, 360, NULL, NULL),
('1094', '16020368', 'VIGA IPE', 'VIGA IPE 360 C/8.1MT', 'KG', 0.00, 'EUR', 1.18, 0.77, 0.00, NULL, '2023-12-13', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 360, 360, NULL, NULL),
('1095', '16020369', 'VIGA IPE', 'VIGA IPE 360 C/9.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 360, 360, NULL, NULL),
('1096', '16020369', 'VIGA IPE', 'VIGA IPE 360 C/9.1MT', 'UN', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 360, 360, NULL, NULL),
('1097', '16020370', 'VIGA IPE', 'VIGA IPE 360 C/10.1MT', 'KG', 0.00, 'EUR', 1.18, 0.99, 0.99, '2021-10-26', '2021-10-29', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 360, 360, NULL, NULL),
('1098', '16020372', 'VIGA IPE', 'VIGA IPE 360 C/12.1MT', 'KG', 3444.00, 'EUR', 1.18, 0.74, 0.74, '2024-06-04', '2024-06-06', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 360, 360, NULL, NULL),
('1099', '16020373', 'VIGA IPE', 'VIGA IPE 360 C/13.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 13100, 360, 360, NULL, NULL),
('1100', '16020374', 'VIGA IPE', 'VIGA IPE 360 C/14.1MT', 'KG', 0.00, 'EUR', 1.18, 0.77, 0.77, '2023-12-07', '2023-07-19', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 360, 360, NULL, NULL),
('1101', '16020375', 'VIGA IPE', 'VIGA IPE 360 C/15.1MT', 'KG', 0.00, 'EUR', 1.18, 0.79, 0.79, '2023-06-27', '2023-06-30', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 360, 360, NULL, NULL),
('1102', '16020376', 'VIGA IPE', 'VIGA IPE 360 C/16.1MT', 'KG', 0.00, 'EUR', 1.18, 0.55, 0.54, '2020-09-24', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 360, 360, NULL, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1103', '16020400', 'VIGA IPE', 'VIGA IPE 400', 'KG', 0.00, 'EUR', 1.18, 0.63, 0.63, '2017-10-10', '2018-12-04', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 400, 400, NULL, NULL),
('1104', '16020406', 'VIGA IPE', 'VIGA IPE 400 C/6.1MT', 'KG', 0.00, 'EUR', 1.18, 0.88, 0.88, '2023-05-05', '2023-05-10', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 400, 400, NULL, NULL),
('1105', '16020408', 'VIGA IPE', 'VIGA IPE 400 C/8.1MT', 'KG', 0.00, 'EUR', 1.18, 0.78, 0.80, '2024-05-13', '2024-06-11', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 400, 400, NULL, NULL),
('1106', '16020409', 'VIGA IPE', 'VIGA IPE 400 C/9.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 400, 400, NULL, NULL),
('1107', '16020410', 'VIGA IPE', 'VIGA IPE 400 C/10.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 400, 400, NULL, NULL),
('1108', '16020412', 'VIGA IPE', 'VIGA IPE 400 C/12.1MT', 'KG', 0.00, 'EUR', 1.18, 0.88, 0.88, '2023-05-05', '2023-05-08', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 400, 400, NULL, NULL),
('1109', '16020413', 'VIGA IPE', 'VIGA IPE 400 C/13.1MT', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 13100, 400, 400, NULL, NULL),
('1110', '16020414', 'VIGA IPE', 'VIGA IPE 400 C/14.1MT', 'KG', 0.00, 'EUR', 1.18, 0.88, 0.88, '2023-05-05', '2023-05-08', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 400, 400, NULL, NULL),
('1111', '16020415', 'VIGA IPE', 'VIGA IPE 400 C/15.1MT', 'KG', 0.00, 'EUR', 1.18, 0.89, 0.89, '2023-05-05', '2023-05-10', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 400, 400, NULL, NULL),
('1112', '16020416', 'VIGA IPE', 'VIGA IPE 400 C/16.1MT', 'KG', 1068.00, 'EUR', 1.18, 0.78, 0.78, '2024-06-12', '2023-05-10', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 400, 400, NULL, NULL),
('1113', '16020418', 'VIGA IPE', 'VIGA IPE 400 C/18.1MT', 'KG', 0.00, 'EUR', 1.18, 0.66, 0.65, '2019-03-27', '2019-03-27', 13, NULL, 3, 1, 10, NULL, NULL, 18100, 400, 400, NULL, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1114', '16020450', 'VIGA IPE', 'VIGA IPE 450', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 450, 450, NULL, NULL),
('1115', '16020456', 'VIGA IPE', 'VIGA IPE 450 C/6.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 450, 450, NULL, NULL),
('1116', '16020458', 'VIGA IPE', 'VIGA IPE 450 C/7.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 7100, 450, 450, NULL, NULL),
('1117', '16020459', 'VIGA IPE', 'VIGA IPE 450 C/9.10MT', 'KG', 0.00, 'EUR', 1.21, 0.56, 0.56, '2020-09-29', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 450, 450, NULL, NULL),
('1118', '16020460', 'VIGA IPE', 'VIGA IPE 450 C/10.1MT', 'KG', 0.00, 'EUR', 1.21, 0.93, 0.93, '2022-12-22', '2022-12-23', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 450, 450, NULL, NULL),
('1119', '16020462', 'VIGA IPE', 'VIGA IPE 450 C/12.1MT', 'KG', 0.00, 'EUR', 1.21, 0.99, 0.99, '2021-12-21', '2021-12-23', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 450, 450, NULL, NULL),
('1120', '16020464', 'VIGA IPE', 'VIGA IPE 450 C/14.1MT', 'KG', 0.00, 'EUR', 1.21, 0.76, 0.76, '2021-04-28', '2021-05-03', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 450, 450, NULL, NULL),
('1121', '16020465', 'VIGA IPE', 'VIGA IPE 450 C/15.1MT', 'KG', 0.00, 'EUR', 1.21, 0.80, 0.80, '2024-01-04', '2024-01-10', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 450, 450, NULL, NULL),
('1122', '16020466', 'VIGA IPE', 'VIGA IPE 450 C/16.1MT', 'KG', 0.00, 'EUR', 1.21, 0.53, 0.53, '2020-08-21', '2019-10-14', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 450, 450, NULL, NULL),
('1123', '16020468', 'VIGA IPE', 'VIGA IPE 450 C/18.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 18100, 450, 450, NULL, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1124', '16020500', 'VIGA IPE', 'VIGA IPE 500', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 500, 500, NULL, NULL),
('1125', '16020506', 'VIGA IPE', 'VIGA IPE 500 C/6.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 500, 500, NULL, NULL),
('1126', '16020508', 'VIGA IPE', 'VIGA IPE 500 C/8.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 500, 500, NULL, NULL),
('1127', '16020509', 'VIGA IPE', 'VIGA IPE 500 C/9.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 9100, 500, 500, NULL, NULL),
('1128', '16020510', 'VIGA IPE', 'VIGA IPE 500 C/10.1MT', 'KG', 0.00, 'EUR', 1.21, 0.65, 0.65, '2018-08-06', '2018-08-08', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 500, 500, NULL, NULL),
('1129', '16020512', 'VIGA IPE', 'VIGA IPE 500 C/12.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 500, 500, NULL, NULL),
('1130', '16020514', 'VIGA IPE', 'VIGA IPE 500 C/14.1 MT', 'KG', 1279.00, 'EUR', 1.21, 0.60, 0.60, '2020-04-07', '2020-12-07', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 500, 500, NULL, NULL),
('1131', '16020515', 'VIGA IPE', 'VIGA IPE 500 C/15.1MT', 'KG', 0.00, 'EUR', 1.21, 0.65, 0.65, '2018-07-13', '2018-07-16', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 500, 500, NULL, NULL),
('1132', '16020516', 'VIGA IPE', 'VIGA IPE 500 C/16.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 500, 500, NULL, NULL),
('1133', '16020517', 'VIGA IPE', 'VIGA IPE 500 C/17.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 17100, 500, 500, NULL, NULL),
('1134', '16020518', 'VIGA IPE', 'VIGA IPE 500 C/18.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 18100, 500, 500, NULL, NULL);






INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1135', '16020550', 'VIGA IPE', 'VIGA IPE 550', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 550, 550, NULL, NULL),
('1136', '16020556', 'VIGA IPE', 'VIGA IPE 550 C/6.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 550, 550, NULL, NULL),
('1137', '16020558', 'VIGA IPE', 'VIGA IPE 550 C/8.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 550, 550, NULL, NULL),
('1138', '16020560', 'VIGA IPE', 'VIGA IPE 550 C/10.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 550, 550, NULL, NULL),
('1139', '16020562', 'VIGA IPE', 'VIGA IPE 550 C/12.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 550, 550, NULL, NULL),
('1140', '16020564', 'VIGA IPE', 'VIGA IPE 550 C/14.1MT', 'KG', 0.00, 'EUR', 1.21, 0.62, 0.59, '2019-07-08', '2019-07-12', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 550, 550, NULL, NULL),
('1141', '16020565', 'VIGA IPE', 'VIGA IPE 550 C/15.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 550, 550, NULL, NULL),
('1142', '16020566', 'VIGA IPE', 'VIGA IPE 550 C/16.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 550, 550, NULL, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1143', '16020600', 'VIGA IPE', 'VIGA IPE 600', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, NULL, 600, 600, NULL, NULL),
('1144', '16020606', 'VIGA IPE', 'VIGA IPE 600 C/6.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 6100, 600, 600, NULL, NULL),
('1145', '16020608', 'VIGA IPE', 'VIGA IPE 600 C/8.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 8100, 600, 600, NULL, NULL),
('1146', '16020610', 'VIGA IPE', 'VIGA IPE 600 C/10.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 10100, 600, 600, NULL, NULL),
('1147', '16020612', 'VIGA IPE', 'VIGA IPE 600 C/12.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 12100, 600, 600, NULL, NULL),
('1148', '16020614', 'VIGA IPE', 'VIGA IPE 600 C/14.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 14100, 600, 600, NULL, NULL),
('1149', '16020615', 'VIGA IPE', 'VIGA IPE 600 C/15.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 15100, 600, 600, NULL, NULL),
('1150', '16020616', 'VIGA IPE', 'VIGA IPE 600 C/16.1MT', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 13, NULL, 3, 1, 10, NULL, NULL, 16100, 600, 600, NULL, NULL);





-- UPDATE AO VALOR DOS VAROES

UPDATE t_product_catalog 
SET 
    width = thickness,
    height = thickness
WHERE
    thickness IS NOT NULL AND type_id = 1
        AND description LIKE '%VARAO%'





-- Chapas

INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1215, '17414100', 'CHAPA F/PRETA', 'CHAPA F/PRETA (DANIFICADA)', 'KG', 0.00, 'EUR', 0.87, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL),
(1216, '17414120', 'CHAPA F/PRETA', 'CHAPA F/PRETA 1400x1000x2,00', 'KG', 0.00, 'EUR', 1.08, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 1400, 1000, NULL, 2.00, NULL),
(1217, '17416115', 'CHAPA F/PRETA', 'CHAPA F/PRETA 1600x1000x1,50', 'KG', 0.00, 'EUR', 1.19, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 1600, 1000, NULL, 1.50, NULL),
(1218, '17418115', 'CHAPA F/PRETA', 'CHAPA F/PRETA 1883x1000x1,50', 'KG', 0.00, 'EUR', 1.19, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 1883, 1000, NULL, 1.50, NULL),
(1219, '17420012', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x995x12', 'KG', 0.00, 'EUR', 1.08, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 995, NULL, 12.00, NULL),
(1220, '17421001', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x1,50', 'KG', 2880.00, 'EUR', 1.25, 0.82, 0.72, '2023-10-20', '2024-06-13', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 1.50, NULL),
(1221, '17421002', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x2,00', 'KG', 2158.00, 'EUR', 1.14, 0.77, 0.77, '2024-02-08', '2024-06-03', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 2.00, NULL),
(1222, '17421003', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x3,00', 'KG', 3866.00, 'EUR', 1.12, 0.71, 0.71, '2024-05-13', '2024-06-13', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 3.00, NULL),
(1223, '17421004', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x4,00', 'KG', 1958.00, 'EUR', 1.12, 0.75, 0.77, '2024-02-22', '2024-06-13', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 4.00, NULL),
(1224, '17421005', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x5,00', 'KG', 1522.00, 'EUR', 1.12, 0.71, 0.71, '2024-05-15', '2024-06-12', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 5.00, NULL),
(1225, '17421006', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x6,00', 'KG', 476.00, 'EUR', 1.12, 0.71, 0.70, '2023-10-09', '2024-06-14', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 6.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1226, '17421008', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x8,00', 'KG', 3675.00, 'EUR', 1.12, 0.74, 0.75, '2024-04-10', '2024-06-13', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 8.00, NULL),
(1227, '17421010', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x10', 'KG', 1385.00, 'EUR', 1.13, 0.73, 0.73, '2024-05-03', '2024-06-06', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 10.00, NULL),
(1228, '17421012', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x12', 'KG', 2514.00, 'EUR', 1.14, 0.72, 0.72, '2024-05-02', '2024-06-11', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 12.00, NULL),
(1229, '17421014', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x15', 'KG', 0.00, 'EUR', 1.29, 0.67, 0.67, '2018-07-13', '2019-04-03', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 15.00, NULL),
(1230, '17421016', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x16', 'KG', 0.00, 'EUR', 1.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 16.00, NULL),
(1231, '17421018', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x18', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 18.00, NULL),
(1232, '17421020', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x20', 'KG', 0.00, 'EUR', 1.40, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 20.00, NULL),
(1233, '17421025', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x25', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 25.00, NULL),
(1234, '17421030', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x30', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 30.00, NULL),
(1235, '17421035', 'CHAPA F/PRETA', 'CHAPA F/PRETA 1000x1000x30', 'KG', 0.00, 'EUR', 1.38, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 1000, 1000, NULL, 30.00, NULL),
(1236, '17421040', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x40', 'KG', 0.00, 'EUR', 1.44, 0.67, 0.67, '2019-10-30', '2019-11-27', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 40.00, NULL),
(1237, '17421045', 'CHAPA F/PRETA', 'CHAPA F/PRETA 1500x1500x30', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 1500, 1500, NULL, 30.00, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1238, '17421046', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2300x1250x3,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2300, 1250, NULL, 3.00, NULL),
(1239, '17421047', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1000x25', 'KG', 0.00, 'EUR', 1.39, 0.55, 0.55, '2017-10-10', '2018-12-05', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1000, NULL, 25.00, NULL),
(1240, '17421048', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2499x1000x2,00', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2499, 1000, NULL, 2.00, NULL),
(1241, '17421050', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1000x50mm', 'KG', 0.00, 'EUR', 1.24, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1000, NULL, 50.00, NULL),
(1242, '17421140', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1250x4,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1250, NULL, 4.00, NULL),
(1243, '17421153', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x2,50', 'KG', 0.00, 'EUR', 1.14, 0.55, 0.54, '2020-10-23', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 2.50, NULL),
(1244, '17421250', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x1,50', 'KG', 1742.00, 'EUR', 1.25, 0.79, 0.75, '2023-12-18', '2024-06-11', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 1.50, NULL),
(1245, '17421251', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x2,50', 'KG', 0.00, 'EUR', 1.14, 0.78, 0.78, '2024-02-08', NULL, 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 2.50, NULL),
(1246, '17421252', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x2,00', 'KG', 5449.00, 'EUR', 1.14, 0.74, 0.71, '2024-05-03', '2024-06-03', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 2.00, NULL),
(1247, '17421253', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x3,00', 'KG', 5316.00, 'EUR', 1.12, 0.71, 0.71, '2024-05-13', '2024-06-04', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 3.00, NULL),
(1248, '17421254', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x4,00', 'KG', 419.00, 'EUR', 1.12, 0.71, 0.70, '2024-05-03', '2024-06-12', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 4.00, NULL),
(1249, '17421255', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x5,00', 'KG', 4600.00, 'EUR', 1.12, 0.72, 0.70, '2024-05-06', '2024-06-03', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 5.00, NULL),
(1250, '17421256', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x6,00', 'KG', 0.00, 'EUR', 1.12, 0.81, 0.81, '2023-02-02', '2023-03-02', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 6.00, NULL),
(1251, '17421258', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x8,00', 'KG', 0.00, 'EUR', 1.12, 0.49, 0.49, '2017-10-10', '2017-12-21', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 8.00, NULL),
(1252, '17421259', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x10', 'KG', 0.00, 'EUR', 1.13, 0.45, 0.80, '2021-03-11', '2021-03-11', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 10.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1253, '17421260', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1000x4,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1000, NULL, 4.00, NULL),
(1254, '17421261', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1500x5,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1500, NULL, 5.00, NULL),
(1255, '17421262', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x12', 'KG', 0.00, 'EUR', 1.14, 0.71, 0.71, '2023-11-23', '2023-11-27', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 12.00, NULL),
(1256, '17421263', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x20', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 20.00, NULL),
(1257, '17421264', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x15', 'KG', 0.00, 'EUR', 1.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 15.00, NULL),
(1258, '17421265', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3250x1250x3,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3250, 1250, NULL, 3.00, NULL),
(1259, '17421266', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1250x6,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1250, NULL, 6.00, NULL),
(1260, '17421267', 'CHAPA F/PRETA', 'CHAPA F/PRETA 7500x1000x3,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 7500, 1000, NULL, 3.00, NULL),
(1261, '17421268', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1250x3,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1250, NULL, 3.00, NULL),
(1263, '17421269', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2200x1500x8,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2200, 1500, NULL, 8.00, NULL),
(1264, '17421270', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2250x1500x8,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2250, 1500, NULL, 8.00, NULL),
(1265, '17421271', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x2000x30', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 2000, NULL, 30.00, NULL),
(1266, '17421272', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2600x1500x2,00', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2600, 1500, NULL, 2.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1267, '17421273', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1250x4,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1250, NULL, 4.00, NULL),
(1268, '17421274', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1250x2,00', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1250, NULL, 2.00, NULL),
(1269, '17421301', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x1,50', 'KG', 0.00, 'EUR', 1.25, 0.73, 0.73, '2024-05-15', '2024-06-12', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 1.50, NULL),
(1270, '17421302', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x2,00', 'KG', 12691.00, 'EUR', 1.14, 0.74, 0.73, '2024-02-27', '2024-06-11', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 2.00, NULL),
(1271, '17421303', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x3,00', 'KG', 1901.00, 'EUR', 1.12, 0.73, 0.72, '2024-04-01', '2024-06-11', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 3.00, NULL),
(1272, '17421304', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x4,00', 'KG', 4818.00, 'EUR', 1.12, 0.71, 0.71, '2024-05-24', '2024-06-13', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 4.00, NULL),
(1273, '17421305', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x5,00', 'KG', 3348.00, 'EUR', 1.12, 0.70, 0.71, '2024-05-24', '2024-06-12', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 5.00, NULL),
(1274, '17421306', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x6,00', 'KG', 13594.00, 'EUR', 1.12, 0.71, 0.71, '2024-05-24', '2024-06-13', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 6.00, NULL),
(1275, '17421308', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x8,00', 'KG', 9334.00, 'EUR', 1.12, 0.71, 0.71, '2024-05-23', '2024-06-12', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 8.00, NULL),
(1276, '17421309', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x2,5', 'KG', 0.00, 'EUR', 1.14, 0.99, 0.99, '2021-12-07', '2021-12-09', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 2.50, NULL),
(1277, '17421310', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x10', 'KG', 1792.00, 'EUR', 1.13, 0.74, 0.74, '2024-06-07', '2024-06-13', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 10.00, NULL),
(1278, '17421311', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000X1500X6,00', 'KG', 0.00, 'EUR', 1.12, 0.61, 0.61, '2018-10-23', '2018-10-29', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 1500, NULL, 6.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1279, '17421312', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x12', 'KG', 6754.00, 'EUR', 1.14, 0.72, 0.72, '2024-05-13', '2024-04-09', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 12.00, NULL),
(1280, '17421315', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x15', 'KG', 4785.00, 'EUR', 1.29, 0.82, 0.81, '2024-04-22', '2024-06-11', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 15.00, NULL),
(1281, '17421316', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x16', 'KG', 0.00, 'EUR', 1.36, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 16.00, NULL),
(1282, '17421318', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x16', 'KG', 0.00, 'EUR', 1.29, 0.65, 0.65, '2018-12-17', '2018-12-14', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 16.00, NULL),
(1283, '17421320', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x20', 'KG', 9312.00, 'EUR', 1.40, 0.89, 0.89, '2024-04-22', '2024-05-31', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 20.00, NULL),
(1284, '17421325', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x25', 'KG', 0.00, 'EUR', 1.30, 0.97, 0.97, '2024-05-07', '2024-05-09', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 25.00, NULL),
(1285, '17421326', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x30', 'KG', 0.00, 'EUR', 1.27, 1.35, 1.35, '2023-03-02', '2023-03-17', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 30.00, NULL),
(1286, '17421327', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x35', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 35.00, NULL),
(1287, '17421328', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x40', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 40.00, NULL),
(1288, '17421329', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x45', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 45.00, NULL),
(1289, '17421330', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x50', 'KG', 0.00, 'EUR', 1.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 50.00, NULL),
(1290, '17421331', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x2450x12', 'KG', 0.00, 'EUR', 1.14, 0.67, 0.67, '2018-09-03', '2018-09-11', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2450, NULL, 12.00, NULL),
(1291, '17421332', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x30', 'KG', 0.00, 'EUR', 1.36, 0.66, 0.66, '2018-06-01', '2018-06-05', 14, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 30.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1292, '17421333', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000x2500x25', 'KG', 0.00, 'EUR', 1.38, 0.59, 0.58, '2020-10-30', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 2500, NULL, 25.00, NULL),
(1293, '17421334', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x2000x3,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 2000, NULL, 3.00, NULL),
(1294, '17421335', 'CHAPA F/PRETA', 'CHAPA F/PRETA 8000x2000x15', 'KG', 0.00, 'EUR', 1.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 8000, 2000, NULL, 15.00, NULL),
(1295, '17421336', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3300x1000x3,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3300, 1000, NULL, 3.00, NULL),
(1296, '17421337', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x3,00', 'KG', 0.00, 'EUR', 0.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 3.00, NULL),
(1297, '17421338', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2800x2500x10', 'KG', 0.00, 'EUR', 1.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2800, 2500, NULL, 10.00, NULL),
(1298, '17421339', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000x1000x3,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 1000, NULL, 3.00, NULL),
(1299, '17421340', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x2000x30', 'KG', 0.00, 'EUR', 1.36, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 2000, NULL, 30.00, NULL),
(1300, '17421341', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3765x1500x2,00', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3765, 1500, NULL, 2.00, NULL),
(1301, '17421342', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1000x2,50', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1000, NULL, 2.50, NULL),
(1302, '17421343', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1000x4 mm', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1000, NULL, 4.00, NULL),
(1303, '17421344', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1250x1,50', 'KG', 0.00, 'EUR', 1.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 1250, NULL, 1.50, NULL),
(1304, '17421400', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000x1000x4,00', 'KG', 0.00, 'EUR', 1.12, 0.56, 0.55, '2019-07-02', '2019-07-03', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 1000, NULL, 4.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1305, '17421401', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4300x1000x5,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4300, 1000, NULL, 5.00, NULL),
(1306, '17421402', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000x1500x3,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 1500, NULL, 3.00, NULL),
(1307, '17421403', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4500x1500x5,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4500, 1500, NULL, 5.00, NULL),
(1308, '17421404', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6500x2000x6,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6500, 2000, NULL, 6.00, NULL),
(1309, '17421405', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000x2000x5,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 2000, NULL, 5.00, NULL),
(1310, '17421406', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000x1500x4,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 1500, NULL, 4.00, NULL),
(1312, '17421407', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4500x1500x10', 'KG', 0.00, 'EUR', 1.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4500, 1500, NULL, 10.00, NULL),
(1314, '17421408', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000x2500x22', 'KG', 0.00, 'EUR', 1.30, 0.63, 0.63, '2020-09-24', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 2500, NULL, 22.00, NULL),
(1316, '17421409', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000x2000x6,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 2000, NULL, 6.00, NULL),
(1318, '17421423', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000x2000x3,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 2000, NULL, 3.00, NULL),
(1319, '17421424', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3080x2000x4,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3080, 2000, NULL, 4.00, NULL),
(1320, '17421428', 'CHAPA F/PRETA', 'CHAPA F/PRETA 8000x2000x8,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 8000, 2000, NULL, 8.00, NULL),
(1321, '17421430', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4000x2000x10,0', 'KG', 0.00, 'EUR', 1.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 4000, 2000, NULL, 10.00, NULL),
(1322, '17421431', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3200x1250x4,00', 'KG', 0.00, 'EUR', 1.12, 0.55, 0.55, '2019-09-18', '2019-09-19', 14, NULL, 5, 1, NULL, 3, NULL, 3200, 1250, NULL, 4.00, NULL),
(1323, '17421432', 'CHAPA F/PRETA', 'CHAPA F/PRETA 4250x1250x4,00', 'KG', 0.00, 'EUR', 1.12, 0.55, 0.55, '2019-09-18', '2019-09-19', 14, NULL, 5, 1, NULL, 3, NULL, 4250, 1250, NULL, 4.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1324, '17421440', 'CHAPA F/PRETA "NAVAL"', 'CHAPA F/PRETA "NAVAL" 3000x2500x6', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 2500, NULL, 6.00, NULL),
(1325, '17421520', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x20', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 20.00, NULL),
(1326, '17421551', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x15', 'KG', 0.00, 'EUR', 1.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 15.00, NULL),
(1327, '17421552', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x2500x20', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2500, NULL, 20.00, NULL),
(1328, '17421553', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x6,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 6.00, NULL),
(1329, '17421554', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x2000x4,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2000, NULL, 4.00, NULL),
(1330, '17421555', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x5,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 5.00, NULL),
(1331, '17421558', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x10', 'KG', 0.00, 'EUR', 1.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 10.00, NULL),
(1332, '17421559', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x8,00', 'KG', 0.00, 'EUR', 1.12, 0.49, 0.49, '2020-07-22', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 8.00, NULL),
(1333, '17421560', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x2000x8,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2000, NULL, 8.00, NULL),
(1334, '17421610', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x2000x10', 'KG', 0.00, 'EUR', 1.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2000, NULL, 10.00, NULL),
(1335, '17421612', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x2000x6,00', 'KG', 0.00, 'EUR', 1.12, 1.15, 1.15, '2023-06-06', '2023-06-16', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2000, NULL, 6.00, NULL),
(1336, '17421613', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x2000x3,00', 'KG', 0.00, 'EUR', 1.12, 0.78, 0.78, '2023-12-04', '2023-12-06', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2000, NULL, 3.00, NULL),
(1337, '17421614', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x2000x20', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2000, NULL, 20.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1338, '17421615', 'CHAPA F/PRETA', 'CHAPA F/PRETA 12000X2500X15 MM', 'KG', 0.00, 'EUR', 1.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 12000, 2500, NULL, 15.00, NULL),
(1339, '17421616', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000X2000X12', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2000, NULL, 12.00, NULL),
(1340, '17421617', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x2500x8', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2500, NULL, 8.00, NULL),
(1341, '17421650', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x12', 'KG', 0.00, 'EUR', 1.14, 0.85, 0.85, '2022-11-02', '2022-12-09', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 12.00, NULL),
(1342, '17421651', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x3,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 3.00, NULL),
(1343, '17421815', 'CHAPA F/PRETA', 'CHAPA F/PRETA 5000x1250x15 mm', 'KG', 0.00, 'EUR', 1.29, 0.61, 0.61, '2017-10-10', '2017-12-21', 14, NULL, 5, 1, NULL, 3, NULL, 5000, 1250, NULL, 15.00, NULL),
(1344, '17421816', 'CHAPA F/PRETA', 'CHAPA F/PRETA 8000x2000x3mm', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 8000, 2000, NULL, 3.00, NULL),
(1345, '17421817', 'CHAPA F/PRETA', 'CHAPA F/PRETA 8000X2000X25mm', 'KG', 0.00, 'EUR', 1.30, 0.58, 0.58, '2020-09-24', '2020-01-22', 14, NULL, 5, 1, NULL, 3, NULL, 8000, 2000, NULL, 25.00, NULL),
(1346, '17421818', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x2000x5,00', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 2000, NULL, 5.00, NULL),
(1347, '17421819', 'CHAPA F/PRETA', 'CHAPA F/PRETA 7000x1500x6mm', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 7000, 1500, NULL, 6.00, NULL),
(1348, '17421820', 'CHAPA F/PRETA', 'CHAPA F/PRETA 5000x1250x4mm', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 5000, 1250, NULL, 4.00, NULL),
(1349, '17421821', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x4mm', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 4.00, NULL),
(1350, '17421822', 'CHAPA F/PRETA', 'CHAPA F/PRETA 6000x1500x10 MM', 'KG', 0.00, 'EUR', 1.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 6000, 1500, NULL, 10.00, NULL),
(1351, '17421823', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 1500x4mm (ROLO)', 'KG', 0.00, 'EUR', 1.12, 0.56, 0.56, '2019-07-15', '2019-07-17', 14, NULL, 5, 1, NULL, 3, NULL, NULL, 1500, NULL, 4.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1352, '17426123', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 30x3mm (ROLO)', 'KG', 0.00, 'EUR', 1.12, 0.70, 0.70, '2024-05-16', '2024-05-17', 14, NULL, 5, 1, NULL, 3, NULL, 30, NULL, NULL, 3.00, NULL),
(1353, '17426124', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/ PRETA 50x2,50 MM (ROLO)', 'KG', 0.00, 'EUR', 1.14, 0.45, 0.75, '2022-11-25', '2022-11-30', 14, NULL, 5, 1, NULL, 3, NULL, 50, NULL, NULL, 2.50, NULL),
(1354, '17426125', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 50x3mm (ROLO)', 'KG', 0.00, 'EUR', 1.12, 0.68, 0.68, '2024-02-05', '2024-02-06', 14, NULL, 5, 1, NULL, 3, NULL, 50, NULL, NULL, 3.00, NULL),
(1355, '17426126', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 34x3mm (ROLO)', 'KG', 0.00, 'EUR', 1.12, 0.57, 0.57, '2018-05-14', '2018-05-16', 14, NULL, 5, 1, NULL, 3, NULL, 34, NULL, NULL, 3.00, NULL),
(1356, '17426127', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 40x3mm (ROLO)', 'KG', 0.00, 'EUR', 1.12, 0.70, 0.70, '2024-05-13', '2024-05-14', 14, NULL, 5, 1, NULL, 3, NULL, 40, NULL, NULL, 3.00, NULL),
(1357, '17426128', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 40x2,5mm (ROLO)', 'KG', 0.00, 'EUR', 1.14, 1.11, 1.11, '2021-08-13', '2021-09-01', 14, NULL, 5, 1, NULL, 3, NULL, 40, NULL, NULL, 2.50, NULL),
(1358, '17426129', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 50x5 mm (ROLO)', 'KG', 0.00, 'EUR', 1.12, 0.48, 0.48, '2020-06-17', '2020-06-19', 14, NULL, 5, 1, NULL, 3, NULL, 50, NULL, NULL, 5.00, NULL),
(1359, '17426130', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 70x3 mm (ROLO)', 'KG', 0.00, 'EUR', 1.12, 0.70, 0.70, '2024-05-16', '2024-05-17', 14, NULL, 5, 1, NULL, 3, NULL, 70, NULL, NULL, 3.00, NULL),
(1360, '17426131', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 80x3 mm (ROLO)', 'KG', 0.00, 'EUR', 1.12, 0.70, 0.70, '2024-05-16', '2024-05-17', 14, NULL, 5, 1, NULL, 3, NULL, 80, NULL, NULL, 3.00, NULL),
(1361, '17510030', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 60,5x3,00 (ROLO)', 'KG', 0.00, 'EUR', 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 60.5, NULL, NULL, 3.00, NULL),
(1362, '17510031', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 60,5x2,8mm (ROLO)', 'KG', 0.00, 'EUR', 1.14, 0.60, 0.60, '2019-04-04', '2019-04-08', 14, NULL, 5, 1, NULL, 3, NULL, 60.5, NULL, NULL, 2.80, NULL),
(1364, '17510033', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 60,5x2mm (ROLO)', 'KG', 0.00, 'EUR', 1.14, 0.57, 0.57, '2019-05-30', '2019-06-04', 14, NULL, 5, 1, NULL, 3, NULL, 60.5, NULL, NULL, 2.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1365, '17520925', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x2000x25mm', 'KG', 0.00, 'EUR', 0.98, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 2000, NULL, 25.00, NULL),
(1366, '17520935', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x2000x20mm', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 3000, 2000, NULL, 20.00, NULL),
(1367, '17520940', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2930x1500x17mm', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 2930, 1500, NULL, 17.00, NULL),
(1368, '17520980', 'CHAPA F/PRETA', 'CHAPA F/PRETA 300x300x10mm', 'KG', 0.00, 'EUR', 1.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 300, 300, NULL, 10.00, NULL),
(1376, '17520993', 'CHAPA F/PRETA', 'CHAPA F/PRETA 870x690x2mm', 'KG', 0.00, 'EUR', 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 870, 690, NULL, 2.00, NULL),
(1377, '17520994', 'CHAPA F/PRETA', 'CHAPA F/PRETA 1600x310x1,5mm', 'KG', 0.00, 'EUR', 1.26, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 1600, 310, NULL, 1.50, NULL),
(1378, '17520995', 'CHAPA F/PRETA', 'CHAPA F/PRETA 200x200x10mm', 'KG', 0.00, 'EUR', 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 200, 200, NULL, 10.00, NULL),
(1379, '17520996', 'CHAPA F/PRETA', 'CHAPA F/PRETA 350x300x16mm', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 350, 300, NULL, 16.00, NULL),
(1380, '17520997', 'CHAPA F/PRETA', 'CHAPA F/PRETA 450x350x18mm', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 450, 350, NULL, 18.00, NULL),
(1381, '17520998', 'CHAPA F/PRETA', 'CHAPA F/PRETA 160x660x18mm', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 160, 660, NULL, 18.00, NULL),
(1382, '17520999', 'CHAPA F/PRETA', 'CHAPA F/PRETA 400x300x20mm', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 400, 300, NULL, 20.00, NULL),
(1383, '17521000', 'CHAPA F/PRETA', 'CHAPA F/PRETA 12000x2500x10', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 12000, 2500, NULL, 10.00, NULL),
(1384, '17521001', 'CHAPA F/PRETA', 'CHAPA F/PRETA 650x500x15mm', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 650, 500, NULL, 15.00, NULL),
(1385, '17521002', 'CHAPA F/PRETA', 'CHAPA F/PRETA 650x500x20mm', 'KG', 0.00, 'EUR', 1.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 3, NULL, 650, 500, NULL, 20.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(1386, '17522000', 'CHAPA F/PRETA (ROLO)', 'CHAPA F/PRETA 30x2,50 MM (ROLO)', 'KG', 0.00, 'EUR', 1.15, 0.46, 0.46, '2021-04-09', '2021-04-13', 14, NULL, 5, 1, NULL, 3, NULL, NULL, NULL, NULL, 2.50, NULL),
(1642, '18315155', 'CHAPA PRE/PINT.PRETA', 'CHAPA PRE/PINT.PRETA 1250x0,50', 'KG', 0.00, 'EUR', 0.06, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, 3, NULL, 1250, NULL, NULL, 0.50, NULL),
(2396, '19921002', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x2,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, '1900-01-01', '1900-01-01', 18, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 2.00, NULL),
(2398, '19921003', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x3,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 3.00, NULL),
(2400, '19921004', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x4,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 4.00, NULL),
(2402, '19921005', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x5,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 5.00, NULL),
(2404, '19921006', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x6,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 6.00, NULL),
(2406, '19921008', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x8,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 8.00, NULL),
(2408, '19921010', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x10', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 10.00, NULL),
(2410, '19921012', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x12', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 12.00, NULL),
(2412, '19921015', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x1,50', 'KG', 0.00, 'EUR', 0.64, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 1.50, NULL),
(2414, '19921016', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x16', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 16.00, NULL),
(2416, '19921020', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2000x1000x20', 'KG', 0.00, 'EUR', 0.39, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2000, 1000, NULL, 20.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(2417, '19921252', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x2,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 2.00, NULL),
(2419, '19921253', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x3,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 3.00, NULL),
(2421, '19921254', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x4,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 4.00, NULL),
(2423, '19921255', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x5,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 5.00, NULL),
(2425, '19921256', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x6,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 6.00, NULL),
(2427, '19921258', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1250x8,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2500, 1250, NULL, 8.00, NULL),
(2429, '19921260', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1000x20', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1000, NULL, 20.00, NULL),
(2431, '19921261', 'CHAPA F/PRETA', 'CHAPA F/PRETA 2500x1500x5,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 2500, 1500, NULL, 5.00, NULL),
(2433, '19921301', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3010x1500x5,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3010, 1500, NULL, 5.00, NULL),
(2435, '19921302', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x2,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 2.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
(2437, '19921303', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x3,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 3.00, NULL),
(2439, '19921304', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x4,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 4.00, NULL),
(2441, '19921305', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x5,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 5.00, NULL),
(2443, '19921306', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x6,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 6.00, NULL),
(2445, '19921308', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x8,00', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 8.00, NULL),
(2447, '19921310', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x10', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 10.00, NULL),
(2449, '19921312', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x12', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 12.00, NULL),
(2451, '19921315', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x15', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 15.00, NULL),
(2453, '19921316', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x16', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 16.00, NULL),
(2455, '19921320', 'CHAPA F/PRETA', 'CHAPA F/PRETA 3000x1500x20', 'KG', 0.00, 'EUR', 0.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, 3, NULL, 3000, 1500, NULL, 20.00, NULL);










INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1155', '17120101', 'CHAPA DECAPADA', 'CH.DECAPADA 2000x1000x1,50', 'KG', 0.00, 'EUR', 1.32, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 2000, 1000, NULL, 1.50, NULL),
('1156', '17120102', 'CHAPA DECAPADA', 'CH.DECAPADA 2000x1000x2,00', 'KG', 0.00, 'EUR', 1.25, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 2000, 1000, NULL, 2.00, NULL),
('1157', '17120103', 'CHAPA DECAPADA', 'CH.DECAPADA 2000x1000x3,00', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 2000, 1000, NULL, 3.00, NULL),
('1158', '17120104', 'CHAPA DECAPADA', 'CH.DECAPADA 2000x1000x2,50', 'KG', 0.00, 'EUR', 1.90, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 2000, 1000, NULL, 2.50, NULL),
('1159', '17125122', 'CHAPA DECAPADA', 'CH.DECAPADA 2500x1250x2,00', 'KG', 0.00, 'EUR', 1.25, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 2500, 1250, NULL, 2.00, NULL),
('1160', '17125125', 'CHAPA DECAPADA', 'CH.DECAPADA 2500x1250x2,50', 'KG', 0.00, 'EUR', 1.25, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 2500, 1250, NULL, 2.50, NULL),
('1161', '17130151', 'CHAPA DECAPADA', 'CH.DECAPADA 3000x1500x1,50', 'KG', 5522.00, 'EUR', 1.32, 0.77, 0.77, 
 '2024-05-23', '2024-06-14', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 1.50, NULL),
('1162', '17130152', 'CHAPA DECAPADA', 'CH.DECAPADA 3000x1500x2,00', 'KG', 6781.00, 'EUR', 1.25, 0.75, 0.75, 
 '2024-05-23', '2024-06-14', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 2.00, NULL),
('1163', '17130153', 'CHAPA DECAPADA', 'CH.DECAPADA 3000x1500x3,00', 'KG', 3907.00, 'EUR', 1.18, 0.76, 0.77, 
 '2024-05-03', '2024-05-10', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 3.00, NULL),
('1164', '17130153', 'CHAPA DECAPADA', 'CH.DECAPADA 3000x1500x3,00', 'UN', 3907.00, 'EUR', -0.10, 0.76, 0.77, 
 '2024-05-03', '2024-05-10', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 3.00, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1165', '17130154', 'CHAPA DECAPADA', 'CH.DECAPADA 3000x1500x4,00', 'KG', 4628.00, 'EUR', 1.18, 0.77, 0.76, 
 '2023-10-20', '2024-06-12', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 4.00, NULL),
('1166', '17130155', 'CHAPA DECAPADA', 'CH.DECAPADA 3000x1500x5,00', 'KG', 1430.00, 'EUR', 1.18, 0.78, 0.75, 
 '2024-04-04', '2024-06-03', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 5.00, NULL),
('1167', '17130156', 'CHAPA DECAPADA', 'CH.DECAPADA 3000x1500x6,00', 'KG', 3200.00, 'EUR', 1.18, 0.73, 0.73, 
 '2024-06-07', '2024-05-10', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 6.00, NULL),
('1168', '17130158', 'CHAPA DECAPADA', 'CH.DECAPADA 3000x1500x8,00', 'KG', 2395.00, 'EUR', 1.18, 0.76, 0.73, 
 '2024-05-23', '2024-06-12', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 8.00, NULL),
('1169', '17130160', 'CHAPA DECAPADA', 'CH.DECAPADA 3000x1500x10,00', 'KG', 28.00, 'EUR', 1.25, 0.83, 0.83, 
 '2024-03-06', '2024-04-18', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 10.00, NULL),
('1170', '17130252', 'CHAPA DECAPADA', 'CH.DECAPADA 3000x1500x2,50', 'KG', 0.00, 'EUR', 1.25, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 2.50, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1171', '17190252', 'CHAPA DECAPADA', 'CHAPA DECAPADA 50x2,50 MM (ROLO)', 'KG', 0.00, 'EUR', 1.25, 0.84, 0.00, 
 '2022-10-19', '2022-11-21', 14, NULL, 5, 1, NULL, 1, NULL, 50, NULL, NULL, 2.50, NULL),
('1172', '17190262', 'CHAPA DECAPADA', 'CH.DECAPADA 60,5x3,00 MM (ROLO)', 'KG', 0.00, 'EUR', 1.18, 0.77, 0.77, 
 '2024-05-21', '2024-05-23', 14, NULL, 5, 1, NULL, 1, NULL, 60.5, NULL, NULL, 3.00, NULL),
('1363', '17510032', 'CHAPA DECAPADA', 'CHAPA DECAPADA 60,5x3mm (ROLO)', 'KG', 0.00, 'EUR', 1.18, 0.75, 0.75, 
 '2023-12-13', '2022-10-21', 14, NULL, 5, 1, NULL, 1, NULL, 60.5, NULL, NULL, 3.00, NULL),
('1469', '17920103', 'CHAPA DECAPADA', 'CHAPA DECAPADA 3000x1500x3,00', 'KG', 0.00, 'EUR', 1.90, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 3000, 1500, NULL, 3.00, NULL),
('1470', '17920125', 'CHAPA DECAPADA', 'CHAPA DECAPADA 2000x1000x2,50', 'KG', 0.00, 'EUR', 1.97, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 2000, 1000, NULL, 2.50, NULL),
('1471', '17920125', 'CHAPA DECAPADA', 'CHAPA DECAPADA 2000x1000x2,50', 'UN', 0.00, 'EUR', 1.10, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 2000, 1000, NULL, 2.50, NULL),
('1472', '17925104', 'CHAPA DECAPADA', 'CHAPA DECAPADA 2500x1250x4,00', 'KG', 0.00, 'EUR', 1.18, 0.61, 0.61, 
 '2017-10-10', '2017-12-21', 14, NULL, 5, 1, NULL, 1, NULL, 2500, 1250, NULL, 4.00, NULL),
('1473', '17925104', 'CHAPA DECAPADA', 'CHAPA DECAPADA 2500x1250x4,00', 'UN', 0.00, 'EUR', -0.10, 0.61, 0.61, 
 '2017-10-10', '2017-12-21', 14, NULL, 5, 1, NULL, 1, NULL, 2500, 1250, NULL, 4.00, NULL),
('1474', '17925105', 'CHAPA DECAPADA', 'CHAPA DECAPADA 2500x1250x5,00', 'KG', 0.00, 'EUR', 1.18, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 2500, 1250, NULL, 5.00, NULL),
('1475', '17925105', 'CHAPA DECAPADA', 'CHAPA DECAPADA 2500x1250x5,00', 'UN', 0.00, 'EUR', -0.10, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 1, NULL, 2500, 1250, NULL, 5.00, NULL);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('203', '117201010', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x1,00', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 1.00, NULL),
('1173', '17201000', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO CANELADA 0.8 mm', 'KG', 0.00, 'EUR', 1.09, 0.51, 0.51, 
 '2017-10-10', '2018-12-04', 14, NULL, 5, 1, NULL, NULL, 4, NULL, NULL, NULL, 0.80, NULL),
('1174', '17201002', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 54x1,5mm (ROLO)', 'KG', 0.00, 'EUR', 1.02, 0.82, 0.82, 
 '2024-03-19', '2024-03-20', 14, NULL, 5, 1, NULL, NULL, 4, 54, NULL, NULL, 1.50, NULL),
('1175', '17201003', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 30x2,50 MM (ROLO)', 'KG', 0.00, 'EUR', 0.95, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 30, NULL, NULL, 2.50, NULL),
('1176', '17201004', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x0,40', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 0.40, NULL),
('1177', '17201005', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x0,50', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 0.50, NULL),
('1178', '17201006', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x0,60', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 0.60, NULL),
('1179', '17201007', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x0,70', 'KG', 0.00, 'EUR', 1.23, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 0.70, NULL),
('1180', '17201008', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x0,80', 'KG', 2476.00, 'EUR', 1.23, 0.82, 0.84, 
 '2024-02-07', '2024-05-31', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 0.80, NULL),
('1181', '17201009', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x0,90', 'KG', 0.00, 'EUR', 1.23, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 0.90, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1182', '17201010', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x1,00', 'KG', 3920.00, 'EUR', 1.23, 0.86, 0.82, 
 '2024-02-14', '2024-06-05', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 1.00, NULL),
('1183', '17201012', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x1,25', 'KG', 1506.00, 'EUR', 1.23, 0.84, 0.84, 
 '2023-01-03', '2024-05-20', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 1.25, NULL),
('1184', '17201015', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x1,50', 'KG', 3142.00, 'EUR', 1.22, 1.03, 1.03, 
 '2023-07-06', '2024-05-17', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 1.50, NULL),
('1185', '17201020', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x2,00', 'KG', 6909.00, 'EUR', 1.22, 0.80, 0.78, 
 '2024-03-05', '2024-05-08', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 2.00, NULL),
('1186', '17201022', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 1800x1500x2 mm', 'KG', 0.00, 'EUR', 1.22, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 1800, 1500, NULL, 2.00, NULL),
('1187', '17201025', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x2,50', 'KG', 0.00, 'EUR', 1.22, 0.26, 0.26, 
 '2017-10-10', '2022-12-16', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 2.50, NULL),
('1188', '17201029', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2000x1000x3,00', 'KG', 0.00, 'EUR', 1.22, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2000, 1000, NULL, 3.00, NULL),
('1189', '17212510', 'CHAPA LAMINADA FRIO', 'CH.LAM. FRIO (ROLO) 1250x1,00', 'KG', 0.00, 'EUR', 1.23, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 1250, NULL, NULL, 1.00, NULL),
('1190', '17251204', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x0,40', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 0.40, NULL),
('1191', '17251205', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x0,50', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 0.50, NULL),
('1192', '17251206', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x0,60', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 0.60, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1193', '17251207', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x0,70', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 0.70, NULL),
('1194', '17251208', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x0,80', 'KG', 0.00, 'EUR', 1.23, 0.67, 0.67, 
 '2017-10-10', '2019-12-03', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 0.80, NULL),
('1195', '17251209', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x0,90', 'KG', 0.00, 'EUR', 1.23, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 0.90, NULL),
('1196', '17251210', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x1,00', 'KG', 0.00, 'EUR', 1.23, 0.57, 0.57, 
 '2017-10-10', '2017-12-21', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 1.00, NULL),
('1197', '17251212', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x1,25', 'KG', 0.00, 'EUR', 1.23, 0.60, 0.60, 
 '2017-10-10', '2023-09-11', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 1.25, NULL),
('1198', '17251215', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x1,50', 'KG', 0.00, 'EUR', 1.22, 0.58, 0.58, 
 '2017-10-10', '2023-09-11', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 1.50, NULL),
('1199', '17251220', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x2,00', 'KG', 0.00, 'EUR', 1.22, 0.89, 0.89, 
 '2021-02-17', '2021-02-18', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 2.00, NULL),
('1200', '17251225', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x2,50', 'KG', 0.00, 'EUR', 1.22, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 2.50, NULL),
('1201', '17251226', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1500x2,50', 'KG', 0.00, 'EUR', 1.22, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1500, NULL, 2.50, NULL),
('1202', '17251229', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 2500x1250x3,00', 'KG', 0.00, 'EUR', 1.22, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2500, 1250, NULL, 3.00, NULL),
('1203', '17301215', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1250x1,50', 'KG', 0.00, 'EUR', 1.22, 0.60, 0.60, 
 '2019-10-24', '2019-10-28', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1250, NULL, 1.50, NULL),
('1204', '17301505', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1500x0,50', 'KG', 0.00, 'EUR', 1.33, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1500, NULL, 0.50, NULL),
('1205', '17301506', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1500x0,60', 'KG', 0.00, 'EUR', 1.30, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1500, NULL, 0.60, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1206', '17301507', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1500x0,70', 'KG', 0.00, 'EUR', 1.23, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1500, NULL, 0.70, NULL),
('1207', '17301508', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1500x0,80', 'KG', 2987.00, 'EUR', 1.23, 0.78, 0.78, 
 '2024-04-18', '2024-05-31', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1500, NULL, 0.80, NULL),
('1208', '17301510', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1500x1,00', 'KG', 2901.00, 'EUR', 1.23, 0.79, 0.78, 
 '2024-04-18', '2024-06-05', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1500, NULL, 1.00, NULL),
('1209', '17301512', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1500x1,25', 'KG', 6137.00, 'EUR', 1.23, 0.83, 0.81, 
 '2024-03-06', '2024-06-03', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1500, NULL, 1.25, NULL),
('1210', '17301515', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1500x1,50', 'KG', 179.00, 'EUR', 1.22, 0.78, 0.78, 
 '2024-04-11', '2024-05-28', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1500, NULL, 1.50, NULL),
('1211', '17301520', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1500x2,00', 'KG', 2238.00, 'EUR', 1.22, 0.83, 0.81, 
 '2024-03-06', '2024-05-21', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1500, NULL, 2.00, NULL),
('1212', '17301525', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1500x2,50', 'KG', 0.00, 'EUR', 1.22, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1500, NULL, 2.50, NULL),
('1213', '17301529', 'CHAPA LAMINADA FRIO', 'CH.LAM.FRIO 3000x1500x3,00', 'KG', 5184.00, 'EUR', 1.22, 0.77, 0.77, 
 '2024-01-30', '2024-06-13', 14, NULL, 5, 1, NULL, NULL, 4, 3000, 1500, NULL, 3.00, NULL),
('1214', '17312003', 'CHAPA LAMINADA FRIO', 'CH. LAM.FRIO ST-12 2546x1500x3', 'KG', 0.00, 'EUR', 0.97, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 2546, 1500, NULL, 3.00, NULL),
('1740', '18500012', 'CHAPA LAMINADA FRIO PERFURADA', 'CHAPA LAM.A FRIO PERF.1018x868x1,0mm R7 U25', 'UN', 0.00, 'EUR', 7.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 4, 1018, 868, NULL, 1.00, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1373', '17520990', 'CHAPA COR-TEN', 'CHAPA COR-TEN 600x450x25', 'KG', 0.00, 'EUR', 1.72, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 600, 450, NULL, 25, NULL),
('1374', '17520991', 'CHAPA COR-TEN', 'CHAPA COR-TEN 1750x1500x1,75mm', 'KG', 0.00, 'EUR', 2.53, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 1750, 1500, NULL, 1.75, NULL),
('1375', '17520992', 'CHAPA COR-TEN', 'CHAPA COR-TEN 650x160x18 mm', 'KG', 0.00, 'EUR', 1.72, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 650, 160, NULL, 18, NULL),
('1387', '17530001', 'CHAPA COR-TEN', 'CHAPA COR-TEN 2000x1000x3,00', 'KG', 0.00, 'EUR', 1.72, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 2000, 1000, NULL, 3.00, NULL),
('1388', '17530002', 'CHAPA COR-TEN', 'CHAPA COR-TEN 2000x1000x1,50', 'KG', 0.00, 'EUR', 2.53, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 2000, 1000, NULL, 1.50, NULL),
('1389', '17530003', 'CHAPA COR-TEN', 'CHAPA COR-TEN 2500x1250x2,00', 'KG', 0.00, 'EUR', 2.40, 0.84, 0.84, 
 '2017-11-13', '2018-12-05', 14, NULL, 5, 1, NULL, 6, NULL, 2500, 1250, NULL, 2.00, NULL),
('1390', '17530004', 'CHAPA COR-TEN', 'CHAPA COR-TEN 2500x1250x1,70', 'KG', 0.00, 'EUR', 2.18, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 2500, 1250, NULL, 1.70, NULL),
('1391', '17530005', 'CHAPA COR-TEN', 'CHAPA COR-TEN 2000x1500x8,00', 'KG', 0.00, 'EUR', 1.72, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 2000, 1500, NULL, 8.00, NULL),
('1392', '17530006', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1250x2 mm', 'KG', 0.00, 'EUR', 2.40, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1250, NULL, 2.00, NULL),
('1393', '17530007', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1450x2,00', 'KG', 0.00, 'EUR', 2.40, 0.88, 0.88, 
 '2018-12-05', '2019-05-28', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1450, NULL, 2.00, NULL),
('1394', '17530008', 'CHAPA COR-TEN', 'CHAPA COR-TEN 2500x1250x3,00', 'KG', 0.00, 'EUR', 1.72, 0.86, 0.86, 
 '2018-12-18', '2018-12-20', 14, NULL, 5, 1, NULL, 6, NULL, 2500, 1250, NULL, 3.00, NULL),
('1395', '17530009', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x2,50', 'KG', 0.00, 'EUR', 1.72, 1.38, 1.38, 
 '2021-09-29', '2022-05-25', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 2.50, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1396', '17530010', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x3,00', 'KG', 7768.00, 'EUR', 1.72, 0.96, 0.95, 
 '2024-05-16', '2024-06-05', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 3.00, NULL),
('1397', '17530011', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x5,00', 'KG', 705.00, 'EUR', 1.72, 0.96, 0.95, 
 '2024-05-20', '2024-06-13', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 5.00, NULL),
('1398', '17530012', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x2,00', 'KG', 8440.00, 'EUR', 2.40, 0.97, 0.95, 
 '2024-05-17', '2024-06-14', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 2.00, NULL),
('1399', '17530013', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1000x3,00', 'KG', 0.00, 'EUR', 1.72, 0.98, 0.98, 
 '2018-09-12', '2018-09-12', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1000, NULL, 3.00, NULL),
('1400', '17530014', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3500x1500x1,50', 'KG', 0.00, 'EUR', 2.53, 1.16, 1.16, 
 '2019-06-07', '2019-06-07', 14, NULL, 5, 1, NULL, 6, NULL, 3500, 1500, NULL, 1.50, NULL),
('1401', '17530015', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x1,50', 'KG', 7830.00, 'EUR', 2.53, 1.27, 1.25, 
 '2024-04-22', '2024-06-11', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 1.50, NULL),
('1402', '17530016', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x2000x15', 'KG', 0.00, 'EUR', 1.72, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 2000, NULL, 15.00, NULL),
('1403', '17530017', 'CHAPA COR-TEN', 'CHAPA COR-TEN 2500x1250x1,50', 'KG', 0.00, 'EUR', 2.53, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 2500, 1250, NULL, 1.50, NULL),
('1404', '17530018', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x8,00', 'KG', 0.00, 'EUR', 1.72, 0.72, 0.72, 
 '2020-07-14', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 8.00, NULL),
('1405', '17530019', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1250x5,00', 'KG', 0.00, 'EUR', 1.72, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1250, NULL, 5.00, NULL),
('1406', '17530020', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x6,00', 'KG', 0.00, 'EUR', 1.72, 0.88, 0.88, 
 '2018-12-17', '2018-12-19', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 6.00, NULL),
('1407', '17530021', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x1,75', 'KG', 0.00, 'EUR', 2.18, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 1.75, NULL),
('1408', '17530022', 'CHAPA COR-TEN', 'CHAPA COR-TEN 6000x1500x3mm', 'KG', 0.00, 'EUR', 0.21, 0.87, 0.86, 
 '2019-03-20', '2019-03-21', 14, NULL, 5, 1, NULL, 6, NULL, 6000, 1500, NULL, 3.00, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1409', '17530023', 'CHAPA COR-TEN', 'CHAPA COR-TEN 4000x1500x8mm', 'KG', 0.00, 'EUR', 1.72, 0.84, 0.84, 
 '2018-02-15', '2018-02-15', 14, NULL, 5, 1, NULL, 6, NULL, 4000, 1500, NULL, 8.00, NULL),
('1410', '17560152', 'CHAPA COR-TEN', 'CHAPA COR-TEN 4000x1500x4mm', 'KG', 0.00, 'EUR', 1.72, 0.76, 0.76, 
 '2020-02-07', '2020-02-11', 14, NULL, 5, 1, NULL, 6, NULL, 4000, 1500, NULL, 4.00, NULL),
('1411', '17560153', 'CHAPA COR-TEN', 'CHAPA COR-TEN 4000x1500x5mm', 'KG', 0.00, 'EUR', 1.72, 0.88, 0.88, 
 '2018-12-20', '2018-12-19', 14, NULL, 5, 1, NULL, 6, NULL, 4000, 1500, NULL, 5.00, NULL),
('1412', '17560154', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x4,00', 'KG', 3436.00, 'EUR', 1.72, 1.00, 1.00, 
 '2023-11-17', '2024-06-12', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 4.00, NULL),
('1413', '17560155', 'CHAPA COR-TEN', 'CHAPA COR-TEN 2000x1000x2mm', 'KG', 0.00, 'EUR', 2.40, 0.84, 0.84, 
 '2017-11-13', '2018-12-05', 14, NULL, 5, 1, NULL, 6, NULL, 2000, 1000, NULL, 2.00, NULL),
('1414', '17560156', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x10mm', 'KG', 0.00, 'EUR', 1.72, 0.80, 0.79, 
 '2019-06-03', '2019-06-03', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 10.00, NULL),
('1415', '17560157', 'CHAPA COR-TEN', 'CHAPA COR-TEN 1500x7mm (ROLO)', 'KG', 0.00, 'EUR', 1.72, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, NULL, 1500, NULL, 7.00, NULL),
('1416', '17560158', 'CHAPA COR-TEN', 'CHAPA COR-TEN 4000x1500x6mm', 'KG', 0.00, 'EUR', 1.72, 0.88, 0.88, 
 '2018-12-17', '2018-12-19', 14, NULL, 5, 1, NULL, 6, NULL, 4000, 1500, NULL, 6.00, NULL),
('1417', '17560159', 'CHAPA COR-TEN', 'CHAPA COR-TEN 2700x1250x2 mm', 'KG', 0.00, 'EUR', 2.40, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 2700, 1250, NULL, 2.00, NULL),
('1418', '17560160', 'CHAPA COR-TEN', 'CHAPA COR-TEN 2500x1250x2.50mm', 'KG', 0.00, 'EUR', 1.72, 0.82, 0.82, 
 '2017-10-10', '2017-12-21', 14, NULL, 5, 1, NULL, 6, NULL, 2500, 1250, NULL, 2.50, NULL),
('1419', '17560161', 'CHAPA COR-TEN', 'CHAPA COR-TEN 3000x1500x12', 'KG', 0.00, 'EUR', 1.72, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 3000, 1500, NULL, 12.00, NULL),
('1421', '17561155', 'CHAPA COR-TEN', 'CHAPA COR-TEN 6000x1500x1,50', 'KG', 0.00, 'EUR', 2.53, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 6, NULL, 6000, 1500, NULL, 1.50, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1371', '17520983', 'CHAPA GALVANIZADA', 'CH. GALV. CANEL. 2500X1250X0,50', 'KG', 0.00, 'EUR', 1.53, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 2500, 1250, NULL, 0.50, NULL),
('1372', '17520984', 'CHAPA GALVANIZADA', 'CH. GALV. CANEL. 2000X900X0,50', 'KG', 0.00, 'EUR', 1.59, 0.00, 0.00, 
 '2017-10-10', '2018-12-05', 14, NULL, 5, 1, NULL, 2, NULL, 2000, 900, NULL, 0.50, NULL),
('1476', '18201000', 'CHAPA GALVANIZADA', 'CH.GAL.LISA (DANIFICADA)', 'KG', 0.00, 'EUR', 1.03, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, NULL, NULL, NULL, NULL, NULL),
('1477', '18201003', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 670x750x0,50', 'KG', 0.00, 'EUR', 0.04, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 670, 750, NULL, 0.50, NULL),
('1478', '18201004', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x0,40', 'KG', 1964.00, 'EUR', 1.73, 1.12, 1.12, 
 '2024-02-27', '2024-06-14', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 0.40, NULL),
('1479', '18201005', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x0,50', 'KG', 1962.00, 'EUR', 1.63, 1.01, 1.00, 
 '2024-01-16', '2024-06-13', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 0.50, NULL),
('1480', '18201006', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x0,63', 'KG', 4290.00, 'EUR', 1.57, 0.98, 0.97, 
 '2024-01-25', '2024-05-03', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 0.63, NULL),
('1481', '18201007', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x0,70', 'KG', 0.00, 'EUR', 1.57, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 0.70, NULL),
('1482', '18201008', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x0,80', 'KG', 2589.00, 'EUR', 1.52, 0.94, 0.94, 
 '2023-09-28', '2024-05-15', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 0.80, NULL),
('1483', '18201010', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x1,00', 'KG', 1520.00, 'EUR', 1.51, 0.91, 0.91, 
 '2023-10-10', '2024-06-05', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 1.00, NULL),
('1484', '18201012', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x1,25', 'KG', 3601.00, 'EUR', 1.50, 0.96, 0.89, 
 '2024-04-03', '2024-04-24', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 1.25, NULL),
('1485', '18201015', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x1,50', 'KG', 3109.00, 'EUR', 1.49, 0.86, 0.87, 
 '2024-04-03', '2024-05-29', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 1.50, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1486', '18201018', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x1,80', 'KG', 0.00, 'EUR', 1.54, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 1.80, NULL),
('1487', '18201020', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x2,00', 'KG', 2103.00, 'EUR', 1.47, 0.92, 0.92, 
 '2024-02-14', '2024-06-12', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 2.00, NULL),
('1488', '18201025', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x2,50', 'KG', 0.00, 'EUR', 1.47, 0.94, 0.94, 
 '2022-11-11', '2022-12-28', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 2.50, NULL),
('1489', '18201029', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1000x3,00', 'KG', 1011.00, 'EUR', 1.47, 0.92, 0.92, 
 '2024-02-27', '2024-06-04', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1000, NULL, 3.00, NULL),
('1490', '18201030', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1250x0,60', 'KG', 0.00, 'EUR', 1.57, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1250, NULL, 0.60, NULL),
('1491', '18201031', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1000x0,40', 'KG', 0.00, 'EUR', 1.73, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1000, NULL, 0.40, NULL),
('1492', '18201508', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2000x1500x0.80', 'KG', 0.00, 'EUR', 1.52, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2000, 1500, NULL, 0.80, NULL),
('1493', '18211212', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 6000x1000x1,25', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 6000, 1000, NULL, 1.25, NULL),
('1494', '18212215', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2050x1500x0,80', 'KG', 0.00, 'EUR', 1.52, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2050, 1500, NULL, 0.80, NULL),
('1495', '18231515', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2350x1500x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2350, 1500, NULL, 1.50, NULL),
('1496', '18246010', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1000x1,00', 'KG', 0.00, 'EUR', 1.51, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1000, NULL, 1.00, NULL),
('1497', '18251204', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x0,40', 'KG', 0.00, 'EUR', 1.73, 0.83, 0.83, 
 '2019-01-31', '2019-02-04', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 0.40, NULL),
('1498', '18251205', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x0,50', 'KG', 0.00, 'EUR', 1.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 0.50, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1499', '18251206', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x0,63', 'KG', 0.00, 'EUR', 1.57, 0.56, 0.56, 
 '2017-10-10', '2023-09-11', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 0.63, NULL),
('1500', '18251207', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x0,70', 'KG', 0.00, 'EUR', 1.52, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 0.70, NULL),
('1501', '18251208', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x0,80', 'KG', 0.00, 'EUR', 1.52, 0.58, 0.58, 
 '2017-10-27', '2017-10-27', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 0.80, NULL),
('1502', '18251210', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x1,00', 'KG', 1720.00, 'EUR', 1.51, 0.94, 0.94, 
 '2024-02-14', '2024-06-07', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 1.00, NULL),
('1503', '18251212', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x1,25', 'KG', 1516.00, 'EUR', 1.50, 0.89, 0.87, 
 '2024-04-11', '2024-06-12', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 1.25, NULL),
('1504', '18251215', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x1,50', 'KG', 1742.00, 'EUR', 1.49, 0.91, 0.91, 
 '2024-02-29', '2024-06-11', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 1.50, NULL),
('1505', '18251220', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x2,00', 'KG', 4670.00, 'EUR', 1.47, 0.87, 0.92, 
 '2024-02-08', '2024-06-05', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 2.00, NULL),
('1506', '18251225', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x2,50', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, 
 NULL, NULL, 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 2.50, NULL),
('1507', '18251229', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1250x3,00', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1250, NULL, 3.00, NULL),
('1508', '18251230', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2515x1000x1.25', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2515, 1000, NULL, 1.25, NULL),
('1509', '18254121', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2548x1250x1,25', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2548, 1250, NULL, 1.25, NULL),
('1510', '18254215', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2550x1250x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2550, 1250, NULL, 1.50, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1511', '18271206', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2750x1250x0,60', 'KG', 0.00, 'EUR', 1.57, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2750, 1250, NULL, 0.60, NULL),
('1512', '18271515', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2700x1500x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2700, 1500, NULL, 1.50, NULL),
('1513', '18271515', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2700x1500x1,50', 'UN', 0.00, 'EUR', 0.99, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2700, 1500, NULL, 1.50, NULL),
('1514', '18281215', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2826x1250x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2826, 1250, NULL, 1.50, NULL),
('1515', '18301012', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1000x1,25', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1000, NULL, 1.25, NULL),
('1516', '18301015', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1000x1,00', 'KG', 0.00, 'EUR', 1.51, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1000, NULL, 1.00, NULL),
('1517', '18301020', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1000x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1000, NULL, 1.50, NULL),
('1518', '18301205', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 2500x1000x0,50', 'KG', 0.00, 'EUR', 1.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 2500, 1000, NULL, 0.50, NULL),
('1519', '18301206', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1250x0,60', 'KG', 0.00, 'EUR', 1.57, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1250, NULL, 0.60, NULL),
('1520', '18301210', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1250x2,00', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1250, NULL, 2.00, NULL),
('1521', '18301215', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3650x1250x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00,
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3650, 1250, NULL, 1.50, NULL),
('1522', '18301505', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1500x0,50', 'KG', 3965.00, 'EUR', 1.63, 0.95, 0.91, 
 '2024-05-15', '2024-05-13', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1500, NULL, 0.50, NULL),
('1523', '18301506', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1500x0,63', 'KG', 8675.00, 'EUR', 1.57, 0.90, 0.89, 
 '2024-05-15', '2024-05-15', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1500, NULL, 0.63, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1524', '18301507', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1500x0,70', 'KG', 0.00, 'EUR', 1.52, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1500, NULL, 0.70, NULL),
('1525', '18301508', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1500x0,80', 'KG', 3195.00, 'EUR', 1.52, 0.88, 0.88, 
 '2024-05-24', '2024-06-13', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1500, NULL, 0.80, NULL),
('1526', '18301510', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1500x1,00', 'KG', 6504.00, 'EUR', 1.51, 0.91, 0.90, 
 '2024-01-16', '2024-06-06', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1500, NULL, 1.00, NULL),
('1527', '18301512', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1500x1,25', 'KG', 2739.00, 'EUR', 1.50, 0.84, 0.84, 
 '2024-05-31', '2024-06-11', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1500, NULL, 1.25, NULL),
('1528', '18301515', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1500x1,50', 'KG', 9524.03, 'EUR', 1.49, 0.85, 0.84, 
 '2024-05-27', '2024-06-14', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1500, NULL, 1.50, NULL),
('1529', '18301518', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1000x0,80', 'KG', 0.00, 'EUR', 1.52, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1000, NULL, 0.80, NULL),
('1530', '18301519', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3140x1000x0,80', 'KG', 0.00, 'EUR', 1.52, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3140, 1000, NULL, 0.80, NULL),
('1531', '18301520', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1500x2,00', 'KG', 12133.00, 'EUR', 1.47, 0.85, 0.84, 
 '2024-05-27', '2024-06-13', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1500, NULL, 2.00, NULL),
('1532', '18301525', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1500x2,50', 'KG', 0.00, 'EUR', 1.47, 0.60, 0.60, 
 '2021-01-20', '2021-01-20', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1500, NULL, 2.50, NULL),
('1533', '18301529', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3000x1500x3,00', 'KG', 2817.00, 'EUR', 1.47, 0.87, 0.89, 
 '2024-04-11', '2024-06-11', 14, NULL, 5, 1, NULL, 2, 1, 3000, 1500, NULL, 3.00, NULL),
('1534', '18301530', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 1450x1250x2,00', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 1450, 1250, NULL, 2.00, NULL),
('1535', '18301531', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3150x1500x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3150, 1500, NULL, 1.50, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1536', '18301532', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3350x1500x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3350, 1500, NULL, 1.50, NULL),
('1537', '18301533', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3400x1500x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3400, 1500, NULL, 1.50, NULL),
('1538', '18301534', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 3600x1500x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 3600, 1500, NULL, 1.50, NULL),
('1539', '18301535', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 6000x1500x1,25', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 6000, 1500, NULL, 1.25, NULL),
('1540', '18301538', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 4000x1500x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 4000, 1500, NULL, 1.50, NULL),
('1541', '18301540', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 4000x1500x3.00', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 4000, 1500, NULL, 3.00, NULL),
('1542', '18301550', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 5000x1500x2,00', 'KG', 0.00, 'EUR', 1.47, 1.40, 1.40, 
 '2021-11-04', '2021-11-09', 14, NULL, 5, 1, NULL, 2, 1, 5000, 1500, NULL, 2.00, NULL),
('1543', '18301608', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 1450x1250x2,00', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 1450, 1250, NULL, 2.00, NULL),
('1544', '18301610', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 6000x1500x2,00', 'KG', 0.00, 'EUR', 1.47, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, 6000, 1500, NULL, 2.00, NULL),
('1545', '18301611', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 6000x1500x1,00', 'KG', 0.00, 'EUR', 1.51, 0.77, 0.77, 
 '2018-12-17', '2018-12-19', 14, NULL, 5, 1, NULL, 2, 1, 6000, 1500, NULL, 1.00, NULL),
('1546', '18301615', 'CHAPA GALVANIZADA', 'CH.GAL.LISA 6000x1500x1,50', 'KG', 0.00, 'EUR', 1.49, 0.74, 0.74, 
 '2018-12-17', '2018-12-19', 14, NULL, 5, 1, NULL, 2, 1, 6000, 1500, NULL, 1.50, NULL),
('1547', '18301620', 'CHAPA GALVANIZADA', 'CHAPA GALV. 6000x1500x2,00mm', 'KG', 0.00, 'EUR', 2.16, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 6000, 1500, NULL, 2.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1548', '18310006', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.836x0,60', 'KG', 0.00, 'EUR', 1.60, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1549', '18311515', 'CHAPA GALVANIZADA LISA', 'CH.GAL.LISA 3150x1500x1,50', 'KG', 0.00, 'EUR', 1.49, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 1, NULL, NULL, NULL, NULL, NULL),
('1550', '18311530', 'CHAPA GALVANIZADA LISA', 'CH.GAL.LISA 6000x1500x3,00', 'KG', 0.00, 'EUR', 1.47, 0.96, 0.96, 
 '2022-09-07', '2022-09-19', 14, NULL, 5, 1, NULL, 2, 1, NULL, NULL, NULL, NULL, NULL),
('1551', '18312004', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.2000x0836x0,40', 'KG', 2664.00, 'EUR', 1.75, 1.08, 1.08, 
 '2024-05-02', '2024-06-14', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1552', '18312005', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.2000x0836x0,50', 'KG', 0.00, 'EUR', 1.65, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1553', '18312006', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.2200x0836x0,60', 'KG', 0.00, 'EUR', 1.60, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1554', '18312504', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.2500x0836x0,40', 'KG', 2058.00, 'EUR', 1.75, 1.08, 1.08, 
 '2024-05-02', '2024-06-07', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1555', '18312505', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.2500x0836x0,50', 'KG', 0.00, 'EUR', 1.65, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1556', '18312506', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.2500x0836x0,60', 'KG', 0.00, 'EUR', 1.60, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1557', '18313004', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.3000x0836x0,40', 'KG', 3486.00, 'EUR', 1.75, 1.10, 1.08, 
 '2024-05-02', '2024-06-11', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1558', '18313005', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.3000x0836x0,50', 'KG', 0.00, 'EUR', 1.65, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1559', '18313006', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.3000x0836x0,60', 'KG', 0.00, 'EUR', 1.60, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1560', '18313504', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.3500x0836x0,40', 'KG', 2192.00, 'EUR', 1.75, 1.10, 1.10, 
 '2023-11-09', '2024-06-13', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL),
('1561', '18313505', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.3500x0836x0,50', 'KG', 0.00, 'EUR', 1.65, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, NULL, NULL, NULL, NULL, NULL);


UPDATE t_product_catalog 
SET 
    length = CASE id
        WHEN '1548' THEN 836
        WHEN '1549' THEN 3150
        WHEN '1550' THEN 6000
        WHEN '1551' THEN 2000
        WHEN '1552' THEN 2000
        WHEN '1553' THEN 2200
        WHEN '1554' THEN 2500
        WHEN '1555' THEN 2500
        WHEN '1556' THEN 2500
        WHEN '1557' THEN 3000
        WHEN '1558' THEN 3000
        WHEN '1559' THEN 3000
        WHEN '1560' THEN 3500
        WHEN '1561' THEN 3500
        ELSE length
    END,
    width = CASE id
        WHEN '1548' THEN NULL
        WHEN '1549' THEN 1500
        WHEN '1550' THEN 1500
        WHEN '1551' THEN 836
        WHEN '1552' THEN 836
        WHEN '1553' THEN 836
        WHEN '1554' THEN 836
        WHEN '1555' THEN 836
        WHEN '1556' THEN 836
        WHEN '1557' THEN 836
        WHEN '1558' THEN 836
        WHEN '1559' THEN 836
        WHEN '1560' THEN 836
        WHEN '1561' THEN 836
        ELSE width
    END,
    thickness = CASE id
        WHEN '1548' THEN 0.60
        WHEN '1549' THEN 1.50
        WHEN '1550' THEN 3.00
        WHEN '1551' THEN 0.40
        WHEN '1552' THEN 0.50
        WHEN '1553' THEN 0.60
        WHEN '1554' THEN 0.40
        WHEN '1555' THEN 0.50
        WHEN '1556' THEN 0.60
        WHEN '1557' THEN 0.40
        WHEN '1558' THEN 0.50
        WHEN '1559' THEN 0.60
        WHEN '1560' THEN 0.40
        WHEN '1561' THEN 0.50
        ELSE thickness
    END
WHERE
    id IN ('1548' , '1549',
        '1550',
        '1551',
        '1552',
        '1553',
        '1554',
        '1555',
        '1556',
        '1557',
        '1558',
        '1559',
        '1560',
        '1561')


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1562', '18313506', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.3500x0836x0,60', 'KG', 0.00, 'EUR', 1.60, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, 3500, 836, NULL, 0.60, NULL),
('1563', '18314004', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.4000x0836x0,40', 'KG', 34.00, 'EUR', 1.75, 1.10, 1.10, 
 '2023-11-09', '2024-05-21', 14, NULL, 5, 1, NULL, 2, 3, 4000, 836, NULL, 0.40, NULL),
('1564', '18314005', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.4000x0836x0,50', 'KG', 0.00, 'EUR', 1.65, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, 4000, 836, NULL, 0.50, NULL),
('1565', '18314006', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.4300x0836x0,60', 'KG', 0.00, 'EUR', 1.60, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, 4300, 836, NULL, 0.60, NULL),
('1566', '18314504', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.4500x0836x0,40', 'KG', 0.00, 'EUR', 1.75, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, 4500, 836, NULL, 0.40, NULL),
('1567', '18314506', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.4500x0836x0,60', 'KG', 0.00, 'EUR', 1.60, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, 4500, 836, NULL, 0.60, NULL),
('1568', '18315004', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.5000x0836x0,40', 'KG', 0.00, 'EUR', 1.75, 0.78, 0.78, 
 '2020-01-24', '2021-01-06', 14, NULL, 5, 1, NULL, 2, 3, 5000, 836, NULL, 0.40, NULL),
('1569', '18315005', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.5000x0836x0,50', 'KG', 0.00, 'EUR', 1.65, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, 5000, 836, NULL, 0.50, NULL),
('1570', '18315006', 'CHAPA GALVANIZADA', 'CHAPA GALV. 1000x0,40 (ROLO)', 'KG', 0.00, 'EUR', 1.06, 1.10, 1.10, 
 '2023-01-19', '2020-12-07', 14, NULL, 5, 1, NULL, 2, NULL, 1000, NULL, NULL, 0.40, NULL),
('1571', '18315010', 'CHAPA GALVANIZADA', 'CHAPA GALV. 1250x0,60 (ROLO)', 'KG', 0.00, 'EUR', 0.64, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 1250, NULL, NULL, 0.60, NULL),
('1572', '18315011', 'CHAPA GALVANIZADA', 'CHAPA GALV. 1500x1,00 (ROLO)', 'KG', 0.00, 'EUR', 0.64, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 1500, NULL, NULL, 1.00, NULL),
('1573', '18315012', 'CHAPA GALVANIZADA', 'CHAPA GALV. 1500x1,25 (ROLO)', 'KG', 0.00, 'EUR', 0.64, 0.72, 0.71, 
 '2019-06-14', '2019-06-17', 14, NULL, 5, 1, NULL, 2, NULL, 1500, NULL, NULL, 1.25, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1574', '18315055', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.7500x0836x0,40', 'KG', 0.00, 'EUR', 1.75, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, 7500, 836, NULL, 0.40, NULL),
('1575', '18315056', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND.5500x0836x0,60', 'KG', 0.00, 'EUR', 1.60, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, 5500, 836, NULL, 0.60, NULL),
('1576', '18315089', 'CHAPA GALVANIZADA', 'CHAPA GALV. 390x1,5mm (ROLO)', 'KG', 0.00, 'EUR', 0.94, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 390, NULL, NULL, 1.50, NULL),
('1577', '18315090', 'CHAPA GALVANIZADA', 'CHAPA GALV. 100x2,0mm (ROLO)', 'KG', 0.00, 'EUR', 0.76, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 100, NULL, NULL, 2.00, NULL),
('1578', '18315091', 'CHAPA GALVANIZADA', 'CHAPA GALV. 130x2mm (ROLO)', 'KG', 0.00, 'EUR', 0.76, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 130, NULL, NULL, 2.00, NULL),
('1579', '18315092', 'CHAPA GALVANIZADA', 'CHAPA GALV. 115x0,50 mm (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 115, NULL, NULL, 0.50, NULL),
('1580', '18315093', 'CHAPA GALVANIZADA', 'CHAPA GALV. 137x0,60 (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.99, 0.99, 
 '2023-02-07', '2023-06-13', 14, NULL, 5, 1, NULL, 2, NULL, 137, NULL, NULL, 0.60, NULL),
('1581', '18315094', 'CHAPA GALVANIZADA', 'CHAPA GALV. 137x0,80 (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.96, 0.96, 
 '2023-02-17', '2023-07-17', 14, NULL, 5, 1, NULL, 2, NULL, 137, NULL, NULL, 0.80, NULL),
('1582', '18315095', 'CHAPA GALVANIZADA', 'CHAPA GALV. 150x0,80 (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 150, NULL, NULL, 0.80, NULL),
('1583', '18315096', 'CHAPA GALVANIZADA', 'CHAPA GALV. 180x0,80 (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 180, NULL, NULL, 0.80, NULL),
('1584', '18315097', 'CHAPA GALVANIZADA', 'CHAPA GALV.1500x2,50 (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 1500, NULL, NULL, 2.50, NULL),
('1585', '18315097', 'CHAPA GALVANIZADA', 'CHAPA GALV.1500x2,50 (ROLO)', 'UN', 0.00, 'EUR', 0.13, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 1500, NULL, NULL, 2.50, NULL),
('1586', '18315098', 'CHAPA GALVANIZADA', 'CHAPA GALV.1000x0,50 (ROLO)', 'KG', 0.00, 'EUR', 0.78, 1.38, 1.38, 
 '2021-11-26', '2021-11-29', 14, NULL, 5, 1, NULL, 2, NULL, 1000, NULL, NULL, 0.50, NULL),
('1587', '18315099', 'CHAPA GALVANIZADA', 'CHAPA GALV. 120x0,80 (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 120, NULL, NULL, 0.80, NULL),
('1588', '18315100', 'CHAPA GALVANIZADA', 'CHAPA GALV. 250x2,00 (ROLO)', 'KG', 0.00, 'EUR', 0.64, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 250, NULL, NULL, 2.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1589', '18315101', 'CHAPA GALVANIZADA', 'CHAPA GALV. 1272x0,75 (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 1272, NULL, NULL, 0.75, NULL),
('1590', '18315102', 'CHAPA GALVANIZADA', 'CHAPA GALV. 1250x1,50 (ROLO)', 'KG', 0.00, 'EUR', 0.65, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 1250, NULL, NULL, 1.50, NULL),
('1591', '18315103', 'CHAPA GALVANIZADA', 'CHAPA GALV. 1250x0,50 (ROLO)', 'KG', 0.00, 'EUR', 0.81, 1.02, 1.02, 
 '2023-02-02', '2023-02-07', 14, NULL, 5, 1, NULL, 2, NULL, 1250, NULL, NULL, 0.50, NULL),
('1592', '18315104', 'CHAPA GALVANIZADA', 'CHAPA GALV.1,250x1,50 (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 1250, NULL, NULL, 1.50, NULL),
('1593', '18315105', 'CHAPA GALVANIZADA', 'CHAPA GALV.1250x0,80 (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.97, 0.97, 
 '2023-10-20', '2023-10-23', 14, NULL, 5, 1, NULL, 2, NULL, 1250, NULL, NULL, 0.80, NULL),
('1594', '18315106', 'CHAPA GALVANIZADA', 'CHAPA GALV.665x0,50 mm (ROLO)', 'KG', 0.00, 'EUR', 0.08, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 665, NULL, NULL, 0.50, NULL),
('1607', '18315119', 'CHAPA GALVANIZADA', 'CHAPA GALV.1500x0,60 (ROLO)', 'KG', 0.00, 'EUR', 1.52, 0.69, 0.69, 
 '2020-01-08', '2020-01-10', 14, NULL, 5, 1, NULL, 2, NULL, 1500, NULL, NULL, 0.60, NULL),
('1608', '18315120', 'CHAPA GALVANIZADA', 'CHAPA GALV.1500x0.80 (ROLO)', 'KG', 0.00, 'EUR', 1.47, 0.69, 0.69, 
 '2020-01-08', '2020-01-10', 14, NULL, 5, 1, NULL, 2, NULL, 1500, NULL, NULL, 0.80, NULL),
('1612', '18315124', 'CHAPA GALVANIZADA PERFURADA', 'CHAPA GALVAN.P-273  0,5mm', 'MT', 0.00, 'EUR', 0.10, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, 2, NULL, NULL, NULL, NULL, 0.50, NULL),
('1613', '18315124', 'CHAPA GALVANIZADA PERFURADA', 'CHAPA GALVAN.P-273  0,5mm', 'UN', 0.00, 'EUR', 0.10, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, 2, NULL,  NULL, NULL, NULL, 0.50, NULL),
('1620', '18315130', 'CHAPA GALVANIZADA CANELADA PERFURADA', 'CH.GALV.CANEL.PERF.1250x0,50', 'M2', 0.00, 'EUR', 5.39, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 1250, NULL, NULL, 0.50, NULL),
('1621', '18315131', 'CHAPA GALVANIZADA', 'CHAPA GALV. 137x0,50 (ROLO)', 'KG', 0.00, 'EUR', 1.58, 1.00, 1.00, 
 '2023-02-24', '2023-07-17', 14, NULL, 5, 1, NULL, 2, NULL, 137, NULL, NULL, 0.50, NULL),
('1622', '18315132', 'CHAPA GALVANIZADA', 'CHAPA GALV. 50x1,00 (ROLO)', 'KG', 0.00, 'EUR', 1.51, 0.92, 0.92, 
 '2024-04-05', '2024-04-09', 14, NULL, 5, 1, NULL, 2, NULL, 50, NULL, NULL, 1.00, NULL),
('1623', '18315133', 'CHAPA GALVANIZADA', 'CHAPA GALV. 50x1,20 (ROLO)', 'KG', 0.00, 'EUR', 1.50, 0.89, 0.89, 
 '2024-04-05', '2024-04-09', 14, NULL, 5, 1, NULL, 2, NULL, 50, NULL, NULL, 1.20, NULL),
('1626', '18315139', 'CHAPA GALVANIZADA CANELADA', 'CH. GALV. CANEL.  2000x0,50', 'KG', 0.00, 'EUR', 0.82, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 2000, NULL, NULL, 0.50, NULL),
('1627', '18315140', 'CHAPA GALVANIZADA CANELADA', 'CH. GALV. CANEL.  2500x0,50', 'KG', 0.00, 'EUR', 0.82, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL, 2500, NULL, NULL, 0.50, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1637', '18315150', 'CHAPA GALVANIZADA PERFIL COLABORANTE', 'CH. GALV. PERFIL COLABORANTE 1850x860x0,70mm', 'M2', 0.00, 'EUR', 0.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, 2, NULL, 1850, 860, NULL, 0.70, NULL),
('1646', '18315159', 'CHAPA GALVANIZADA', 'CHAPA GALV. 350x1,50mm', 'M2', 0.00, 'EUR', 1.44, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2,  NULL, 350, NULL, NULL, 1.50, NULL),
('1647', '18315160', 'CHAPA GALVANIZADA', 'CHAPA GALV. 300x2,00mm', 'M2', 0.00, 'EUR', 1.42, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2,  NULL, 300, NULL, NULL, 2.00, NULL),
('1653', '18315166', 'CHAPA GALVANIZADA', 'CHAPA GALV. 350x1,50 (ROLO)', 'KG', 0.00, 'EUR', 1.44, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2,  NULL, 350, NULL, NULL, 1.50, NULL),
('1654', '18315167', 'CHAPA GALVANIZADA', 'CHAPA GALV. 200x1,50 (ROLO)', 'KG', 0.00, 'EUR', 1.44, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, NULL,  200, NULL, NULL, 1.50, NULL),
('1658', '18316004', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND. 6000x0836x0,40', 'KG', 0.00, 'EUR', 1.70, 0.83, 0.83, 
 '2019-01-08', '2020-12-07', 14, NULL, 5, 1, NULL, 2, 3, 6000, 836, NULL, 0.40, NULL),
('1659', '18316005', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND. 6000x0836x0,50', 'KG', 0.00, 'EUR', 1.60, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, 6000, 836, NULL, 0.50, NULL),
('1660', '18317004', 'CHAPA GALVANIZADA ONDULADA', 'CH.GALV.OND. 7000x0836x0,40', 'KG', 0.00, 'EUR', 1.70, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2, 3, 7000, 836, NULL, 0.40, NULL),
('1727', '18420108', 'CHAPA PERFURADA GALVANIZADA', 'CHAPA PERF.GALV. 2000x1000x1,0 R5 T8', 'UN', 0.00, 'EUR', 19.20, 17.63, 17.63, 
 '2020-08-12', '2019-05-29', 14, NULL, 5, 1, NULL, 2,  NULL, 2000, 1000, NULL, 1.00, NULL),
('1732', '18500004', 'CHAPA PERFURADA GALVANIZADA', 'CHAPA GALV. PERF.2500x1250x1,5  R10T15', 'UN', 0.00, 'EUR', 16.58, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 2,  NULL, 2500, 1250, NULL, 1.50, NULL),
('1758', '18500030', 'CHAPA PERFURADA GALVANIZADA', 'CHAPA PERF.GALV. 3000x1500x1.0 R5T8', 'UN', 0.00, 'EUR', 41.20, 47.15, 47.15, 
 '2020-08-12', '1900-01-01', 14, NULL, 5, 1, NULL, 2,  NULL, 3000, 1500, NULL, 1.00, NULL),
('1760', '18515008', 'CHAPA PERFURADA GALVANIZADA', 'CH. PERF.GALVANIZADA 2000x1000x1.50  R5 T8', 'UN', 0.00, 'EUR', 26.00, 30.50, 30.50, 
 '2020-02-17', '2020-03-12', 14, NULL, 5, 1, NULL, 2,  NULL, 2000, 1000, NULL, 1.50, NULL),
('1765', '18521513', 'CHAPA PERFURADA GALVANIZADA', 'CH.PERF.GALV.2500X1250X1,00 R5T8', 'UN', 0.00, 'EUR', 27.00, 30.89, 30.89, 
 '2019-11-28', '2019-12-02', 14, NULL, 5, 1, NULL, 2,  NULL, 2500, 1250, NULL, 1.00, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1661', '18401200', 'CHAPA ZINCOR DANIFICADA', 'CHAPA ZINCOR (DANIFICADA)', 'KG', 0.00, 'EUR', 1.37, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL),
('1662', '18401215', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1250x1,50 (ROLO)', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, 6, 1250, NULL, NULL, 1.50, NULL),
('1663', '18401220', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1250x2,00 (ROLO)', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, 6, 1250, NULL, NULL, 2.00, NULL),
('1664', '18410000', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1500x1,50 (ROLO)', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, 6, 1500, NULL, NULL, 1.50, NULL),
('1665', '18410001', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1250x1000x1,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, 6, 1250, 1000, NULL, 1.50, NULL),
('1666', '18410002', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1250x1,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, 6, 2000, 1250, NULL, 1.50, NULL),
('1667', '18410003', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2550x1250x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, 6, 2550, 1250, NULL, 2.00, NULL),
('1668', '18410004', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1600x1250x3,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, 6, 1600, 1250, NULL, 3.00, NULL),
('1669', '18410008', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1000x0,80', 'KG', 2975.00, 'EUR', 1.52, 0.97, 0.98, 
 '2024-04-08', '2024-05-14', 14, NULL, 5, 1, NULL, 5, 6, 2000, 1000, NULL, 0.80, NULL),
('1670', '18410009', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2100x1000x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, 6, 2100, 1000, NULL, 2.00, NULL),
('1671', '18410010', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1000x1,00', 'KG', 779.00, 'EUR', 1.50, 0.94, 0.92, 
 '2023-12-18', '2024-06-05', 14, NULL, 5, 1, NULL, 5, 6, 2000, 1000, NULL, 1.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1661', '18401200', 'CHAPA ZINCOR DANIFICADA', 'CHAPA ZINCOR (DANIFICADA)', 'KG', 0.00, 'EUR', 1.37, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL),
('1662', '18401215', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1250x1,50 (ROLO)', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 1250, NULL, NULL, 1.50, NULL),
('1663', '18401220', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1250x2,00 (ROLO)', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 1250, NULL, NULL, 2.00, NULL),
('1664', '18410000', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1500x1,50 (ROLO)', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 1500, NULL, NULL, 1.50, NULL),
('1665', '18410001', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1250x1000x1,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 1250, 1000, NULL, 1.50, NULL),
('1666', '18410002', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1250x1,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 2000, 1250, NULL, 1.50, NULL),
('1667', '18410003', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2550x1250x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 2550, 1250, NULL, 2.00, NULL),
('1668', '18410004', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1600x1250x3,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 1600, 1250, NULL, 3.00, NULL),
('1669', '18410008', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1000x0,80', 'KG', 2975.00, 'EUR', 1.52, 0.97, 0.98, 
 '2024-04-08', '2024-05-14', 14, NULL, 5, 1, NULL, 5, NULL, 2000, 1000, NULL, 0.80, NULL),
('1670', '18410009', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2100x1000x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 2100, 1000, NULL, 2.00, NULL),
('1671', '18410010', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1000x1,00', 'KG', 779.00, 'EUR', 1.50, 0.94, 0.92, 
 '2023-12-18', '2024-06-05', 14, NULL, 5, 1, NULL, 5, NULL, 2000, 1000, NULL, 1.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1672', '18410012', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1000x1,25', 'KG', 1967.00, 'EUR', 1.50, 1.02, 0.92, 
 '2023-12-18', '2024-06-12', 14, NULL, 5, 1, NULL, 5, NULL, 2000, 1000, NULL, 1.25, NULL),
('1673', '18410015', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1000x1,50', 'KG', 264.00, 'EUR', 1.50, 0.93, 0.92, 
 '2023-12-18', '2024-06-12', 14, NULL, 5, 1, NULL, 5, NULL, 2000, 1000, NULL, 1.50, NULL),
('1674', '18410020', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1000x2,00', 'KG', 3940.00, 'EUR', 1.50, 0.95, 0.94, 
 '2024-04-03', '2024-06-07', 14, NULL, 5, 1, NULL, 5,NULL,  2000, 1000, NULL, 2.00, NULL),
('1675', '18410025', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1000x2,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,NULL,  2000, 1000, NULL, 2.50, NULL),
('1676', '18410030', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1000x3,00', 'KG', 392.00, 'EUR', 1.50, 0.96, 0.96, 
 '2023-09-14', '2024-06-05', 14, NULL, 5, 1, NULL, 5,NULL,  2000, 1000, NULL, 3.00, NULL),
('1677', '18410035', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1500x1,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 2000, 1500, NULL, 1.50, NULL),
('1678', '18411010', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2000x1250x1,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,NULL,  2000, 1250, NULL, 1.00, NULL),
('1679', '18411015', 'CHAPA ZINCOR', 'CHAPA ZINCOR 1930x1250x1,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 1930, 1250, NULL, 1.00, NULL),
('1680', '18411016', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2400x1250x1,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 2400, 1250, NULL, 1.00, NULL),
('1681', '18412508', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2500x1250x0,80', 'KG', 0.00, 'EUR', 1.52, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,NULL,  2500, 1250, NULL, 0.80, NULL),
('1682', '18412510', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2500x1250x1,00', 'KG', 2464.00, 'EUR', 1.50, 0.95, 0.92, 
 '2024-01-18', '2024-06-14', 14, NULL, 5, 1, NULL, 5, NULL, 2500, 1250, NULL, 1.00, NULL),
('1683', '18412512', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2500x1250x1,25', 'KG', 3301.00, 'EUR', 1.50, 0.94, 0.92, 
 '2024-01-24', '2024-06-14', 14, NULL, 5, 1, NULL, 5, NULL, 2500, 1250, NULL, 1.25, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1684', '18412515', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2500x1250x1,50', 'KG', 3233.00, 'EUR', 1.50, 0.94, 0.94, 
 '2024-05-24', '2024-06-11', 14, NULL, 5, 1, NULL, 5,  NULL,2500, 1250, NULL, 1.50, NULL),
('1685', '18412520', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2500x1250x2,00', 'KG', 2577.00, 'EUR', 1.50, 0.93, 0.92, 
 '2024-02-28', '2024-06-14', 14, NULL, 5, 1, NULL, 5, NULL, 2500, 1250, NULL, 2.00, NULL),
('1686', '18412525', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2500x1250x2,50', 'KG', 0.00, 'EUR', 1.50, 1.12, 1.12, 
 '2023-03-06', '2023-03-07', 14, NULL, 5, 1, NULL, 5, NULL, 2500, 1250, NULL, 2.50, NULL),
('1687', '18412530', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2500x1250x3,00', 'KG', 0.00, 'EUR', 1.50, 0.77, 0.77, 
 '2019-09-13', '2019-09-12', 14, NULL, 5, 1, NULL, 5, NULL, 2500, 1250, NULL, 3.00, NULL),
('1688', '18413026', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2600x1250x1,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL, 2600, 1250, NULL, 1.00, NULL),
('1689', '18414012', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3000x1000x1,25', 'KG', 0.00, 'EUR', 1.50, 1.60, 0.00, 
 '2022-10-20', '2022-10-20', 14, NULL, 5, 1, NULL, 5, NULL, 3000, 1000, NULL, 1.25, NULL),
('1690', '18414015', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3000x1000x1,50', 'KG', 0.00, 'EUR', 1.50, 0.94, 0.72, 
 '2020-02-05', '2024-01-05', 14, NULL, 5, 1, NULL, 5, NULL, 3000, 1000, NULL, 1.50, NULL),
('1691', '18414020', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3000x1000x2,00', 'KG', 0.00, 'EUR', 1.50, 0.80, 0.80, 
 '2017-11-17', '2019-06-21', 14, NULL, 5, 1, NULL, 5, NULL, 3000, 1000, NULL, 2.00, NULL),
('1692', '18415008', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3000x1500x0,80', 'KG', 2602.00, 'EUR', 1.52, 1.05, 1.05, 
 '2023-07-03', '2024-06-13', 14, NULL, 5, 1, NULL, 5, NULL, 3000, 1500, NULL, 0.80, NULL),
('1693', '18415010', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3000x1500x1,00', 'KG', 1557.00, 'EUR', 1.50, 0.95, 0.95, 
 '2024-05-15', '2024-06-14', 14, NULL, 5, 1, NULL, 5, NULL, 3000, 1500, NULL, 1.00, NULL),
('1694', '18415012', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3000x1500x1,25', 'KG', 3920.00, 'EUR', 1.50, 0.93, 0.92, 
 '2023-12-15', '2024-06-14', 14, NULL, 5, 1, NULL, 5, NULL, 3000, 1500, NULL, 1.25, NULL),
('1695', '18415015', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3000x1500x1,50', 'KG', 12350.02, 'EUR', 1.50, 0.94, 0.94, 
 '2024-05-24', '2024-06-14', 14, NULL, 5, 1, NULL, 5, NULL, 3000, 1500, NULL, 1.50, NULL),
('1696', '18415020', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3000x1500x2,00', 'KG', 1303.00, 'EUR', 1.50, 0.93, 0.94, 
 '2024-04-03', '2024-06-13', 14, NULL, 5, 1, NULL, 5, NULL, 3000, 1500, NULL, 2.00, NULL),
('1697', '18415025', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3000x1500x2,50', 'KG', 0.00, 'EUR', 1.50, 0.59, 0.59, 
 '2017-10-10', '2019-01-10', 14, NULL, 5, 1, NULL, 5, NULL, 3000, 1500, NULL, 2.50, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1698', '18415030', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3000x1500x3,00', 'KG', 5560.00, 'EUR', 1.50, 0.94, 0.95, 
 '2024-05-03', '2024-06-05', 14, NULL, 5, 1, NULL, 5, NULL,  3000, 1500, NULL, 3.00, NULL),
('1699', '18415031', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2800x1250x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  2800, 1250, NULL, 2.00, NULL),
('1700', '18415032', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3200x1500x1,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  3200, 1500, NULL, 1.50, NULL),
('1701', '18415033', 'CHAPA ZINCOR', 'CHAPA ZINCOR 4500x1500x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 4500, 1500, NULL, 2.00, NULL),
('1702', '18415034', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3765x1500x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  3765, 1500, NULL, 2.00, NULL),
('1703', '18415035', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3500x1500x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 3500, 1500, NULL, 2.00, NULL),
('1704', '18415036', 'CHAPA ZINCOR', 'CHAPA ZINCOR 4000x1250x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 4000, 1250, NULL, 2.00, NULL),
('1705', '18415037', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3500x1250x3,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  3500, 1250, NULL, 3.00, NULL),
('1706', '18415038', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3700x1250x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 3700, 1250, NULL, 2.00, NULL),
('1707', '18415039', 'CHAPA ZINCOR', 'CHAPA ZINCOR 3700x1500x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 3700, 1500, NULL, 2.00, NULL),
('1708', '18415040', 'CHAPA ZINCOR', 'CHAPA ZINCOR 4000x1000x1,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  4000, 1000, NULL, 1.50, NULL),
('1709', '18415041', 'CHAPA ZINCOR', 'CHAPA ZINCOR 4000x1000x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 4000, 1000, NULL, 2.00, NULL),
('1710', '18415042', 'CHAPA ZINCOR', 'CHAPA ZINCOR 4000x1500x1,25', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 4000, 1500, NULL, 1.25, NULL),
('1711', '18415045', 'CHAPA ZINCOR', 'CHAPA ZINCOR 2150x1500x1,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 2150, 1500, NULL, 1.50, NULL),
('1712', '18415046', 'CHAPA ZINCOR', 'CHAPA ZINCOR 4000x1500x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 4000, 1500, NULL, 2.00, NULL),
('1713', '18415050', 'CHAPA ZINCOR', 'CHAPA ZINCOR 4000x1500x3,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  4000, 1500, NULL, 3.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1714', '18415051', 'CHAPA ZINCOR', 'CHAPA ZINCOR 4000x1500x2,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  4000, 1500, NULL, 2.50, NULL),
('1715', '18415515', 'CHAPA ZINCOR', 'CHAPA ZINCOR 5000x1000x1,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  5000, 1000, NULL, 1.50, NULL),
('1716', '18415520', 'CHAPA ZINCOR', 'CHAPA ZINCOR 5000x1500x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  5000, 1500, NULL, 2.00, NULL),
('1717', '18415521', 'CHAPA ZINCOR', 'CHAPA ZINCOR 5500x1000x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 5500, 1000, NULL, 2.00, NULL),
('1718', '18415522', 'CHAPA ZINCOR', 'CHAPA ZINCOR 5410x1500x5,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  5410, 1500, NULL, 5.00, NULL),
('1720', '18416006', 'CHAPA ZINCOR', 'CHAPA ZINCOR 6000x1000x1,25', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  6000, 1000, NULL, 1.25, NULL),
('1721', '18416010', 'CHAPA ZINCOR', 'CHAPA ZINCOR 4000x1500x1,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 4000, 1500, NULL, 1.00, NULL),
('1722', '18416012', 'CHAPA ZINCOR', 'CHAPA ZINCOR 6000x1500x1,25', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 6000, 1500, NULL, 1.25, NULL),
('1723', '18416015', 'CHAPA ZINCOR', 'CHAPA ZINCOR 6000x1500x1,50', 'KG', 0.00, 'EUR', 1.50, 1.35, 1.35, 
 '2024-04-02', '2024-04-03', 14, NULL, 5, 1, NULL, 5, NULL,  6000, 1500, NULL, 1.50, NULL),
('1724', '18416020', 'CHAPA ZINCOR', 'CHAPA ZINCOR 6000x1500x2,00', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5,  NULL, 6000, 1500, NULL, 2.00, NULL),
('1725', '18416025', 'CHAPA ZINCOR', 'CHAPA ZINCOR 6000x1500x2,50', 'KG', 0.00, 'EUR', 1.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  6000, 1500, NULL, 2.50, NULL),
('1728', '18471215', 'CHAPA ZINCOR', 'CH. ZINCOR ROLO 1250x1,50', 'KG', 0.00, 'EUR', 0.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, 5, NULL,  NULL, 1250, NULL, 1.50, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1443', '17626205', 'CHAPA ALUMINIO GOTAS', 'CHAPA ALUMINIO GOTAS 2000x1000x4,00', 'KG', 0.00, 'EUR', 9.80, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL,NULL, 2000, 1000, NULL, 4.00, NULL),
('1444', '17626206', 'CH. ALUMINIO ANT.DERR.', 'CH. ALUMINIO ANT.DERR.3000x1500x2/3', 'KG', 0.00, 'EUR', 9.80, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL,NULL, 3000, 1500, NULL, NULL, NULL),
('1460', '17730125', 'CHAPA ALUMINIO.OND.', 'CHAPA ALUMINIO.OND. 3000x1270x0.56', 'KG', 0.00, 'EUR', 4.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, 3, NULL,3000, 1270, NULL, 0.56, NULL),
('1461', '17740125', 'CHAPA ALUMINIO.OND.', 'CHAPA ALUMINIO.OND. 4000x1270x0.56', 'KG', 0.00, 'EUR', 4.50, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, 3, NULL,4000, 1270, NULL, 0.56, NULL),
('1462', '17750125', 'CHAPA ALUMINIO OND.', 'CHAPA ALUMINIO OND. 5000x1270x0.56', 'KG', 0.00, 'EUR', 4.60, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, 3, NULL,5000, 1270, NULL, 0.56, NULL),
('1463', '17770125', 'CHAPA ALUMINIO OND.', 'CHAPA ALUMINIO OND. 7000x1270x0,50', 'KG', 0.00, 'EUR', 4.64, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, 3, NULL,7000, 1270, NULL, 0.50, NULL),
('1464', '17770126', 'CHAPA ALUMINIO "1050"', 'CHAPA ALUMINIO "1050" 3000x1500x2,00', 'KG', 0.00, 'EUR', 3.45, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL,NULL, 3000, 1500, NULL, 2.00, NULL),
('1466', '17770129', 'CHAPA ALUMINIO', 'CHAPA ALUMINIO  3000x1500x1,5mm', 'KG', 0.00, 'EUR', 9.80, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL,NULL, 3000, 1500, NULL, 1.50, NULL),
('1766', '18521600', 'CH. ALUMINIO MEIO DURO', 'CH. ALUMINIO MEIO DURO 2500x1250x1,5', 'KG', 0.00, 'EUR', -4.24, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL, NULL,2500, 1250, NULL, 1.50, NULL),
('1767', '18521601', 'CH. ALUMINIO MEIO DURO', 'CH. ALUMINIO MEIO DURO 3000x800x1,5', 'KG', 0.00, 'EUR', -4.24, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL,NULL, 3000, 800, NULL, 1.50, NULL),
('1768', '18521602', 'CH. ALUMINIO MEIO DURO', 'CH. ALUMINIO MEIO DURO 3000x1500x1,5', 'KG', 0.00, 'EUR', -4.24, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL,NULL, 3000, 1500, NULL, 1.50, NULL),
('1770', '18525011', 'CHAPA ALUMINIO', 'CHAPA ALUMINIO 4000x1500x2mm', 'KG', 0.00, 'EUR', 0.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL,NULL, 4000, 1500, NULL, 2.00, NULL);


UPDATE t_product_catalog 
SET 
    surface_id = CASE
        WHEN product_code = '17626205' THEN '11'
        WHEN product_code = '17626206' THEN NULL
        WHEN product_code = '17730125' THEN '3'
        WHEN product_code = '17740125' THEN '3'
        WHEN product_code = '17750125' THEN '3'
        WHEN product_code = '17770125' THEN '3'
        WHEN product_code = '17770126' THEN NULL
        WHEN product_code = '17770129' THEN NULL
        WHEN product_code = '18521600' THEN NULL
        WHEN product_code = '18521601' THEN NULL
        WHEN product_code = '18521602' THEN NULL
        WHEN product_code = '18525011' THEN NULL
    END,
    finishing_id = NULL
WHERE
    product_code IN ('17626205' , '17626206',
        '17730125',
        '17740125',
        '17750125',
        '17770125',
        '17770126',
        '17770129',
        '18521600',
        '18521601',
        '18521602',
        '18525011')

INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1771', '18530121', 'CH. ALUMINIO PERF. F3', 'CH. ALUMINIO PERF. F3 - 3000x1250x1,5mm', 'KG', 0.00, 'EUR', 8.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL, NULL, 3000, 1250, NULL, 1.50, NULL),
('1772', '18530121', 'CH. ALUMINIO PERF. F3', 'CH. ALUMINIO PERF. F3 - 3000x1250x1,5mm', 'UN', 0.00, 'EUR', 8.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL, NULL, 3000, 1250, NULL, 1.50, NULL),
('1773', '18530122', 'CH. ALUMINIO PERF. F3', 'CH. ALUMINIO PERF. F3 - 3000x1250x2,0mm', 'UN', 0.00, 'EUR', 8.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL, NULL, 3000, 1250, NULL, 2.00, NULL),
('1774', '18530321', 'CH. ALUMINIO PERF. F8', 'CH. ALUMINIO PERF. F8 - 3000x1250x1,5mm', 'KG', 0.00, 'EUR', 8.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL, NULL, 3000, 1250, NULL, 1.50, NULL),
('1775', '18530321', 'CH. ALUMINIO PERF. F8', 'CH. ALUMINIO PERF. F8 - 3000x1250x1,5mm', 'UN', 0.00, 'EUR', 8.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 3, NULL, NULL, NULL, 3000, 1250, NULL, 1.50, NULL),
('2123', '19658004', 'BASES EM CHAPA AÇO', 'BASES EM CHAPA AÇO 60X40 VERDES', 'UN', 156.00, 'EUR', 9.40, 4.90, 4.90, 
 '2023-11-21', '2024-06-14', 15, NULL, 5, 2, NULL, 8, NULL, 60, 40, NULL, NULL, NULL),
('2124', '19658005', 'BASES EM CHAPA AÇO', 'BASES EM CHAPA AÇO 60X40 BRANCO', 'UN', 268.00, 'EUR', 9.40, 4.84, 4.84, 
 '2023-07-13', '2024-06-07', 15, NULL, 5, 2, NULL, 9, NULL, 60, 40, NULL, NULL, NULL),
('2127', '19658008', 'BASE EM CHAPA AÇO', 'BASE EM CHAPA AÇO 60x60 CINZA', 'UN', 0.00, 'EUR', 0.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 2, NULL, 10, NULL, 60, 60, NULL, NULL, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('3558', '23620008', 'CHAPA INOX 304', 'CHAPA INOX 304 2000x1000x0,80', 'KG', 0.00, 'EUR', 3.35, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, NULL, 2000, 1000, NULL, 0.80, NULL),
('3559', '23620009', 'CHAPA INOX 304', 'CHAPA INOX 304 3000x1500x0,80', 'KG', 0.00, 'EUR', 2.44, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, NULL, 3000, 1500, NULL, 0.80, NULL),
('3560', '23620010', 'CHAPA INOX 304', 'CHAPA INOX 304 2000x1000x1,00', 'KG', 0.00, 'EUR', 4.90, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, NULL, 2000, 1000, NULL, 1.00, NULL),
('3561', '23620011', 'CHAPA INOX 304', 'CHAPA INOX 304 3000x1500x1,00', 'KG', 0.00, 'EUR', 2.42, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, NULL, 3000, 1500, NULL, 1.00, NULL),
('3562', '23620020', 'CHAPA INOX 304', 'CHAPA INOX 304 2000x1000x2,00', 'KG', 0.00, 'EUR', 2.37, 2.20, 2.20, 
 '2020-04-27', '2020-04-28', 62, NULL, 5, 4, NULL, NULL, NULL, 2000, 1000, NULL, 2.00, NULL),
('3563', '23620030', 'CHAPA INOX 304', 'CHAPA INOX 304 2000x1000x3,00', 'KG', 0.00, 'EUR', 4.83, 2.15, 2.15, 
 '2020-04-27', '2020-04-28', 62, NULL, 5, 4, NULL, NULL, NULL, 2000, 1000, NULL, 3.00, NULL),
('3564', '23620031', 'CHAPA INOX 304', 'CHAPA INOX 304 2470x1500x0.80', 'KG', 0.00, 'EUR', 5.14, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, NULL, 2470, 1500, NULL, 0.80, NULL),
('3565', '23620032', 'CHAPA INOX 304', 'CHAPA INOX 304 3170x1500x0.80', 'KG', 0.00, 'EUR', 4.98, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, NULL, 3170, 1500, NULL, 0.80, NULL),
('3566', '23625015', 'CHAPA INOX 304', 'CHAPA INOX 304 2500x1250x1,50', 'KG', 0.00, 'EUR', 22.51, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, NULL, 2500, 1250, NULL, 1.50, NULL),
('3567', '23625020', 'CHAPA INOX 304', 'CHAPA INOX304 2500x1250x2mm', 'KG', 0.00, 'EUR', 4.94, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, NULL, 2500, 1250, NULL, 2.00, NULL),
('3569', '23630015', 'CHAPA INOX 304', 'CHAPA INOX  304  3000x1500x2,0', 'KG', 0.00, 'EUR', 4.24, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, NULL, 3000, 1500, NULL, 2.00, NULL),
('3570', '23630412', 'CHAPA INOX 304', 'CHAPA INOX  304  3000x1500x1,2', 'KG', 0.00, 'EUR', 3.45, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, NULL, 3000, 1500, NULL, 1.20, NULL),
('3571', '23630415', 'CHAPA INOX 304', 'CHAPA INOX  304  3000x1500x1,50', 'KG', 0.00, 'EUR', 5.22, 2.25, 2.25, 
 '2020-04-27', '2020-04-28', 62, NULL, 5, 4, NULL, NULL, NULL, 3000, 1500, NULL, 1.50, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('3572', '23631001', 'CH. INOX 304 GOTAS', 'CH. INOX 304 GOTAS 3000x1250x3/5', 'KG', 0.00, 'EUR', 4.42, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, 11, 3000, 1250, NULL, NULL, NULL),
('3573', '23643015', 'CHAPA INOX 304', 'CHAPA INOX 304   3000x1500x1,5', 'KG', 0.00, 'EUR', 4.64, 2.25, 0.00, 
 '2020-04-28', '2020-04-28', 62, NULL, 5, 4, NULL, NULL, NULL, 3000, 1500, NULL, 1.50, NULL),
('12168', '70800215', 'ADAPTADOR CHAPA INOX DIR.', 'ADAPTADOR CHAPA INOX DIR.', 'UN', 4.00, 'EUR', 17.81, 7.62, 7.62, 
 '2017-10-10', '1900-01-01', 52, NULL, 5, 4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
('12169', '70800215', 'ADAPTADOR CHAPA INOX DIR.', 'ADAPTADOR CHAPA INOX DIR.', 'UN', 4.00, 'PTE', 2807.00, 7.62, 7.62, 
 '2017-10-10', '1900-01-01', 52, NULL, 5, 4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
('12170', '70800216', 'ADAPTADOR CHAPA INOX ESQ.', 'ADAPTADOR CHAPA INOX ESQ.', 'UN', 6.00, 'EUR', 17.81, 7.62, 7.62, 
 '2017-10-10', '1900-01-01', 52, NULL, 5, 4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
('12171', '70800216', 'ADAPTADOR CHAPA INOX ESQ.', 'ADAPTADOR CHAPA INOX ESQ.', 'UN', 6.00, 'PTE', 2807.00, 7.62, 7.62, 
 '2017-10-10', '1900-01-01', 52, NULL, 5, 4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
('13373', '76800010', 'CHAPA PLAST. CANELADA', 'CHAPA PLAST. CANELADA', 'M2', 0.00, 'EUR', 0.00, 4.24, 4.24, 
 '2017-10-10', '2018-12-28', 62, NULL, 5, 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
('13374', '76800010', 'CHAPA PLAST. CANELADA', 'CHAPA PLAST. CANELADA', 'M2', 0.00, 'PTE', 0.00, 4.24, 4.24, 
 '2017-10-10', '2018-12-28', 62, NULL, 5, 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1422', '17622003', 'CHAPA XADREZ', 'CHAPA XADREZ 2000x1000x3,00', 'KG', 0.00, 'EUR', 1.42, 0.56, 0.56, 
 '2017-12-27', '2020-12-15', 14, NULL, 5, 1, NULL, NULL, 6, 2000, 1000, NULL, 3.00, NULL),
('1423', '17622004', 'CHAPA XADREZ', 'CHAPA XADREZ 2000x1000x4,00', 'KG', 0.00, 'EUR', 1.42, 0.60, 0.60, 
 '2017-10-10', '2023-12-14', 14, NULL, 5, 1, NULL, NULL, 6, 2000, 1000, NULL, 4.00, NULL),
('1424', '17622005', 'CHAPA XADREZ', 'CHAPA XADREZ 2000x1000x5,00', 'KG', 0.00, 'EUR', 1.42, 0.53, 0.53, 
 '2017-10-10', '2023-01-11', 14, NULL, 5, 1, NULL, NULL, 6, 2000, 1000, NULL, 5.00, NULL),
('1425', '17623003', 'CHAPA XADREZ', 'CHAPA XADREZ 3000x1250x3,00', 'KG', 0.00, 'EUR', 1.42, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 6, 3000, 1250, NULL, 3.00, NULL),
('1426', '17623005', 'CHAPA XADREZ', 'CHAPA XADREZ 2340x1000x5,00', 'KG', 0.00, 'EUR', 1.42, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 6, 2340, 1000, NULL, 5.00, NULL),
('1427', '17623015', 'CHAPA XADREZ', 'CHAPA XADREZ 3000x1500x4,00', 'KG', 0.00, 'EUR', 1.42, 0.54, 0.54, 
 '2017-10-10', '2018-12-06', 14, NULL, 5, 1, NULL, NULL, 6, 3000, 1500, NULL, 4.00, NULL),
('1428', '17624015', 'CHAPA XADREZ', 'CHAPA XADREZ 3000x1500x3,00', 'KG', 0.00, 'EUR', 1.42, 1.24, 1.24, 
 '2021-09-03', '2023-11-16', 14, NULL, 5, 1, NULL, NULL, 6, 3000, 1500, NULL, 3.00, NULL),
('1429', '17625015', 'CHAPA XADREZ', 'CHAPA XADREZ 3000x1500x5,00', 'KG', 0.00, 'EUR', 1.42, 0.64, 0.64, 
 '2021-03-22', '2021-03-23', 14, NULL, 5, 1, NULL, NULL, 6, 3000, 1500, NULL, 5.00, NULL),
('1430', '17625153', 'CHAPA XADREZ', 'CHAPA XADREZ 2500x1250x3,00', 'KG', 0.00, 'EUR', 1.42, 0.57, 0.57, 
 '2018-07-09', '2018-12-06', 14, NULL, 5, 1, NULL, NULL, 6, 2500, 1250, NULL, 3.00, NULL),
('1431', '17625154', 'CHAPA XADREZ', 'CHAPA XADREZ 2500x1250x4,00', 'KG', 0.00, 'EUR', 1.42, 0.60, 0.60, 
 '2018-12-06', '2020-12-09', 14, NULL, 5, 1, NULL, NULL, 6, 2500, 1250, NULL, 4.00, NULL),
('1432', '17625155', 'CHAPA XADREZ', 'CHAPA XADREZ 2500x1250x5,00', 'KG', 0.00, 'EUR', 1.42, 0.62, 0.62, 
 '2019-03-08', '2021-08-11', 14, NULL, 5, 1, NULL, NULL, 6, 2500, 1250, NULL, 5.00, NULL),
('1433', '17625213', 'CHAPA XADREZ', 'CHAPA XADREZ 5000x1500x3,00', 'KG', 0.00, 'EUR', 1.42, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 6, 5000, 1500, NULL, 3.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('2457', '19922003', 'CHAPA XADREZ', 'CHAPA XADREZ 2000x1000x3,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 2000, 1000, NULL, 3.00, NULL),
('2458', '19922003', 'CHAPA XADREZ', 'CHAPA XADREZ 2000x1000x3,00', 'KG', 0.00, 'PTE', 115.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 2000, 1000, NULL, 3.00, NULL),
('2459', '19922004', 'CHAPA XADREZ', 'CHAPA XADREZ 2000x1000x4,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 2000, 1000, NULL, 4.00, NULL),
('2460', '19922004', 'CHAPA XADREZ', 'CHAPA XADREZ 2000x1000x4,00', 'KG', 0.00, 'PTE', 115.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 2000, 1000, NULL, 4.00, NULL),
('2461', '19922005', 'CHAPA XADREZ', 'CHAPA XADREZ 2000x1000x5,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 2000, 1000, NULL, 5.00, NULL),
('2462', '19922005', 'CHAPA XADREZ', 'CHAPA XADREZ 2000x1000x5,00', 'KG', 0.00, 'PTE', 115.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 2000, 1000, NULL, 5.00, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1434', '17626103', 'CHAPA GOTAS', 'CHAPA GOTAS 2000x1000x3,00', 'KG', 6049.00, 'EUR', 1.42, 0.89, 0.90, 
 '2024-02-27', '2024-06-14', 14, NULL, 5, 1, NULL, NULL, 11, 2000, 1000, NULL, 3.00, NULL),
('1435', '17626104', 'CHAPA GOTAS', 'CHAPA GOTAS 2000x1000x4,00', 'KG', 2589.00, 'EUR', 1.42, 0.97, 0.97, 
 '2023-09-22', '2024-02-28', 14, NULL, 5, 1, NULL, NULL, 11, 2000, 1000, NULL, 4.00, NULL),
('1436', '17626105', 'CHAPA GOTAS', 'CHAPA GOTAS 2000x1000x5,00', 'KG', 3429.00, 'EUR', 1.42, 0.88, 0.85, 
 '2024-04-03', '2024-06-13', 14, NULL, 5, 1, NULL, NULL, 11, 2000, 1000, NULL, 5.00, NULL),
('1437', '17626123', 'CHAPA GOTAS', 'CHAPA GOTAS 2500x1250x3,00', 'KG', 6311.00, 'EUR', 1.42, 0.87, 0.87, 
 '2024-04-30', '2024-06-07', 14, NULL, 5, 1, NULL, NULL, 11, 2500, 1250, NULL, 3.00, NULL),
('1438', '17626124', 'CHAPA GOTAS', 'CHAPA GOTAS 3000x1500x3,00', 'KG', 16297.00, 'EUR', 1.42, 0.88, 0.89, 
 '2024-05-31', '2024-06-04', 14, NULL, 5, 1, NULL, NULL, 11, 3000, 1500, NULL, 3.00, NULL),
('1439', '17626125', 'CHAPA GOTAS', 'CHAPA GOTAS 3000x1500x4,00', 'KG', 4238.00, 'EUR', 1.42, 0.84, 0.84, 
 '2023-10-30', '2024-05-29', 14, NULL, 5, 1, NULL, NULL, 11, 3000, 1500, NULL, 4.00, NULL),
('1440', '17626126', 'CHAPA GOTAS', 'CHAPA GOTAS 3000x1500x5,00', 'KG', 5996.00, 'EUR', 1.42, 0.88, 0.87, 
 '2024-05-03', '2024-06-11', 14, NULL, 5, 1, NULL, NULL, 11, 3000, 1500, NULL, 5.00, NULL),
('1447', '17626300', 'CHAPA GOTAS', 'CHAPA GOTAS 3000x1500x8/10mm', 'KG', 0.00, 'EUR', 0.97, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 11, 3000, 1500, NULL, NULL, NULL),
('1448', '17626301', 'CHAPA GOTAS', 'CHAPA GOTAS 3915x1500x5,00', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 11, 3915, 1500, NULL, 5.00, NULL),
('1449', '17626302', 'CHAPA GOTAS', 'CHAPA GOTAS 4665x1250x5,00', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 11, 4665, 1250, NULL, 5.00, NULL),
('1450', '17626303', 'CHAPA GOTAS', 'CHAPA GOTAS 4670x1500x5,00', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 11, 4670, 1500, NULL, 5.00, NULL),
('1451', '17626304', 'CHAPA GOTAS', 'CHAPA GOTAS 5410x1500x5,00', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 11, 5410, 1500, NULL, 5.00, NULL),
('1452', '17626305', 'CHAPA GOTAS', 'CHAPA GOTAS 4840x1500x5,00', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 11, 4840, 1500, NULL, 5.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('1453', '17627103', 'CHAPA GOTAS', 'CHAPA GOTAS 3000x1000x3,00', 'KG', 0.00, 'EUR', 1.42, 0.68, 0.00, 
 '2020-03-06', '2020-03-06', 14, NULL, 5, 1, NULL, NULL, 11, 3000, 1000, NULL, 3.00, NULL),
('1454', '17627104', 'CHAPA GOTAS', 'CHAPA GOTAS 3000x1250x4,00', 'KG', 0.00, 'EUR', 1.17, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 14, NULL, 5, 1, NULL, NULL, 11, 3000, 1250, NULL, 4.00, NULL),
('1455', '17627124', 'CHAPA GOTAS', 'CHAPA GOTAS 2500x1250x4,00', 'KG', 3193.00, 'EUR', 1.42, 0.87, 0.85, 
 '2024-04-03', '2024-04-30', 14, NULL, 5, 1, NULL, NULL, 11, 2500, 1250, NULL, 4.00, NULL),
('1456', '17627126', 'CHAPA GOTAS', 'CHAPA GOTAS 2500x1250x5,00', 'KG', 2960.00, 'EUR', 1.42, 0.86, 0.85, 
 '2024-01-02', '2024-05-14', 14, NULL, 5, 1, NULL, NULL, 11, 2500, 1250, NULL, 5.00, NULL),
('2471', '19926103', 'CHAPA GOTAS', 'CHAPA GOTAS 2000x1000x3,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 11, 2000, 1000, NULL, 3.00, NULL),
('2472', '19926103', 'CHAPA GOTAS', 'CHAPA GOTAS 2000x1000x3,00', 'KG', 0.00, 'PTE', 0.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 11, 2000, 1000, NULL, 3.00, NULL),
('2473', '19926123', 'CHAPA GOTAS', 'CHAPA GOTAS 2500x1250x3,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 11, 2500, 1250, NULL, 3.00, NULL),
('2474', '19926123', 'CHAPA GOTAS', 'CHAPA GOTAS 2500x1250x3,00', 'KG', 0.00, 'PTE', 0.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 11, 2500, 1250, NULL, 3.00, NULL),
('2475', '19926124', 'CHAPA GOTAS', 'CHAPA GOTAS 3000x1500x3,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 11, 3000, 1500, NULL, 3.00, NULL),
('2476', '19926124', 'CHAPA GOTAS', 'CHAPA GOTAS 3000x1500x3,00', 'KG', 0.00, 'PTE', 0.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 11, 3000, 1500, NULL, 3.00, NULL),
('2477', '23631001', 'CH. INOX 304 GOTAS', 'CH. INOX 304 GOTAS 3000x1250x3/5', 'KG', 0.00, 'EUR', 4.42, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 62, NULL, 5, 4, NULL, NULL, 11, 3000, 1250, NULL, NULL, NULL);






INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, price_pvp, 
    price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, 
    type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter
) VALUES
('2463', '19923015', 'CHAPA  XADREZ', 'CHAPA  XADREZ 3x1,5x4,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 3000, 1500, NULL, 4.00, NULL),
('2464', '19923015', 'CHAPA  XADREZ', 'CHAPA  XADREZ 3x1,5x4,00', 'KG', 0.00, 'PTE', 115.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 3000, 1500, NULL, 4.00, NULL),
('2465', '19924015', 'CHAPA  XADREZ', 'CHAPA  XADREZ 3x1,5x3,00', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 3000, 1500, NULL, 3.00, NULL),
('2466', '19924015', 'CHAPA  XADREZ', 'CHAPA  XADREZ 3x1,5x3,00', 'KG', 0.00, 'PTE', 115.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 3000, 1500, NULL, 3.00, NULL),
('2467', '19925153', 'CHAPA  XADREZ', 'CHAPA  XADREZ 2,5x1,25x3/5', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 2500, 1250, NULL, 0.60, NULL),
('2468', '19925153', 'CHAPA  XADREZ', 'CHAPA  XADREZ 2,5x1,25x3/5', 'KG', 0.00, 'PTE', 115.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 2500, 1250, NULL, 0.60, NULL),
('2469', '19925155', 'CHAPA  XADREZ', 'CHAPA  XADREZ 2,5x1,25x5/7', 'KG', 0.00, 'EUR', 0.63, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 2500, 1250, NULL, 0.70, NULL),
('2470', '19925155', 'CHAPA  XADREZ', 'CHAPA  XADREZ 2,5x1,25x5/7', 'KG', 0.00, 'PTE', 115.00, 0.00, 0.00, 
 '1900-01-01', '1900-01-01', 15, NULL, 5, 1, NULL, NULL, 6, 2500, 1250, NULL, 0.70, NULL);


-- METER ROLO NA DESCRI'CAO


    UPDATE t_product_catalog
SET description = CONCAT(description, ' (ROLO)')
WHERE description_full LIKE '%(ROLO)%' 
  AND description NOT LIKE '%(ROLO)%'
  and type_id = 5;



-- TUBOS (SAO 1.700 💀💀)
-- LETS GO

INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2727', '20000910', 'TUBO ACO RED.', 'TUBO ACO RED. 10x1,00', 'MT', 0.00, 'EUR', 
 0.72, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 10),
('2728', '20000912', 'TUBO ACO RED.', 'TUBO ACO RED. 10x1,25', 'MT', 534.00, 'EUR', 
 0.87, 0.31, 0.31, '2024-02-27', '2024-06-06', 21, NULL, 6, 2, 11, NULL,NULL,  NULL, NULL, NULL, 1.25, 10),
('2729', '20000915', 'TUBO ACO RED.', 'TUBO AÇO RED. 10x1,50', 'MT', 0.00, 'EUR', 
 1.19, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 10),
('2730', '20001210', 'TUBO ACO RED.', 'TUBO ACO RED. 12x1,00', 'MT', 0.00, 'EUR', 
 0.73, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 12),
('2731', '20001212', 'TUBO ACO RED.', 'TUBO ACO RED. 12x1,25', 'MT', 0.00, 'EUR', 
 0.92, 0.30, 0.30, '2017-10-10', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 12),
('2732', '20001215', 'TUBO ACO RED.', 'TUBO ACO RED. 12x1,50', 'MT', 2106.00, 'EUR', 
 1.31, 0.56, 0.56, '2024-03-13', '2024-06-06', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 12),
('2733', '20001408', 'TUBO ACO RED.', 'TUBO AÇO RED. 14x0,80', 'MT', 0.00, 'EUR', 
 0.64, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 0.80, 14),
('2734', '20001410', 'TUBO ACO RED.', 'TUBO ACO RED. 14x1,00', 'MT', 0.00, 'EUR', 
 0.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 14),
('2735', '20001608', 'TUBO ACO RED.', 'TUBO AÇO RED. 16x0,80', 'MT', 31.02, 'EUR', 
 0.83, 0.27, 0.50, '2017-10-10', '2023-10-02', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 0.80, 16),
('2736', '20001610', 'TUBO ACO RED.', 'TUBO ACO RED. 16x1,00', 'MT', 0.00, 'EUR', 
 0.87, 0.35, 0.35, '2024-01-08', '2024-01-09', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 16),
('2737', '20001612', 'TUBO ACO RED.', 'TUBO ACO RED. 16x1,25', 'MT', 12.00, 'EUR', 
 1.16, 0.35, 0.35, '2019-07-01', '2022-09-29', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 16),
('2738', '20001615', 'TUBO ACO RED.', 'TUBO ACO RED. 16x1,50', 'MT', 2004.00, 'EUR', 
 1.39, 0.56, 0.56, '2024-05-09', '2024-06-14', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 16),
('2739', '20001620', 'TUBO ACO RED.', 'TUBO ACO RED. 16x2,00', 'MT', 318.00, 'EUR', 
 1.75, 0.74, 0.74, '2024-01-09', '2024-06-14', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 16),
('2740', '20001810', 'TUBO ACO RED.', 'TUBO AÇO RED. 18x1,00', 'MT', 0.00, 'EUR', 
 0.97, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 18);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2741', '20001812', 'TUBO AÇO RED.', 'TUBO AÇO RED.  18x1,25', 'MT', 0.00, 'EUR', 
 1.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 18),
('2742', '20001908', 'TUBO AÇO RED.', 'TUBO AÇO RED.  19x0,80', 'MT', 0.00, 'EUR', 
 0.89, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 0.80, 19),
('2743', '20001910', 'TUBO AÇO RED.', 'TUBO ACO RED.  19x1,00', 'MT', 0.00, 'EUR', 
 1.06, 0.44, 0.44, '2024-04-23', '2024-04-23', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 19),
('2744', '20001912', 'TUBO AÇO RED.', 'TUBO ACO RED.  19x1,25', 'MT', 0.00, 'EUR', 
 1.30, 0.46, 0.46, '2017-10-10', '2019-10-23', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 19),
('2745', '20001915', 'TUBO AÇO RED.', 'TUBO ACO RED.  19x1,50', 'MT', 0.00, 'EUR', 
 1.69, 0.42, 0.42, '2017-10-10', '2017-10-25', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 19),
('2746', '20001920', 'TUBO AÇO RED.', 'TUBO ACO RED.  19x2,00', 'MT', 0.00, 'EUR', 
 2.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 19),
('2747', '20002012', 'TUBO AÇO RED.', 'TUBO AÇO RED.  20x1,25', 'MT', 0.00, 'EUR', 
 1.34, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 20),
('2748', '20002015', 'TUBO AÇO RED.', 'TUBO AÇO RED   20x1,50', 'MT', 3906.00, 'EUR', 
 1.61, 0.65, 0.63, '2024-05-28', '2024-06-14', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 20),
('2749', '20002020', 'TUBO AÇO RED.', 'TUBO AÇO RED.  20x2,00', 'MT', 564.00, 'EUR', 
 2.00, 0.82, 0.82, '2024-04-08', '2024-06-12', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 20),
('2750', '20002208', 'TUBO AÇO RED.', 'TUBO AÇO RED. 22x0,80', 'ML', 0.00, 'EUR', 
 1.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 0.80, 22),
('2751', '20002208', 'TUBO AÇO RED.', 'TUBO AÇO RED. 22x0,80', 'MT', 0.00, 'EUR', 
 1.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 0.80, 22);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2752', '20002210', 'TUBO ACO RED.', 'TUBO ACO RED.  22x1,00', 'MT', 0.00, 'EUR', 
 1.13, 0.54, 0.54, '2024-05-14', '2024-05-15', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 22),
('2753', '20002212', 'TUBO ACO RED.', 'TUBO ACO RED.  22x1,25', 'MT', 0.00, 'EUR', 
 1.46, 0.49, 0.49, '2017-10-10', '2021-07-30', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 22),
('2754', '20002215', 'TUBO ACO RED.', 'TUBO ACO RED.  22x1,50', 'MT', 1548.00, 'EUR', 
 1.81, 0.74, 0.74, '2024-05-03', '2024-06-12', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 22),
('2755', '20002220', 'TUBO ACO RED.', 'TUBO ACO RED.  22x2,00', 'MT', 0.00, 'EUR', 
 2.28, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 22),
('2756', '20002510', 'TUBO ACO RED.', 'TUBO ACO RED.  25x1,00', 'MT', 0.00, 'EUR', 
 1.26, 0.40, 0.40, '2017-10-10', '2018-08-17', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 25),
('2757', '20002512', 'TUBO ACO RED.', 'TUBO ACO RED.  25x1,25', 'MT', 0.00, 'EUR', 
 1.66, 0.00, 0.00, '2021-03-19', '2021-03-19', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 25),
('2758', '20002515', 'TUBO ACO RED.', 'TUBO ACO RED.  25x1,50', 'MT', 4188.00, 'EUR', 
 1.96, 0.82, 0.77, '2024-05-22', '2024-06-13', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 25),
('2759', '20002520', 'TUBO ACO RED.', 'TUBO ACO RED.  25x2,00', 'MT', 798.00, 'EUR', 
 2.45, 1.00, 1.01, '2024-03-15', '2024-06-06', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 25),
('2760', '20002709', 'TUBO AÇO RED.', 'TUBO AÇO RED.  27x00,9', 'MT', 0.00, 'EUR', 
 1.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 0.90, 27),
('2761', '20002710', 'TUBO AÇO RED.', 'TUBO AÇO RED.  27x1,00', 'MT', 1194.00, 'EUR', 
 1.59, 0.61, 0.61, '2017-10-10', '2024-04-17', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 27),
('2762', '20002712', 'TUBO AÇO RED.', 'TUBO AÇO RED.  27x1,25', 'MT', 49.98, 'EUR', 
 1.87, 0.45, 0.45, '2017-10-10', '2024-04-02', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 27),
('2763', '20002810', 'TUBO ACO RED.', 'TUBO ACO RED.  28x1,00', 'MT', 0.00, 'EUR', 
 1.59, 0.50, 0.50, '2017-10-10', '2018-12-14', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 28);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2764', '20002812', 'TUBO ACO RED.', 'TUBO ACO RED.  28x1,25', 'MT', 0.00, 'EUR', 
 1.87, 0.43, 0.43, '2017-10-10', '2018-12-14', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 28),
('2765', '20002815', 'TUBO ACO RED.', 'TUBO ACO RED.  28x1,50', 'MT', 372.00, 'EUR', 
 2.33, 1.02, 1.02, '2023-02-10', '2024-06-05', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 28),
('2766', '20002820', 'TUBO ACO RED.', 'TUBO ACO RED.  28x2,00', 'MT', 0.00, 'EUR', 
 2.93, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 28),
('2767', '20003003', 'TUBO ACO RED.', 'TUBO ACO RED.  30x3,00', 'MT', 0.00, 'EUR', 
 4.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 30),
('2768', '20003010', 'TUBO ACO RED.', 'TUBO ACO RED.  30x1,00', 'MT', 0.00, 'EUR', 
 1.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 30),
('2769', '20003012', 'TUBO ACO RED.', 'TUBO ACO RED.  30x1,25', 'MT', 0.00, 'EUR', 
 1.95, 1.22, 1.22, '2021-09-24', '2021-09-30', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 30),
('2770', '20003015', 'TUBO ACO RED.', 'TUBO ACO RED.  30x1,50', 'MT', 522.00, 'EUR', 
 2.30, 0.97, 0.98, '2024-01-30', '2024-05-28', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 30),
('2771', '20003020', 'TUBO ACO RED.', 'TUBO ACO RED.  30x2,00', 'MT', 636.00, 'EUR', 
 2.91, 1.17, 1.17, '2024-04-18', '2024-06-06', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 30),
('2772', '20003210', 'TUBO ACO RED.', 'TUBO ACO RED.  32x1,00', 'MT', 0.00, 'EUR', 
 1.86, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 32),
('2773', '20003212', 'TUBO ACO RED.', 'TUBO ACO RED.  32x1,25', 'MT', 0.00, 'EUR', 
 2.07, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 32),
('2774', '20003215', 'TUBO ACO RED.', 'TUBO ACO RED.  32x1,50', 'MT', 2550.00, 'EUR', 
 2.49, 0.99, 0.98, '2024-05-28', '2024-05-21', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 32),
('2775', '20003220', 'TUBO ACO RED.', 'TUBO ACO RED.  32x2,00', 'MT', 0.00, 'EUR', 
 3.16, 0.00, 0.00, NULL, '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 32),
('2776', '20003225', 'TUBO ACO RED.', 'TUBO ACO RED.  32x2,50', 'MT', 0.00, 'EUR', 
 3.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.50, 32);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2777', '20003512', 'TUBO ACO RED.', 'TUBO ACO RED.  35x1,25', 'MT', 0.00, 'EUR', 
 2.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 35),
('2778', '20003515', 'TUBO ACO RED.', 'TUBO ACO RED.  35x1,50', 'MT', 354.00, 'EUR', 
 2.73, 1.33, 1.34, '2022-09-20', '2024-06-12', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 35),
('2779', '20003520', 'TUBO ACO RED.', 'TUBO ACO RED.  35x2,00', 'MT', 0.00, 'EUR', 
 3.49, 1.01, 1.01, '2017-10-10', '2018-09-10', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 35),
('2780', '20003812', 'TUBO ACO RED.', 'TUBO ACO RED.  38x1,25', 'MT', 0.00, 'EUR', 
 2.69, 0.77, 0.77, '2017-10-10', '2018-12-14', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 38),
('2781', '20003815', 'TUBO ACO RED.', 'TUBO ACO RED.  38x1,50', 'MT', 558.00, 'EUR', 
 2.96, 1.36, 1.37, '2023-06-02', '2024-06-06', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 38),
('2782', '20003820', 'TUBO ACO RED.', 'TUBO ACO RED.  38x2,00', 'MT', 0.00, 'EUR', 
 4.19, 2.68, 2.68, '2022-04-21', '2022-12-07', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 38),
('2783', '20004012', 'TUBO ACO RED.', 'TUBO ACO RED.  40x1,25', 'MT', 0.00, 'EUR', 
 2.72, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 40),
('2784', '20004015', 'TUBO ACO RED.', 'TUBO ACO RED.  40x1,50', 'MT', 1008.00, 'EUR', 
 3.11, 1.26, 1.22, '2024-04-15', '2024-06-03', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 40),
('2785', '20004020', 'TUBO ACO RED.', 'TUBO ACO RED.  40x2,00', 'MT', 900.00, 'EUR', 
 4.00, 1.63, 1.60, '2024-05-09', '2024-06-05', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 40),
('2786', '20004030', 'TUBO ACO RED.', 'TUBO ACO RED.  40X3,00', 'MT', 0.00, 'EUR', 
 5.74, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 40),
('2787', '20004110', 'TUBO ACO RED.', 'TUBO ACO RED.  41x1,00', 'MT', 0.00, 'EUR', 
 2.57, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 41),
('2788', '20004112', 'TUBO ACO RED.', 'TUBO ACO RED.  41x1,25', 'MT', 0.00, 'EUR', 
 2.72, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 41),
('2789', '20004115', 'TUBO ACO RED.', 'TUBO ACO RED.  42x1,50', 'MT', 750.00, 'EUR', 
 3.25, 1.28, 1.27, '2024-05-28', '2024-05-17', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 42);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2790', '20004120', 'TUBO ACO RED.', 'TUBO ACO RED.  42x2,00', 'MT', 474.00, 'EUR', 
 4.16, 1.72, 1.73, '2023-12-21', '2024-05-31', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 42),
('2791', '20004512', 'TUBO ACO RED.', 'TUBO ACO RED.  45x1,25', 'MT', 0.00, 'EUR', 
 3.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 45),
('2792', '20004515', 'TUBO ACO RED.', 'TUBO ACO RED.  45x1,50', 'MT', 570.00, 'EUR', 
 3.54, 1.40, 1.39, '2024-04-16', '2024-06-07', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 45),
('2793', '20004520', 'TUBO ACO RED.', 'TUBO ACO RED.  45x2,00', 'MT', 840.00, 'EUR', 
 4.51, 1.82, 1.77, '2024-05-24', '2024-06-13', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 45),
('2794', '20004812', 'TUBO ACO RED.', 'TUBO ACO RED.  48x1,25', 'MT', 0.00, 'EUR', 
 3.98, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 48),
('2795', '20004815', 'TUBO ACO RED.', 'TUBO ACO RED.  48x1,50', 'MT', 336.00, 'EUR', 
 3.73, 1.58, 1.58, '2022-10-04', '2024-05-27', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 48),
('2796', '20004820', 'TUBO ACO RED.', 'TUBO ACO RED.  48x2,00', 'MT', 636.00, 'EUR', 
 4.83, 1.95, 1.93, '2024-05-09', '2024-05-10', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 48),
('2797', '20004825', 'TUBO ACO RED.', 'TUBO ACO RED.  48x2,50', 'MT', 0.00, 'EUR', 
 6.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.50, 48),
('2798', '20004830', 'TUBO ACO RED.', 'TUBO ACO RED.  48x3,00', 'MT', 0.00, 'EUR', 
 6.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 48),
('2799', '20005012', 'TUBO ACO RED.', 'TUBO ACO RED.  50x1,25', 'MT', 0.00, 'EUR', 
 3.98, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 50),
('2800', '20005015', 'TUBO ACO RED.', 'TUBO ACO RED.  50x1,50', 'MT', 900.00, 'EUR', 
 4.01, 1.61, 1.61, '2024-05-09', '2024-06-13', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 50),
('2801', '20005020', 'TUBO ACO RED.', 'TUBO ACO RED.  50x2,00', 'MT', 540.00, 'EUR', 
 5.10, 2.05, 2.04, '2024-04-08', '2024-06-13', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 50),
('2802', '20005025', 'TUBO ACO RED.', 'TUBO ACO RED.  50x2,50', 'MT', 0.00, 'EUR', 
 7.28, 1.59, 1.59, '2020-11-04', '2020-12-24', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.50, 50),
('2803', '20005030', 'TUBO ACO RED.', 'TUBO ACO RED.  50x3,00', 'MT', 0.00, 'EUR', 
 7.28, 2.15, 2.15, '2017-10-10', '2024-04-24', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 50),
('2804', '20005515', 'TUBO ACO RED.', 'TUBO ACO RED.  55x1,50', 'MT', 0.00, 'EUR', 
 4.46, 1.46, 1.46, '2017-10-10', '2017-11-07', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 55);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2805', '20005520', 'TUBO ACO RED.', 'TUBO ACO RED.  55x2,00', 'MT', 0.00, 'EUR', 
 5.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 55),
('2806', '20005715', 'TUBO ACO RED.', 'TUBO ACO RED.  57x1.50', 'MT', 480.00, 'EUR', 
 5.04, 1.98, 1.98, '2017-10-10', '2023-11-20', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 57),
('2807', '20005720', 'TUBO ACO RED.', 'TUBO ACO RED.  57x2,00', 'MT', 0.00, 'EUR', 
 6.49, 1.25, 1.25, '2018-06-01', '2018-07-05', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 57),
('2808', '20005730', 'TUBO ACO RED.', 'TUBO ACO RED.  57x3,00', 'MT', 0.00, 'EUR', 
 8.81, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 57),
('2809', '20006015', 'TUBO ACO RED.', 'TUBO ACO RED.  60x1,50', 'MT', 600.00, 'EUR', 
 4.81, 2.00, 1.96, '2024-04-08', '2024-06-13', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 60),
('2810', '20006020', 'TUBO ACO RED.', 'TUBO ACO RED.  60x2,00', 'MT', 1314.00, 'EUR', 
 6.04, 2.56, 2.56, '2024-03-13', '2024-05-27', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 60),
('2811', '20006030', 'TUBO ACO RED.', 'TUBO ACO RED.  60x3,00', 'MT', 0.00, 'EUR', 
 8.83, 2.87, 0.00, '2020-04-14', '2020-04-09', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 60),
('2812', '20006515', 'TUBO ACO RED.', 'TUBO ACO RED.  65x1,50', 'MT', 0.00, 'EUR', 
 5.78, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 65),
('2813', '20006520', 'TUBO ACO RED.', 'TUBO ACO RED.  65x2,00', 'MT', 0.00, 'EUR', 
 7.48, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 65),
('2814', '20006530', 'TUBO ACO RED.', 'TUBO ACO RED.  65x3,00', 'MT', 0.00, 'EUR', 
 10.38, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 65),
('2815', '20007015', 'TUBO ACO RED.', 'TUBO ACO RED.  70x1,50', 'MT', 0.00, 'EUR', 
 5.64, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 70),
('2816', '20007020', 'TUBO ACO RED.', 'TUBO ACO RED.  70x2,00', 'MT', 126.00, 'EUR', 
 7.16, 2.92, 2.92, '2024-04-08', '2024-05-28', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 70),
('2817', '20007030', 'TUBO ACO RED.', 'TUBO ACO RED.  70x3,00', 'MT', 0.00, 'EUR', 
 10.38, 6.23, 8.30, '2021-10-29', '2021-11-04', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 70),
('2818', '20007515', 'TUBO ACO RED.', 'TUBO ACO RED.  75x1,50', 'MT', 0.00, 'EUR', 
 6.06, 1.82, 1.94, '2018-07-01', '2018-07-11', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 75),
('2819', '20007520', 'TUBO ACO RED.', 'TUBO ACO RED.  75x2,00', 'MT', 0.00, 'EUR', 
 7.83, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 75);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2820', '20007530', 'TUBO ACO RED.', 'TUBO ACO RED.  76,1x3,00', 'MT', 0.00, 'EUR', 
 11.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 76.1),
('2821', '20007615', 'TUBO AÇO RED.', 'TUBO AÇO RED. 76x1,50', 'MT', 0.00, 'EUR', 
 6.06, 2.52, 2.52, '2023-12-18', '2023-12-20', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 76),
('2822', '20007620', 'TUBO ACO RED.', 'TUBO ACO RED.  76x2,00', 'MT', 144.00, 'EUR', 
 7.83, 3.30, 3.30, '2023-08-04', '2024-05-14', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 76),
('2823', '20007630', 'TUBO EST. RED.', 'TUBO EST. RED. 76,1x3,00', 'KG', 0.00, 'EUR', 
 11.31, 6.06, 6.06, '2020-04-16', '2020-04-16', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 76.1),
('2824', '20007650', 'TUBO EST. RED.', 'TUBO EST. RED. 76,1x5,00', 'MT', 0.00, 'EUR', 
 19.08, 5.61, 0.00, '2020-04-16', '2020-04-23', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 76.1),
('2825', '20008015', 'TUBO ACO RED.', 'TUBO ACO RED.  80x1,50', 'MT', 0.00, 'EUR', 
 6.65, 4.54, 0.00, '2022-06-29', '2022-06-29', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 80),
('2826', '20008020', 'TUBO ACO RED.', 'TUBO ACO RED.  80x2,00', 'MT', 396.00, 'EUR', 
 8.45, 3.51, 3.28, '2023-12-04', '2024-05-20', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 80),
('2827', '20008030', 'TUBO ACO RED.', 'TUBO ACO RED.  80x3,00', 'MT', 0.00, 'EUR', 
 11.91, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 80),
('2828', '20008232', 'TUBO AÇO RED.', 'TUBO AÇO RED.  82,5x3,2', 'MT', 0.00, 'EUR', 
 17.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.20, 82.5),
('2829', '20008830', 'TUBO ACO RED.', 'TUBO ACO RED. 88,9x3,00', 'ML', 354.00, 'EUR', 
 14.01, 5.72, 5.72, '2024-06-10', '2024-06-13', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 88.9),
('2830', '20008960', 'TUBO ACO RED.', 'TUBO ACO RED.  89x6,00', 'M', 0.00, 'EUR', 
 25.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 6.00, 89),
('2831', '20009015', 'TUBO ACO RED.', 'TUBO ACO RED.  90x1,50', 'MT', 0.00, 'EUR', 
 7.44, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 90),
('2832', '20009020', 'TUBO ACO RED.', 'TUBO ACO RED.  90x2,00', 'MT', 0.00, 'EUR', 
 10.04, 4.22, 4.18, '2023-07-27', '2024-04-23', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 90),
('2833', '20009030', 'TUBO ACO RED.', 'TUBO ACO RED.  90x3,00', 'MT', 192.00, 'EUR', 
 13.46, 6.33, 6.25, '2023-05-19', '2024-05-17', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 90),
('2834', '20009040', 'TUBO ACO RED.', 'TUBO ACO RED.  90x4,00', 'MT', 0.00, 'EUR', 
 18.50, 9.92, 14.17, '2022-01-13', '2022-01-14', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 90),
('2835', '20009915', 'TUBO ACO RED.', 'TUBO ACO RED.  100x1.50', 'MT', 0.00, 'EUR', 
 8.71, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 100),
('2836', '20009920', 'TUBO ACO RED.', 'TUBO ACO RED.  100x2,00', 'MT', 1044.00, 'EUR', 
 11.29, 4.54, 4.52, '2024-06-06', '2024-06-14', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 100);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2837', '20009930', 'TUBO ACO RED.', 'TUBO ACO RED.  100x3,00', 'MT', 204.00, 'EUR', 
 15.83, 6.28, 6.20, '2024-05-24', '2024-05-16', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 100),
('2838', '20009940', 'TUBO AÇO RED.', 'TUBO AÇO RED. 100X4,00', 'MT', 0.00, 'EUR', 
 20.85, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 100),
('2839', '20009940', 'TUBO AÇO RED.', 'TUBO AÇO RED. 100X4,00', 'UN', 0.00, 'EUR', 
 20.85, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 100),
('2840', '20010150', 'TUBO RED.', 'TUBO RED. 101.6X5,00mm', 'MT', 0.00, 'EUR', 
 27.24, 7.50, 7.50, '2017-10-10', '2020-01-15', 23, NULL, 6, 1, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 101.6),
('2841', '20011430', 'TUBO AÇO RED.', 'TUBO AÇO RED. 114,3x,3,00', 'MT', 150.00, 'EUR', 
 17.20, 6.81, 6.74, '2024-05-22', '2024-06-13', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 114.3),
('2842', '20011430', 'TUBO AÇO RED.', 'TUBO AÇO RED. 114,3x,3,00', 'UN', 150.00, 'EUR', 
 17.20, 6.81, 6.74, '2024-05-22', '2024-06-13', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 114.3),
('2843', '20011431', 'TUBO AÇO RED.', 'TUBO AÇO RED. 114,3x4,00', 'ML', 0.00, 'EUR', 
 22.78, 14.76, 18.22, '2021-06-30', '2021-09-24', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 114.3),
('2844', '20011440', 'TUBO AÇO RED.', 'TUBO AÇO RED. 114,3x5,00', 'MT', 0.00, 'EUR', 
 29.35, 8.55, 8.55, '2017-10-10', '2018-08-20', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 114.3),
('2845', '20011440', 'TUBO AÇO RED.', 'TUBO AÇO RED. 114,3x5,00', 'UN', 0.00, 'EUR', 
 29.35, 8.55, 8.55, '2017-10-10', '2018-08-20', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 114.3),
('2846', '20012030', 'TUBO AÇO RED.', 'TUBO AÇO RED.  120x3,00', 'MT', 114.00, 'EUR', 
 18.10, 7.46, 7.38, '2024-06-10', '2024-06-06', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 120),
('2847', '20012030', 'TUBO AÇO RED.', 'TUBO AÇO RED.  120x3,00', 'UN', 114.00, 'EUR', 
 18.10, 7.46, 7.38, '2024-06-10', '2024-06-06', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 120),
('2848', '20012040', 'TUBO AÇO RED.', 'TUBO AÇO RED.  120x2,00', 'ML', 168.00, 'EUR', 
 14.25, 5.81, 5.81, '2024-06-10', '2024-05-29', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 120),
('2849', '20012040', 'TUBO AÇO RED.', 'TUBO AÇO RED.  120x2,00', 'MT', 168.00, 'EUR', 
 14.25, 5.81, 5.81, '2024-06-10', '2024-05-29', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 120),
('2850', '20012530', 'TUBO AÇO RED.', 'TUBO AÇO RED. 125x3,00', 'MT', 0.00, 'EUR', 
 18.88, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 125);


WITH cte AS (
    SELECT 
        MIN(id) AS id
    FROM 
        t_product_catalog
    WHERE 
        type_id = 6 AND shape_id = 11
    GROUP BY 
        product_code
)
DELETE FROM t_product_catalog
WHERE id NOT IN (SELECT id FROM cte) AND type_id = 6 AND shape_id = 11;




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2851', '20012540', 'TUBO AÇO RED.', 'TUBO AÇO RED.  127x4,00', 'MT', 0.00, 'EUR', 
 25.40, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 127),
('2853', '20012750', 'TUBO AÇO RED.', 'TUBO AÇO RED. 127x5,00', 'MT', 0.00, 'EUR', 
 37.00, 17.10, 17.10, '2022-01-26', '2022-02-18', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 127),
('2854', '20013330', 'TUBO AÇO RED.', 'TUBO AÇO RED. 133x3,00', 'ML', 0.00, 'EUR', 
 20.24, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 133),
('2856', '20013340', 'TUBO AÇO RED.', 'TUBO AÇO RED. 133x4,00', 'ML', 0.00, 'EUR', 
 26.90, 8.81, 8.81, '2018-06-30', '2018-06-27', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 133),
('2857', '20013930', 'TUBO AÇO RED.', 'TUBO AÇO RED.  139,7x3,00', 'MT', 0.00, 'EUR', 
 21.28, 8.67, 8.60, '2023-10-18', '2024-05-23', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 139.7),
('2859', '20013940', 'TUBO AÇO RED.', 'TUBO AÇO RED.  139,7x4,00', 'MT', 24.00, 'EUR', 
 28.38, 11.35, 11.35, '2024-05-31', '2024-06-13', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 139.7),
('2861', '20013950', 'TUBO AÇO RED.', 'TUBO AÇO RED. 139,7x5,00', 'MT', 0.00, 'EUR', 
 35.13, 14.05, 14.05, '2023-11-27', '2023-12-04', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 139.7),
('2863', '20013951', 'TUBO AÇO RED.', 'TUBO AÇO RED. 139,7x12,00', 'ML', 0.00, 'EUR', 
 93.35, 30.89, 30.89, '2019-10-01', '2019-12-18', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 12.00, 139.7),
('2864', '20015010', 'TUBO ACO RED.', 'TUBO ACO RED.150x10mm', 'MT', 0.00, 'EUR', 
 92.01, 76.70, 76.70, '2017-10-10', '2018-12-03', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 150),
('2865', '20015020', 'TUBO ACO RED.', 'TUBO ACO RED. FUR.50x2', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 50),
('2866', '20015230', 'TUBO ACO RED.', 'TUBO ACO RED 152x3,00', 'MT', 0.00, 'EUR', 
 24.23, 10.91, 10.91, '2023-05-19', '2023-09-21', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 152);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2867', '20015260', 'TUBO AÇO RED.', 'TUBO AÇO RED. 152x6,00', 'ML', 0.00, 'EUR', 
 48.76, 25.36, 39.01, '2022-01-26', '2022-02-18', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 6.00, 152),
('2868', '20015290', 'TUBO AÇO RED.', 'TUBO AÇO RED. 152,4x10,00', 'MT', 0.00, 'EUR', 
 84.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 152.4),
('2869', '20016830', 'TUBO AÇO RED.', 'TUBO AÇO RED. 168,3x3,00', 'MT', 30.00, 'EUR', 
 25.70, 10.28, 10.28, '2023-11-02', '2024-06-05', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 168.3),
('2870', '20016840', 'TUBO AÇO RED.', 'TUBO AÇO RED. 168,3x4,00', 'ML', 0.00, 'EUR', 
 34.30, 13.78, 13.72, '2023-11-02', '2024-05-16', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 168.3),
('2871', '20016850', 'TUBO AÇO RED.', 'TUBO AÇO RED. 168,3x5,00', 'MT', 96.00, 'EUR', 
 42.59, 17.38, 17.38, '2024-06-10', '2024-06-07', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 168.3),
('2872', '20016860', 'TUBO AÇO RED.', 'TUBO AÇO RED. 168,3x6,00', 'MT', 0.00, 'EUR', 
 51.69, 28.12, 41.35, '2022-02-15', '2022-02-18', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 6.00, 168.3),
('2873', '20016890', 'TUBO AÇO RED.', 'TUBO AÇO RED. 168,3x10,00', 'MT', 0.00, 'EUR', 
 90.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 168.3),
('2874', '20017750', 'TUBO AÇO RED.', 'TUBO AÇO RED. 177,8x5,00', 'MT', 0.00, 'EUR', 
 16.45, 23.33, 37.03, '2022-08-03', '2022-08-06', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 177.8),
('2875', '20017780', 'TUBO AÇO RED.', 'TUBO AÇO RED. 159x3,00', 'ML', 0.00, 'EUR', 
 24.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 159),
('2877', '20019340', 'TUBO AÇO RED.', 'TUBO AÇO RED. 193,7x4,00', 'MT', 0.00, 'EUR', 
 39.56, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 193.7),
('2878', '20019360', 'TUBO AÇO RED.', 'TUBO AÇO RED. 193,7x5,00', 'MT', 42.00, 'EUR', 
 49.29, 20.11, 20.11, '2024-04-08', '2024-06-13', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 193.7),
('2879', '20019361', 'TUBO AÇO RED.', 'TUBO AÇO RED. 193,7x10,00', 'MT', 0.00, 'EUR', 
 106.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 193.7),
('2880', '20019362', 'TUBO AÇO RED.', 'TUBO AÇO RED. 193,7x6,00', 'MT', 0.00, 'EUR', 
 59.90, 30.17, 47.89, '2022-10-25', '2022-10-27', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 6.00, 193.7),
('2881', '20020040', 'TUBO AÇO RED.', 'TUBO AÇO RED. 200x4,00 mm', 'MT', 48.00, 'EUR', 
 44.10, 17.33, 17.33, '2024-04-08', '2024-05-10', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 200);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2882', '20020050', 'TUBO AÇO RED.', 'TUBO AÇO RED. 200x3,00 mm', 'MT', 0.00, 'EUR', 
 32.15, 12.99, 12.99, '2023-10-20', '2023-10-26', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 200),
('2883', '20020060', 'TUBO AÇO RED.', 'TUBO AÇO RED.  200x3,00', 'MT', 0.00, 'EUR', 
 18.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 200),
('2884', '20020061', 'TUBO AÇO RED.', 'TUBO AÇO RED. 200x5,00', 'MT', 0.00, 'EUR', 
 54.19, 22.54, 22.54, '2023-08-04', '2023-08-11', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 200),
('2885', '20021910', 'TUBO AÇO RED.', 'TUBO AÇO RED. 219,1x5,00', 'ML', 0.00, 'EUR', 
 35.40, 19.37, 19.37, '2020-10-22', '2020-04-22', 21, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 219.1),
('2886', '20021911', 'TUBO AÇO RED.', 'TUBO AÇO RED. 219,1x10,00mm', 'ML', 0.00, 'EUR', 
 119.96, 37.34, 37.34, '2019-11-28', '2019-12-02', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 219.1),
('2887', '20021940', 'TUBO AÇO RED.', 'TUBO AÇO RED. 219,1x4,00', 'ML', 0.00, 'EUR', 
 45.09, 19.48, 19.48, '2023-06-15', '2023-06-15', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 219.1),
('2888', '20021950', 'TUBO AÇO RED.', 'TUBO AÇO RED. 219,1x5,00', 'ML', 24.00, 'EUR', 
 56.19, 23.19, 22.92, '2024-04-08', '2024-06-03', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 219.1),
('2889', '20021960', 'TUBO AÇO RED.', 'TUBO AÇO RED. 219,1x12,50', 'ML', 0.00, 'EUR', 
 155.56, 54.78, 54.78, '2018-08-30', '2018-08-30', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 12.50, 219.1),
('2890', '20021961', 'TUBO AÇO RED.', 'TUBO AÇO RED. 219,1x6,00', 'MT', 0.00, 'EUR', 
 68.20, 34.36, 54.54, '2022-10-25', '2022-10-27', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 6.00, 219.1),
('2891', '20021990', 'TUBO AÇO RED.', 'TUBO AÇO RED. 219,1x3,00', 'ML', 0.00, 'EUR', 
 33.86, 18.96, 27.09, '2021-12-16', '2022-05-12', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 3.00, 219.1),
('2892', '20024450', 'TUBO AÇO RED.', 'TUBO AÇO RED. 244,5x6,00', 'ML', 0.00, 'EUR', 
 79.49, 20.57, 20.57, '2019-11-25', '2019-11-26', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 6.00, 244.5),
('2893', '20024460', 'TUBO AÇO RED.', 'TUBO AÇO RED. 244,5x4,00', 'ML', 0.00, 'EUR', 
 52.08, 16.20, 16.20, '2017-10-10', '2018-04-03', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 244.5),
('2894', '20027312', 'TUBO AÇO RED.', 'TUBO AÇO RED.273x4,00', 'MT', 0.00, 'EUR', 
 58.23, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 273),
('2895', '20027350', 'TUBO AÇO RED.', 'TUBO AÇO RED. 273x5,00', 'MT', 0.00, 'EUR', 
 74.30, 34.48, 34.48, '2023-01-30', '2023-01-31', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 273);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2897', '20027360', 'TUBO AÇO RED.', 'TUBO AÇO RED. 273x10,00', 'MT', 0.00, 'EUR', 
 154.98, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 273),
('2899', '20027361', 'TUBO AÇO RED.', 'TUBO AÇO RED. 273x6,00', 'MT', 0.00, 'EUR', 
 88.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 6.00, 273),
('2900', '20027380', 'TUBO AÇO RED.', 'TUBO AÇO RED. 273x8,00', 'MT', 0.00, 'EUR', 
 126.50, 69.72, 101.05, '2022-01-10', '2022-01-17', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 8.00, 273),
('2901', '20032310', 'TUBO AÇO RED.', 'TUBO AÇO RED. 323,9x10,00', 'MT', 0.00, 'EUR', 
 184.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 323.9),
('2902', '20032340', 'TUBO EST. RED.', 'TUBO EST. RED. 323,9x4,00', 'MT', 0.00, 'EUR', 
 69.41, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 323.9),
('2903', '20032350', 'TUBO EST. RED.', 'TUBO EST. RED. 323,9x5,00', 'MT', 0.00, 'EUR', 
 88.48, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 5.00, 323.9),
('2904', '20032360', 'TUBO EST. RED.', 'TUBO EST. RED. 323,9x6,00', 'ML', 0.00, 'EUR', 
 105.83, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 6.00, 323.9),
('2906', '20032380', 'TUBO EST. RED.', 'TUBO EST. RED. 323,9x8,00', 'MT', 0.00, 'EUR', 
 150.48, 68.62, 68.62, '2023-08-16', '2023-08-17', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 8.00, 323.9),
('2907', '20040660', 'TUBO AÇO RED.', 'TUBO AÇO RED. 406,4x10,00', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 406.4),
('2908', '20040680', 'TUBO AÇO RED.', 'TUBO AÇO RED. 406,4x8,00', 'ML', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 8.00, 406.4),
('2910', '20061010', 'TUBO EST. RED.', 'TUBO EST. RED.  50x4 MM', 'KG', 0.00, 'EUR', 
 10.61, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 4.00, 50),
('2912', '20061012', 'TUBO EST. RED.', 'TUBO EST. RED. 610x12,5mm', 'ML', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 12.50, 610);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3302', '23101615', 'T.AÇO GALV.RED.', 'T.AÇO GALV.RED. 16x1,50', 'MT', 2856.00, 'EUR', 
 1.46, 0.59, 0.60, '2024-06-07', '2024-06-14', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 16),
('3303', '23101915', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 19x1,50', 'MT', 0.00, 'EUR', 
 1.66, 0.49, 0.49, '2017-10-10', '2017-12-12', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 19),
('3304', '23102015', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 20x1,50', 'MT', 4026.00, 'EUR', 
 1.73, 0.70, 0.68, '2024-05-22', '2024-06-14', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 20),
('3305', '23102215', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 22x1,50', 'MT', 1716.00, 'EUR', 
 1.93, 0.97, 0.97, '2022-10-19', '2024-03-11', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 22),
('3306', '23102515', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 25x1,50', 'MT', 3240.00, 'EUR', 
 2.13, 0.87, 0.83, '2024-04-08', '2024-06-14', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 25),
('3307', '23102815', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 28x1,50', 'MT', 0.00, 'EUR', 
 2.41, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 28),
('3308', '23103015', 'T.AÇO GALV.RED.', 'T.AÇO GALV.RED. 30x1,50', 'MT', 30.00, 'EUR', 
 2.51, 1.03, 1.03, '2024-04-18', '2024-06-12', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 30),
('3309', '23103215', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 32x1,50', 'MT', 1860.00, 'EUR', 
 2.66, 1.09, 1.07, '2024-04-08', '2024-06-14', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 32),
('3310', '23103220', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 32x2,00', 'MT', 0.00, 'EUR', 
 2.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 32),
('3312', '23103315', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 33x1,50', 'MT', 0.00, 'EUR', 
 2.93, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 33),
('3313', '23103415', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 34x1,50', 'MT', 0.00, 'EUR', 
 2.93, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 34),
('3314', '23103515', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 35x1,50', 'MT', 786.00, 'EUR', 
 2.93, 1.16, 1.15, '2024-05-24', '2024-06-12', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 35),
('3315', '23103815', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 38x1,50', 'MT', 1014.00, 'EUR', 
 3.19, 1.42, 1.40, '2024-01-19', '2024-06-13', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 38),
('3316', '23104015', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 40x1,50', 'MT', 1134.00, 'EUR', 
 3.34, 1.36, 1.36, '2024-06-07', '2024-06-14', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 40),
('3317', '23104115', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 42x1,50', 'MT', 804.00, 'EUR', 
 3.51, 1.42, 1.42, '2024-06-10', '2024-06-12', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 42),
('3318', '23104515', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 45x1,50', 'MT', 468.00, 'EUR', 
 3.78, 1.67, 1.67, '2024-01-19', '2024-06-11', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 1.50, 45);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3319', '23104520', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 45x2,00', 'MT', 0.00, 'EUR', 
 3.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 2.00, 45),
('3321', '23104715', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 42,2x3,2', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2, NULL,  NULL, NULL, NULL, 3.20, 42.2),
('3322', '23104815', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 48x1,50', 'MT', 180.00, 'EUR', 
 4.04, 1.62, 1.58, '2023-12-13', '2024-06-14', 22, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 1.50, 48),
('3323', '23105015', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 50x1,50', 'MT', 1272.00, 'EUR', 
 4.21, 1.73, 1.65, '2024-05-24', '2024-06-13', 22, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 1.50, 50),
('3324', '23105515', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 55x1,50', 'MT', 0.00, 'EUR', 
 5.03, 1.52, 1.52, '2019-12-17', '2020-05-22', 22, NULL, 6, 2, 11, 2, NULL,  NULL, NULL, NULL, 1.50, 55),
('3325', '23106015', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 60x1,50', 'MT', 1002.00, 'EUR', 
 5.08, 2.03, 1.99, '2024-05-24', '2024-05-28', 22, NULL, 6, 2, 11, 2, NULL,  NULL, NULL, NULL, 1.50, 60),
('3326', '23107015', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 70x1,50', 'MT', 0.00, 'EUR', 
 5.94, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 1.50, 70),
('3327', '23107615', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 76x1,50', 'MT', 0.00, 'EUR', 
 6.63, 3.34, 5.30, '2022-11-03', '2022-11-07', 22, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 1.50, 76),
('3328', '23108915', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 89x1,50', 'MT', 0.00, 'EUR', 
 8.19, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 1.50, 89),
('3329', '23201920', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 19x2,00', 'MT', 0.00, 'EUR', 
 2.16, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 2.00, 19),
('3330', '23202020', 'T.ACO RED.GALV.', 'T.ACO RED.GALV. 20x2,00', 'ML', 0.00, 'EUR', 
 2.21, 0.94, 0.94, '2023-07-05', '2023-07-07', 22, NULL, 6, 2, 11, 2, NULL,  NULL, NULL, NULL, 2.00, 20),
('3331', '23202220', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 22x2,00', 'MT', 0.00, 'EUR', 
 2.51, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 2.00, 22),
('3332', '23202520', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 25x2,00', 'MT', 0.00, 'EUR', 
 2.73, 1.16, 1.16, '2023-07-05', '2023-09-26', 22, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 2.00, 25),
('3333', '23202820', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 28x2,00', 'MT', 0.00, 'EUR', 
 3.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2, NULL,  NULL, NULL, NULL, 2.00, 28),
('3334', '23203220', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 32x2,00', 'MT', 1314.00, 'EUR', 
 3.49, 1.36, 1.36, '2024-05-31', '2024-05-29', 22, NULL, 6, 2, 11, 2, NULL,  NULL, NULL, NULL, 2.00, 32),
('3335', '23203320', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 33x2,00', 'MT', 0.00, 'EUR', 
 3.81, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2, NULL,  NULL, NULL, NULL, 2.00, 33),
('3336', '23203420', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 34x2,00', 'MT', 0.00, 'EUR', 
 3.81, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 2.00, 34),
('3337', '23203520', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 35x2,00', 'MT', 0.00, 'EUR', 
 3.81, 1.48, 1.48, '2020-01-30', '2020-01-14', 22, NULL, 6, 2, 11, 2,  NULL, NULL, NULL, NULL, 2.00, 35),
('3338', '23203820', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 38x2,00', 'MT', 0.00, 'EUR', 
 4.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2, NULL,  NULL, NULL, NULL, 2.00, 38);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3339', '23204020', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 40x2,00', 'MT', 870.00, 'EUR', 
 4.40, 1.75, 1.72, '2024-05-31', '2024-06-13', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 40),
('3340', '23204120', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 42x2,00', 'MT', 198.00, 'EUR', 
 4.65, 1.87, 1.87, '2023-09-25', '2024-06-14', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 42),
('3341', '23204520', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 45x2,00', 'MT', 0.00, 'EUR', 
 5.01, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 45),
('3342', '23204720', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 48x2,00', 'MT', 216.00, 'EUR', 
 5.33, 2.27, 2.27, '2024-02-26', '2024-06-11', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 48),
('3343', '23205020', 'T.AÇO GALV RED.', 'T.AÇO GALV RED. 50x2,00', 'MT', 624.00, 'EUR', 
 5.55, 2.38, 2.38, '2024-02-26', '2024-06-05', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 50),
('3344', '23205520', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 55x2,00', 'MT', 0.00, 'EUR', 
 6.79, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 55),
('3345', '23206020', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 60x2,00', 'MT', 264.00, 'EUR', 
 6.71, 2.87, 2.87, '2024-03-08', '2024-06-14', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 60),
('3346', '23206030', 'T.AÇO GALV.RED.', 'T.AÇO GALV.RED. 60x3,00', 'ML', 0.00, 'EUR', 
 10.01, 4.41, 4.41, '2023-02-15', '2023-02-16', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 3.00, 60),
('3347', '23206520', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 65x2,00', 'MT', 0.00, 'EUR', 
 7.08, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 65),
('3348', '23207020', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 70x2,00', 'MT', 216.00, 'EUR', 
 7.86, 3.42, 3.39, '2024-04-08', '2024-05-17', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 70);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3352', '23208020', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 80x2,00', 'MT', 300.00, 'EUR', 
 9.03, 3.55, 3.54, '2024-05-24', '2024-06-11', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 80),
('3353', '23208920', 'T.ACO GALV.RED.', 'T.ACO GALV.RED. 89x2,00', 'MT', 240.00, 'EUR', 
 10.64, 4.68, 4.68, '2024-03-05', '2024-06-05', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 89),
('3354', '23210020', 'T.AÇO GALV. RED.', 'T.AÇO GALV. RED 100x2,00', 'MT', 246.00, 'EUR', 
 11.85, 4.85, 4.77, '2024-03-19', '2024-06-05', 22, NULL, 6, 2, 11, 2, NULL, NULL, NULL, NULL, 2.00, 100),
('3430', '23501010', 'TUBO RED.INOX.', 'TUBO RED.INOX 304 10x1,00', 'MT', 0.00, 'EUR', 
 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 10),
('3431', '23501215', 'TUBO INOX RED.', 'TUBO INOX RED.304 12x1,20', 'MT', 0.00, 'EUR', 
 2.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.20, 12),
('3432', '23501410', 'TUBO INOX RED.', 'TUBO INOX RED.304 14x1,00', 'MT', 0.00, 'EUR', 
 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 14),
('3433', '23501612', 'TUBO RED.INOX.', 'TUBO RED.INOX 304 16x1,25', 'MT', 0.00, 'EUR', 
 3.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.25, 16),
('3434', '23501615', 'TUBO INOX RED.', 'TUBO INOX RED.304 16x1,50', 'MT', 0.00, 'EUR', 
 2.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 16),
('3435', '23501915', 'TUBO INOX RED.', 'TUBO INOX RED.304 19x1,50', 'MT', 0.00, 'EUR', 
 2.59, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 19),
('3436', '23502012', 'TUBO INOX RED.', 'TUBO INOX RED.304 20x1,20', 'MT', 0.00, 'EUR', 
 2.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.20, 20),
('3437', '23502015', 'TUBO INOX RED.', 'TUBO INOX RED.304 20x1,50', 'MT', 0.00, 'EUR', 
 3.11, 2.74, 2.74, '2017-10-10', '2018-12-28', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 20),
('3438', '23502515', 'TUBO INOX RED.', 'TUBO INOX RED.304 25x1,50', 'MT', 0.00, 'EUR', 
 3.30, 2.59, 2.59, '2017-10-10', '2018-12-28', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 25),
('3440', '23502720', 'TUBO INOX RED.', 'TUBO INOX RED 304 26,9x2,60', 'UN', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 2.60, 26.9),
('3441', '23503010', 'TUBO INOX RED.', 'TUBO INOX RED 304 30x1,00', 'MT', 0.00, 'EUR', 
 3.68, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.00, 30),
('3442', '23503015', 'TUBO INOX RED.', 'TUBO INOX RED.304 30x1,50', 'MT', 0.00, 'EUR', 
 2.72, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 30),
('3443', '23503215', 'TUBO INOX RED.', 'TUBO INOX RED.304 32x1,50', 'MT', 0.00, 'EUR', 
 4.49, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 32);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3444', '23503315', 'TUBO INOX RED.', 'TUBO INOX RED.304 33x1,50', 'MT', 0.00, 'EUR', 
 3.70, 3.01, 3.01, '2017-10-10', '2018-12-28', 62, NULL, 6, 4, 11, NULL,  NULL, NULL, NULL, NULL, 1.50, 33),
('3445', '23503326', 'TUBO INOX RED.', 'TUBO INOX RED.316 33,7x1,60', 'MT', 0.00, 'EUR', 
 9.75, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL,  NULL, NULL, NULL, NULL, 1.60, 33.7),
('3446', '23503515', 'TUBO INOX RED.', 'TUBO INOX RED.304 35x1,50', 'MT', 0.00, 'EUR', 
 4.77, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL, NULL, NULL, NULL, NULL, 1.50, 35),
('3447', '23503815', 'TUBO INOX RED.', 'TUBO INOX RED.304 38x1,50', 'MT', 0.00, 'EUR', 
 3.97, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL,  NULL, NULL, NULL, NULL, 1.50, 38),
('3449', '23504015', 'TUBO INOX RED.', 'TUBO INOX RED.304 40x1,50', 'MT', 0.00, 'EUR', 
 4.38, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL, NULL, NULL, NULL, NULL, 1.50, 40),
('3450', '23504215', 'TUBO INOX RED.', 'TUBO INOX RED.304 42,4x2,00', 'MT', 0.00, 'EUR', 
 4.45, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL, NULL, NULL, NULL, NULL, 2.00, 42.4),
('3452', '23504220', 'TUBO INOX RED.', 'TUBO INOX RED 304 40x2,0', 'MT', 0.00, 'EUR', 
 11.87, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL,  NULL, NULL, NULL, NULL, 2.00, 40),
('3453', '23504315', 'TUBO INOX RED.', 'TUBO INOX RED 304 42x1,50', 'MT', 0.00, 'EUR', 
 5.32, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL, NULL, NULL, NULL, NULL, 1.50, 42),
('3454', '23504515', 'TUBO INOX RED.', 'TUBO INOX RED.304 45x1,50', 'MT', 0.00, 'EUR', 
 5.85, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL, NULL, NULL, NULL, NULL, 1.50, 45),
('3455', '23504815', 'TUBO INOX RED.', 'TUBO INOX RED.304 48,3x1,50', 'MT', 0.00, 'EUR', 
 5.97, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL, NULL, NULL, NULL, NULL, 1.50, 48.3),
('3456', '23504820', 'TUBO INOX RED.', 'TUBO INOX RED 304 48,3x2,00', 'UN', 0.00, 'EUR', 
 6.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL,  NULL, NULL, NULL, NULL, 2.00, 48.3),
('3457', '23504826', 'TUBO INOX RED.', 'TUBO INOX RED.304 48,3x2,60', 'MT', 0.00, 'EUR', 
 9.06, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL,  NULL, NULL, NULL, NULL, 2.60, 48.3),
('3458', '23504832', 'TUBO INOX RED.', 'TUBO INOX RED.304 48,3x3,20', 'MT', 0.00, 'EUR', 
 9.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL,  NULL, NULL, NULL, NULL, 3.20, 48.3),
('3459', '23505015', 'TUBO INOX RED.', 'TUBO INOX RED.304 50,8x1,50', 'MT', 0.00, 'EUR', 
 5.85, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL,  NULL, NULL, NULL, NULL, 1.50, 50.8),
('3461', '23506016', 'TUBO INOX RED.', 'TUBO INOX RED.304 60x1,50', 'MT', 0.00, 'EUR', 
 5.99, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL, NULL, NULL, NULL, NULL, 1.50, 60),
('3462', '23507615', 'TUBO INOX RED.', 'TUBO INOX RED.304 76x1,50', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL, NULL, NULL, NULL, NULL, 1.50, 76),
('3463', '23508420', 'TUBO INOX RED.', 'TUBO INOX RED.304 84x2,00', 'MT', 0.00, 'EUR', 
 10.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL, NULL, NULL, NULL, NULL, 2.00, 84),
('3465', '23510420', 'TUBO INOX RED.', 'TUBO INOX RED.304 104x2,00', 'MT', 0.00, 'EUR', 
 14.78, 12.50, 12.50, '2020-05-18', '2020-05-18', 62, NULL, 6, 4, 11, NULL,  NULL, NULL, NULL, NULL, 2.00, 104);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3576', '23663015', 'TUBO INOX RED.', 'TUBO INOX RED.316 30x15x1,5', 'MT', 0.00, 'EUR', 
 5.90, 4.91, 4.91, '2017-10-10', '2018-12-28', 62, NULL, 6, 4, 11,  NULL,NULL, NULL, NULL, NULL, 1.50, 30),
('3587', '23691815', 'TUBO INOX RED.', 'TUBO INOX RED. 316 18x1,50', 'MT', 0.00, 'EUR', 
 3.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 18),
('3588', '23691915', 'TUBO INOX RED.', 'TUBO INOX RED. 316 19x1,50', 'MT', 0.00, 'EUR', 
 4.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL,NULL, NULL, NULL, NULL, 1.50, 19),
('3589', '23692004', 'TUBO INOX RED.', 'TUBO INOX RED. 316 204x2,00', 'MT', 0.00, 'EUR', 
 48.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 2.00, 204),
('3590', '23692015', 'TUBO INOX RED.', 'TUBO INOX RED 316 20x1,50', 'MT', 0.00, 'EUR', 
 6.81, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 20),
('3592', '23692315', 'TUBO INOX RED.', 'TUBO INOX RED.316 23x1,50', 'MT', 0.00, 'EUR', 
 4.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.50, 23),
('3593', '23694816', 'TUBO INOX RED.', 'TUBO INOX RED 316 48,3x1,60', 'MT', 0.00, 'EUR', 
 7.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11, NULL, NULL, NULL, NULL, NULL, 1.60, 48.3),
('3594', '23695015', 'TUBO INOX RED.', 'TUBO INOX RED.316 50,8x1,50', 'MT', 0.00, 'EUR', 
 9.48, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 11,  NULL,NULL, NULL, NULL, NULL, 1.50, 50.8),
('4110', '31000114', 'TUBO AÇO RED.', 'TUBO AÇO RED. 114,3x6,00', 'ML', 0.00, 'EUR', 
 33.79, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11,  NULL,NULL, NULL, NULL, NULL, 6.00, 114.3),
('4115', '31000192', 'TUBO PRETO RED.', 'TUBO PRETO RED. 193,7x4,00', 'MT', 0.00, 'EUR', 
 39.56, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 1, 11, 3,  NULL,NULL, NULL, NULL, 4.00, 193.7),
('4116', '31000193', 'TUBO PRETO RED.', 'TUBO PRETO RED. 193,7x5,00', 'MT', 0.00, 'EUR', 
 49.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 1, 11, 3, NULL, NULL, NULL, NULL, 5.00, 193.7),
('4117', '31000218', 'TUBO PRETO RED.', 'TUBO PRETO RED. 219,1x4,00', 'MT', 0.00, 'EUR', 
 45.09, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 1, 11, 3,  NULL,NULL, NULL, NULL, 4.00, 219.1),
('4118', '31000219', 'TUBO PRETO RED.', 'TUBO PRETO RED. 219,1x5,00', 'MT', 0.00, 'EUR', 
 56.19, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 1, 11, 3, NULL, NULL, NULL, NULL, 5.00, 219.1),
('4119', '31000220', 'TUBO PRETO RED.', 'TUBO PRETO RED. 219,1x12mm', 'MT', 0.00, 'EUR', 
 146.38, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 1, 11, 3, NULL, NULL, NULL, NULL, 12.00, 219.1),
('4149', '33000168', 'TUBO PRETO RED.', 'TUBO PRETO RED. 168,3x3,00', 'ML', 0.00, 'EUR', 
 25.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 1, 11, 3, NULL, NULL, NULL, NULL, 3.00, 168.3),
('4151', '33000193', 'TUBO PRETO RED.', 'TUBO PRETO RED. 193,7x4,00', 'MT', 0.00, 'EUR', 
 39.56, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 1, 11, 3,  NULL,NULL, NULL, NULL, 4.00, 193.7),
('4152', '33000194', 'TUBO EST. RED.', 'TUBO EST. RED. 193,7x10', 'ML', 0.00, 'EUR', 
 106.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11,  NULL,NULL, NULL, NULL, NULL, 10.00, 193.7);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('4154', '33000219', 'TUBO PRETO RED.', 'TUBO PRETO RED. 219,1x4,00', 'MT', 0.00, 'EUR', 
 45.09, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 1, 11, 3,NULL,  NULL, NULL, NULL, 4.00, 219.1),
('4155', '33000323', 'TUBO EST. RED.', 'TUBO EST. RED. 323,9x10,00', 'ML', 0.00, 'EUR', 
 184.83, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 323.9),
('4239', '33015945', 'TUBO AÇO S/COST.RED.', 'TUBO AÇO S/COST.RED. 159x4,5', 'MT', 0.00, 'EUR', 
 24.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 41, NULL, 6, 2, 11, NULL, NULL, 10, NULL, NULL, 4.50, 159),
('4249', '33027363', 'TUBO AÇO EST.RED.', 'TUBO AÇO EST.RED. 273x12,50', 'MT', 0.00, 'EUR', 
 200.51, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 12.50, 273),
('4265', '33110150', 'TUBO EST. RED.', 'TUBO EST. RED. 101,6x10,00', 'MT', 0.00, 'EUR', 
 59.75, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 101.6),
('4270', '33113950', 'TUBO AÇO RED.', 'TUBO AÇO RED. 139,7x10', 'MT', 0.00, 'EUR', 
 74.09, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 139.7),
('4279', '33121999', 'TUBO AÇO RED.C/COST.', 'TUBO AÇO RED.C/COST. 219,1x8mm', 'KG', 0.00, 'EUR', 
 95.93, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11, NULL, 9, NULL, NULL, NULL, 8.00, 219.1),
('4280', '33124450', 'TUBO AÇO EST. RED.', 'TUBO AÇO EST. RED. 244,5x12mm', 'MT', 0.00, 'EUR', 
 171.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11,NULL,  NULL, NULL, NULL, NULL, 12.00, 244.5),
('4285', '33132350', 'TUBO AÇO RED.', 'TUBO AÇO RED. 323,9x10', 'MT', 0.00, 'EUR', 
 225.44, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 11,NULL,  NULL, NULL, NULL, NULL, 10.00, 323.9),
('4286', '33140610', 'TUBO RHS FE510 RED.', 'TUBO RHS FE510 RED. 406,4x10mm', 'MT', 0.00, 'EUR', 
 90.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 1, 11, NULL, NULL, NULL, NULL, NULL, 10.00, 406.4),
('4289', '33140663', 'TUBO AÇO RED.C/COST.', 'TUBO AÇO RED.C/COST. 457x8', 'MT', 0.00, 'EUR', 
 166.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 2, 11, NULL, 9, NULL, NULL, NULL, 8.00, 457),
('4298', '33361063', 'TUBO C/COST.RED.', 'TUBO C/COST.RED. 177,8x5,00', 'MT', 0.00, 'EUR', 
 46.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 1, 11, NULL, 9, NULL, NULL, NULL, 5.00, 177.8);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2914', '21000910', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 10x1,00', 'MT', 0.00, 'EUR', 
 0.73, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.00, 10),
('2915', '21000912', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 10x1,25', 'MT', 2922.00, 'EUR', 
 1.14, 0.39, 0.39, '2024-05-02', '2024-06-06', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.25, 10),
('2916', '21000915', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 10x1,50', 'MT', 0.00, 'EUR', 
 1.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 10),
('2917', '21001210', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 12x1,00', 'MT', 0.00, 'EUR', 
 1.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.00, 12),
('2918', '21001212', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 12x1,25', 'MT', 0.00, 'EUR', 
 1.19, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.25, 12),
('2919', '21001215', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 12x1,50', 'MT', 3840.00, 'EUR', 
 1.36, 0.56, 0.56, '2024-05-02', '2024-06-14', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 12),
('2920', '21001610', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 16x1,00', 'MT', 966.00, 'EUR', 
 1.12, 0.43, 0.47, '2019-03-06', '2023-07-12', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.00, 16),
('2921', '21001612', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 16x1,25', 'MT', 0.00, 'EUR', 
 1.26, 0.79, 0.79, '2017-10-10', '2020-12-30', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.25, 16),
('2922', '21001615', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 16x1,50', 'MT', 3894.00, 'EUR', 
 1.55, 0.64, 0.63, '2024-06-06', '2024-06-14', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 16),
('2923', '21001620', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 16x2,00', 'MT', 1680.00, 'EUR', 
 1.96, 0.79, 0.79, '2024-04-29', '2024-05-24', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 16),
('2924', '21001910', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 19x1,00', 'MT', 0.00, 'EUR', 
 1.26, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.00, 19),
('2925', '21001912', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 19x1,25', 'MT', 0.00, 'EUR', 
 1.66, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.25, 19),
('2926', '21001915', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 19x1,50', 'MT', 0.00, 'EUR', 
 1.89, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 19),
('2927', '21001920', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 19x2,00', 'MT', 0.00, 'EUR', 
 2.64, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 19),
('2928', '21002010', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 20x1,00', 'MT', 0.00, 'EUR', 
 1.26, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.00, 20),
('2929', '21002012', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 20x1,25', 'MT', 0.00, 'EUR', 
 1.66, 0.66, 0.66, '2024-01-08', '2024-01-09', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.25, 20),
('2930', '21002015', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 20x1,50', 'MT', 2904.00, 'EUR', 
 1.89, 0.76, 0.77, '2024-06-07', '2024-06-07', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 20);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2931', '21002020', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 20x2,00', 'MT', 78.00, 'EUR', 
 2.64, 1.07, 1.06, '2024-05-03', '2024-06-13', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 20),
('2932', '21002210', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 22x1,00', 'MT', 0.00, 'EUR', 
 1.49, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.00, 22),
('2933', '21002212', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 22x1,25', 'MT', 0.00, 'EUR', 
 1.97, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.25, 22),
('2934', '21002215', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 22x1,50', 'MT', 0.00, 'EUR', 
 2.48, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 22),
('2935', '21002220', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 22x2,00', 'MT', 0.00, 'EUR', 
 3.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 22),
('2936', '21002512', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 25x1,25', 'MT', 48.00, 'EUR', 
 1.97, 0.78, 0.78, '2017-10-10', '2022-09-12', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.25, 25),
('2937', '21002515', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 25x1,50', 'MT', 2532.00, 'EUR', 
 2.38, 0.94, 0.93, '2024-05-27', '2024-06-14', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 25),
('2938', '21002520', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 25x2,00', 'MT', 858.00, 'EUR', 
 3.24, 1.32, 1.30, '2024-04-18', '2024-06-04', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 25),
('2939', '21002530', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 25x3,00', 'MT', 0.00, 'EUR', 
 4.01, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 25),
('2940', '21002812', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 28x1,25', 'MT', 0.00, 'EUR', 
 2.36, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.25, 28),
('2941', '21002815', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 28x1,50', 'MT', 0.00, 'EUR', 
 2.66, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 28),
('2942', '21003010', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 30x1,00', 'MT', 0.00, 'EUR', 
 1.73, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.00, 30),
('2943', '21003012', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 30x1,25', 'MT', 0.00, 'EUR', 
 2.36, 1.01, 1.01, '2024-01-08', '2024-01-09', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.25, 30),
('2944', '21003015', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 30x1,50', 'MT', 2250.00, 'EUR', 
 2.84, 1.13, 1.16, '2024-06-07', '2024-06-14', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 30),
('2945', '21003020', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 30x2,00', 'MT', 1416.00, 'EUR', 
 3.65, 1.47, 1.46, '2024-04-18', '2024-06-14', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 30),
('2946', '21003030', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 30x3,00', 'MT', 258.00, 'EUR', 
 4.99, 1.99, 2.00, '2024-05-09', '2024-06-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 30),
('2947', '21003030', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 30x3,00', 'UN', 258.00, 'EUR', 
 4.99, 1.99, 2.00, '2024-05-09', '2024-06-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 30);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2948', '21003215', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 32x1,50', 'MT', 0.00, 'EUR', 
 3.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 32),
('2949', '21003515', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 35x1,50', 'MT', 1584.00, 'EUR', 
 3.40, 1.36, 1.33, '2024-05-24', '2024-06-14', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 35),
('2950', '21003520', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 35x2,00', 'MT', 798.00, 'EUR', 
 4.40, 1.42, 1.36, '2024-05-31', '2024-06-05', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 35),
('2951', '21003530', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 35x3,00', 'MT', 0.00, 'EUR', 
 6.00, 2.73, 2.73, '2023-06-09', '2023-06-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 35),
('2952', '21003815', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 38x1,50', 'MT', 0.00, 'EUR', 
 3.75, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 38),
('2953', '21003820', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 38x2,00', 'MT', 0.00, 'EUR', 
 4.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 38),
('2954', '21003830', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 38x3,00', 'MT', 0.00, 'EUR', 
 6.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 38),
('2955', '21004015', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 40x1,50', 'MT', 4764.00, 'EUR', 
 3.75, 1.49, 1.47, '2024-05-28', '2024-06-13', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 40),
('2956', '21004020', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 40x2,00', 'MT', 300.00, 'EUR', 
 4.90, 2.06, 2.06, '2024-03-15', '2024-06-14', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 40),
('2957', '21004030', 'TUBO AÇO QUAD.', 'TUBO AÇO QUAD. 40x3,00', 'MT', 444.00, 'EUR', 
 6.43, 2.71, 2.65, '2024-03-15', '2024-06-14', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 40),
('2958', '21004040', 'TUBO AÇO QUAD.', 'TUBO AÇO QUAD. 40x4,00', 'MT', 228.00, 'EUR', 
 8.19, 3.36, 3.28, '2024-04-08', '2024-06-05', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 40),
('2959', '21004515', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 45x1,50', 'MT', 594.00, 'EUR', 
 4.56, 1.83, 1.83, '2024-04-26', '2024-05-29', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 45),
('2960', '21004520', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 45x2,00', 'MT', 0.00, 'EUR', 
 5.79, 0.00, 0.00, NULL, NULL, 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 45),
('2961', '21004530', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 45x3,00', 'MT', 0.00, 'EUR', 
 7.98, 2.47, 2.47, '2019-11-25', '2019-11-25', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 45),
('2962', '21005015', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 50x1,50', 'MT', 1602.00, 'EUR', 
 5.08, 2.03, 1.99, '2024-05-24', '2024-06-12', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 50),
('2963', '21005020', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 50x2,00', 'MT', 1266.00, 'EUR', 
 6.49, 2.65, 2.59, '2024-04-26', '2024-06-13', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 50),
('2964', '21005030', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 50x3,00', 'MT', 1356.00, 'EUR', 
 8.28, 3.34, 3.31, '2024-06-07', '2024-06-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 50),
('2965', '21005032', 'TUBO AÇO QUAD.', 'TUBO AÇO QUAD. 50x6,00', 'MT', 0.00, 'EUR', 
 15.99, 5.15, 5.15, '2019-05-13', '2019-05-15', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 50);








INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2966', '21005040', 'TUBO ACO.QUAD.', 'TUBO ACO.QUAD. 50x4,00', 'MT', 222.00, 'EUR', 
 10.61, 4.16, 4.16, '2024-05-28', '2024-06-05', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 50),
('2967', '21005050', 'TUBO ACO.QUAD.', 'TUBO ACO.QUAD. 50x5,00', 'MT', 222.00, 'EUR', 
 13.33, 5.43, 5.44, '2024-06-10', '2024-05-31', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 50),
('2968', '21006015', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 60x1,50', 'MT', 1920.00, 'EUR', 
 6.20, 2.55, 2.43, '2024-05-27', '2024-06-12', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 60),
('2969', '21006020', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 60x2,00', 'MT', 1566.00, 'EUR', 
 7.89, 3.28, 3.09, '2024-05-28', '2024-06-11', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 60),
('2970', '21006030', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 60x3,00', 'MT', 588.00, 'EUR', 
 10.10, 4.06, 3.96, '2024-05-22', '2024-06-14', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 60),
('2971', '21006040', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 60x4,00', 'MT', 66.00, 'EUR', 
 13.06, 5.30, 5.23, '2024-04-26', '2024-06-07', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 60),
('2972', '21006050', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 60x5,00', 'MT', 6.00, 'EUR', 
 16.08, 4.10, 0.00, NULL, NULL, 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 60),
('2973', '21006060', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 60x6,00', 'MT', 84.00, 'EUR', 
 19.46, 7.99, 7.94, '2024-04-08', '2024-06-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 60),
('2974', '21006080', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 60x8,00', 'MT', 0.00, 'EUR', 
 25.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 60),
('2975', '21007006', 'TUBO ACO.QUAD.', 'TUBO ACO.QUAD. 70x6,00', 'MT', 0.00, 'EUR', 
 23.28, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 70),
('2976', '21007008', 'TUBO ACO.QUAD.', 'TUBO ACO.QUAD. 70x8,00', 'MT', 0.00, 'EUR', 
 33.86, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 70),
('2977', '21007015', 'TUBO ACO.QUAD.', 'TUBO ACO.QUAD. 70x1,50', 'MT', 0.00, 'EUR', 
 7.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 70),
('2978', '21007020', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 70x2,00', 'MT', 162.00, 'EUR', 
 9.26, 3.71, 3.71, '2024-05-09', '2024-06-12', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 70),
('2979', '21007030', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 70x3,00', 'MT', 372.00, 'EUR', 
 11.95, 4.68, 4.68, '2024-05-24', '2024-06-14', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 70);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2980', '21007040', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 70x4,00', 'MT', 0.00, 'EUR', 
 15.51, 9.13, 9.13, '2022-01-20', '2022-01-21', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 70),
('2981', '21007050', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 70x5,00', 'MT', 0.00, 'EUR', 
 19.70, 5.95, 6.28, '2019-09-27', '2019-09-30', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 70),
('2982', '21007060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 70x6,00', 'ML', 0.00, 'EUR', 
 23.28, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 70),
('2983', '21007060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 70x6,00', 'MT', 0.00, 'EUR', 
 23.28, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 70),
('2984', '21008020', 'TUBO ACO QUAD.', 'TUBO ACO QUAD. 80x2,00', 'MT', 0.00, 'EUR', 
 10.31, 4.13, 4.13, '2024-04-09', '2024-06-14', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 80),
('2985', '21008030', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 80x3,00', 'MT', 390.00, 'EUR', 
 13.78, 5.51, 5.51, '2024-04-26', '2024-06-14', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 80),
('2986', '21008040', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 80x4,00', 'MT', 240.00, 'EUR', 
 17.94, 7.25, 7.18, '2024-05-10', '2024-06-11', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 80),
('2987', '21008050', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 80x5,00', 'MT', 120.00, 'EUR', 
 22.94, 9.36, 9.36, '2024-06-06', '2024-05-29', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 80),
('2988', '21008060', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 80x6,00', 'MT', 0.00, 'EUR', 
 26.83, 7.60, 13.58, '2020-06-30', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 80),
('2989', '21008080', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 80x8,00', 'MT', 0.00, 'EUR', 
 37.89, 13.19, 13.19, '2019-09-18', '2019-09-19', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 80),
('2990', '21009030', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 90x3,00', 'MT', 174.00, 'EUR', 
 16.04, 6.53, 6.54, '2024-06-07', '2024-06-03', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 90),
('2991', '21009045', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 90x4,00', 'MT', 0.00, 'EUR', 
 21.04, 12.38, 12.38, '2022-01-20', '2022-01-21', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 90),
('2992', '21009050', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 90x5,00', 'MT', 0.00, 'EUR', 
 25.68, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 90),
('2993', '21009060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 90x6,00', 'MT', 0.00, 'EUR', 
 31.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 90),
('2994', '21010010', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 100x10', 'MT', 0.00, 'EUR', 
 59.74, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 10.00, 100),
('2995', '21010020', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 100x2,00', 'MT', 360.00, 'EUR', 
 13.13, 5.27, 5.15, '2024-05-28', '2024-06-14', 21, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 100),
('2996', '21010030', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 100x3,00', 'MT', 552.00, 'EUR', 
 17.45, 7.01, 6.84, '2024-05-22', '2024-06-11', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 100);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2997', '21010040', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 100x4,00', 'MT', 336.00, 'EUR', 
 22.78, 9.26, 9.11, '2024-04-09', '2024-06-07', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 100),
('2998', '21010050', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 100x5,00', 'MT', 0.00, 'EUR', 
 29.25, 9.34, 9.34, '2018-12-19', '2019-01-08', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 100),
('2999', '21010060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 100x6,00', 'MT', 12.00, 'EUR', 
 34.63, 14.13, 14.13, '2024-05-24', '2024-06-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 100),
('3000', '21010080', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 100x8,00', 'MT', 0.00, 'EUR', 
 47.35, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 100),
('3001', '21010091', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 100x10,00', 'MT', 0.00, 'EUR', 
 59.90, 26.28, 26.28, '2023-09-29', '2023-10-02', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 10.00, 100),
('3002', '21012030', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 120x3,00', 'MT', 372.00, 'EUR', 
 21.85, 8.75, 8.57, '2024-05-22', '2024-06-14', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 120),
('3003', '21012040', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 120x4,00', 'MT', 462.00, 'EUR', 
 28.48, 11.66, 11.62, '2024-06-06', '2024-05-22', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 120),
('3004', '21012050', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 120x5,00', 'MT', 0.00, 'EUR', 
 35.55, 11.70, 11.70, '2019-09-09', '2019-09-10', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 120),
('3005', '21012060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 120x6,00', 'MT', 0.00, 'EUR', 
 42.56, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 120),
('3006', '21012080', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 120x8,00', 'MT', 6.00, 'EUR', 
 59.65, 24.10, 24.10, '2023-10-06', '2023-12-04', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 120),
('3007', '21012090', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 120x10,00', 'ML', 0.00, 'EUR', 
 73.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 10.00, 120),
('3008', '21012090', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 120x10,00', 'MT', 0.00, 'EUR', 
 73.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 10.00, 120);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3009', '21012540', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 125x4,00', 'MT', 0.00, 'EUR', 
 31.91, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 125),
('3010', '21014030', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 140x3,00', 'MT', 6.00, 'EUR', 
 26.50, 10.60, 10.60, '2024-05-10', '2024-06-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 140),
('3011', '21014040', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 140x4,00', 'MT', 126.00, 'EUR', 
 34.19, 13.58, 13.40, '2024-05-22', '2024-06-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 140),
('3012', '21014050', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 140x5,00', 'MT', 0.00, 'EUR', 
 42.19, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 140),
('3013', '21014060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 140x6,00', 'MT', 0.00, 'EUR', 
 50.68, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 140),
('3014', '21014080', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 140x8,00', 'MT', 0.00, 'EUR', 
 70.69, 34.61, 34.61, '2023-06-14', '2023-06-16', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 140),
('3015', '21015010', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 150x10,00', 'MT', 0.00, 'EUR', 
 95.79, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 10.00, 150),
('3016', '21015030', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 150x3,00', 'MT', 138.00, 'EUR', 
 28.58, 11.49, 11.20, '2024-05-22', '2024-06-14', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 150),
('3017', '21015040', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 150x4,00', 'MT', 198.00, 'EUR', 
 37.35, 15.30, 15.24, '2024-04-19', '2024-06-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 150),
('3018', '21015050', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 150x5,00', 'MT', 78.00, 'EUR', 
 45.45, 18.54, 18.54, '2024-05-21', '2024-06-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 150),
('3019', '21015060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 150x6,00', 'MT', 0.00, 'EUR', 
 53.88, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 150),
('3020', '21015080', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 150x8,00', 'MT', 0.00, 'EUR', 
 76.31, 25.04, 25.04, '2019-01-17', '2019-01-22', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 150),
('3021', '21016040', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 160x4,00', 'MT', 228.00, 'EUR', 
 40.55, 16.30, 15.90, '2024-05-22', '2024-05-14', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 160),
('3022', '21016050', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 160x5,00', 'MT', 0.00, 'EUR', 
 49.68, 16.35, 16.35, '2017-10-10', '2019-05-15', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 160);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3023', '21016060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 160x6,00', 'MT', 0.00, 'EUR', 
 59.76, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 160),
('3024', '21016061', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 160x8,00', 'MT', 0.00, 'EUR', 
 82.46, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 160),
('3025', '21016062', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 160x10,00', 'MT', 0.00, 'EUR', 
 103.71, 33.54, 33.54, '2019-02-22', '2019-02-26', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 10.00, 160),
('3026', '21016080', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 160x8,00', 'MT', 0.00, 'EUR', 
 82.46, 40.68, 40.68, '2021-04-26', '2021-04-26', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 160),
('3027', '21016092', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 160x12,50', 'MT', 0.00, 'EUR', 
 125.68, 60.67, 71.38, '2021-04-26', '2021-04-26', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 12.50, 160),
('3028', '21016096', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 160x16,00', 'MT', 0.00, 'EUR', 
 0.00, 102.94, 102.94, '2021-04-21', '2021-04-26', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 16.00, 160),
('3029', '21018010', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 180x6,00', 'ML', 0.00, 'EUR', 
 67.44, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 180),
('3030', '21018010', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 180x6,00', 'MT', 0.00, 'EUR', 
 67.44, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 180),
('3031', '21018030', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 180x3,00', 'ML', 0.00, 'EUR', 
 36.25, 11.96, 11.96, '2019-09-09', '2019-09-10', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 180),
('3032', '21018050', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 180x180x4', 'ML', 54.00, 'EUR', 
 46.71, 19.19, 19.06, '2024-05-24', '2024-06-03', 23, NULL, 6, 2, 2, NULL, NULL,  NULL, NULL, NULL, 4.00, 180),
('3033', '21018051', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 180x5,00', 'MT', 0.00, 'EUR', 
 56.43, 17.43, 28.58, '2020-05-07', '2020-05-11', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 180),
('3034', '21020010', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 200x200x10', 'MT', 0.00, 'EUR', 
 131.74, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, 200, 200, NULL, 10.00, 200),
('3035', '21020030', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 200x3,00', 'MT', 0.00, 'EUR', 
 52.05, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 3.00, 200),
('3036', '21020040', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 200x4,00', 'MT', 96.00, 'EUR', 
 52.05, 21.05, 20.82, '2024-04-29', '2024-05-23', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 200);




    
WITH cte AS (
    SELECT 
        MIN(id) AS id
    FROM 
        mf_product_catalog
    WHERE 
        type_id = 6 AND shape_id = 2
    GROUP BY 
        product_code
)
DELETE FROM mf_product_catalog
WHERE id NOT IN (SELECT id FROM cte) AND type_id = 6 AND shape_id = 2;


    
WITH cte AS (
    SELECT 
        MIN(id) AS id
    FROM 
        t_product_catalog
    WHERE 
        type_id = 6 AND shape_id = 2
    GROUP BY 
        product_code
)
DELETE FROM t_product_catalog
WHERE id NOT IN (SELECT id FROM cte) AND type_id = 6 AND shape_id = 2;






INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3037', '21020050', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 200x5,00', 'MT', 18.00, 'EUR', 
 63.90, 25.56, 25.56, '2024-04-29', '2024-05-13', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 200),
('3038', '21020060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 200x6,00', 'MT', 0.00, 'EUR', 
 76.69, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 200),
('3039', '21020080', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 200x8,00', 'MT', 0.00, 'EUR', 
 105.06, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 200),
('3040', '21022040', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 220x4,00', 'ML', 0.00, 'EUR', 
 60.51, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 220),
('3041', '21022040', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 220x4,00', 'MT', 0.00, 'EUR', 
 60.51, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 4.00, 220),
('3042', '21022080', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 220x8,00', 'MT', 0.00, 'EUR', 
 121.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 8.00, 220),
('3043', '21025050', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 250x5,00', 'MT', 0.00, 'EUR', 
 85.55, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 5.00, 250),
('3044', '21025060', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 250x6,00', 'MT', 0.00, 'EUR', 
 101.78, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 6.00, 250),
('3045', '21025070', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 250x10,00', 'ML', 0.00, 'EUR', 
 163.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 10.00, 250),
('3046', '21025070', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 250x10,00', 'MT', 0.00, 'EUR', 
 163.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL, NULL, NULL, NULL, 10.00, 250);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3047', '21025080', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 250x250x6,00', 'ML', 0.00, 'EUR', 
 101.78, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL,250, 250, NULL, 6.00, 250),
('3048', '21025080', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 250x250x6,00', 'MT', 0.00, 'EUR', 
 101.78, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL,250, 250, NULL, 6.00, 250),
('3049', '21026060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 260x260x6,00', 'ML', 0.00, 'EUR', 
 109.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL,260, 260, NULL, 6.00, 260),
('3050', '21026060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 260x260x6,00', 'MT', 0.00, 'EUR', 
 109.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL,260, 260, NULL, 6.00, 260),
('3051', '21026080', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 250x250x8,00', 'ML', 0.00, 'EUR', 
 136.28, 44.75, 44.75, '2018-07-16', '2018-07-17', 23, NULL, 6, 2, 2, NULL, NULL,250, 250, NULL, 8.00, 250),
('3052', '21030060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 300x6,00', 'ML', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL,NULL, 300, NULL, NULL, 6.00, 300),
('3053', '21030060', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 300x6,00', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL,300, NULL, NULL, 6.00, 300),
('3054', '21030080', 'TUBO EST. QUAD.', 'TUBO EST. QUAD. 300x8,00', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL,300, NULL, NULL, 8.00, 300),
('3056', '21212010', 'TUBO EST.QUAD.', 'TUBO EST.QUAD. 120x8,00', 'MT', 0.00, 'EUR', 
 59.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 2, NULL, NULL,120, NULL, NULL, 8.00, 120);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3355', '23301615', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 16x1,50', 'MT', 5406.00, 'EUR', 
 1.70, 0.68, 0.67, '2024-05-24', '2024-06-14', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 1.50, 16),
('3356', '23302015', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 20x1,50', 'MT', 2340.00, 'EUR', 
 2.04, 0.81, 0.80, '2024-05-14', '2024-06-14', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 1.50, 20),
('3357', '23302020', 'T.AÇO GALV.QUAD.', 'T.AÇO GALV.QUAD. 20x2,00', 'MT', 1038.00, 'EUR', 
 2.66, 1.09, 1.09, '2023-10-20', '2024-06-07', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 2.00, 20),
('3358', '23302512', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 25x1,25', 'MT', 0.00, 'EUR', 
 2.21, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 1.25, 25),
('3359', '23302515', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 25x1,50', 'MT', 4764.00, 'EUR', 
 2.56, 1.06, 1.01, '2024-05-22', '2024-06-13', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 1.50, 25),
('3360', '23302520', 'T.AÇO GALV.QUAD.', 'T.AÇO GALV.QUAD. 25x2,00', 'ML', 786.00, 'EUR', 
 3.39, 1.50, 1.49, '2024-03-05', '2024-06-13', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 2.00, 25),
('3361', '23303015', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 30x1,50', 'MT', 8790.00, 'EUR', 
 3.09, 1.24, 1.28, '2024-05-31', '2024-06-14', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 1.50, 30),
('3362', '23303020', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 30x2,00', 'MT', 2970.00, 'EUR', 
 4.08, 1.64, 1.60, '2024-05-22', '2024-06-13', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 2.00, 30),
('3363', '23303030', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 30x3,00', 'MT', 0.00, 'EUR', 
 5.65, 3.11, 3.11, '2022-02-15', '2023-12-13', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 3.00, 30),
('3364', '23303515', 'T.AÇO GALV.QUAD.', 'T.AÇO GALV.QUAD. 35x1,50', 'MT', 738.00, 'EUR', 
 3.69, 1.49, 1.48, '2024-05-10', '2024-06-14', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 1.50, 35),
('3365', '23303520', 'T.ACO GALV. QUAD.', 'T.ACO GALV. QUAD. 35x2,00', 'M', 0.00, 'EUR', 
 4.89, 3.28, 3.28, '2021-09-10', '2022-06-03', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 2.00, 35),
('3366', '23303520', 'T.ACO GALV. QUAD.', 'T.ACO GALV. QUAD. 35x2,00', 'MT', 0.00, 'EUR', 
 4.89, 3.28, 3.28, '2021-09-10', '2022-06-03', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 2.00, 35),
('3367', '23304015', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 40x1,50', 'MT', 2298.00, 'EUR', 
 4.18, 1.67, 1.64, '2024-05-22', '2024-06-14', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 1.50, 40),
('3368', '23304020', 'T.ACO GALV. QUAD.', 'T.ACO GALV. QUAD. 40x2,00', 'ML', 3162.00, 'EUR', 
 5.49, 2.21, 2.22, '2024-05-10', '2024-06-14', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 2.00, 40),
('3369', '23304515', 'T.AÇO GALV.QUAD.', 'T.AÇO GALV.QUAD. 45x1,50', 'MT', 384.00, 'EUR', 
 4.95, 2.00, 1.98, '2024-04-08', '2024-06-11', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 1.50, 45),
('3370', '23304520', 'T.ACO GALV. QUAD.', 'T.ACO GALV. QUAD. 45x2,00', 'MT', 666.00, 'EUR', 
 6.75, 2.70, 2.70, '2024-05-10', '2024-06-03', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL,  NULL,2.00, 45);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3371', '23305015', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 50x1,50', 'MT', 1332.00, 'EUR', 
 5.26, 2.24, 2.24, '2024-02-27', '2024-06-14', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 1.50, 50),
('3372', '23305020', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 50x2,00', 'MT', 2106.00, 'EUR', 
 6.94, 2.76, 2.72, '2024-05-24', '2024-06-13', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 2.00, 50),
('3373', '23305030', 'T. AÇO GALV.QUAD.', 'T. AÇO GALV.QUAD. 50x3,00', 'ML', 174.00, 'EUR', 
 10.05, 4.10, 4.10, '2024-06-06', '2024-06-12', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 3.00, 50),
('3374', '23306015', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 60x1,50', 'MT', 876.00, 'EUR', 
 6.35, 2.59, 2.53, '2024-05-14', '2024-06-14', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 1.50, 60),
('3375', '23306020', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 60x2,00', 'MT', 1482.00, 'EUR', 
 8.39, 3.43, 3.35, '2024-05-14', '2024-06-14', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 2.00, 60),
('3376', '23306030', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 60x3,00', 'MT', 132.00, 'EUR', 
 12.28, 4.93, 4.93, '2023-11-21', '2024-06-12', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 3.00, 60),
('3377', '23308020', 'T.AÇO GALV.QUAD.', 'T.AÇO GALV.QUAD. 80x2,00', 'MT', 1620.00, 'EUR', 
 11.39, 4.53, 4.60, '2024-05-31', '2024-06-14', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 2.00, 80),
('3378', '23308030', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 80x3,00', 'MT', 300.00, 'EUR', 
 16.73, 6.87, 6.76, '2024-05-10', '2024-06-11', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 3.00, 80),
('3379', '23310020', 'T.AÇO GALV.QUAD.', 'T.AÇO GALV.QUAD. 100x2,00', 'MT', 1182.00, 'EUR', 
 14.43, 5.77, 5.66, '2024-05-24', '2024-06-14', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 2.00, 100),
('3380', '23310030', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 100x3,00', 'MT', 240.00, 'EUR', 
 21.20, 8.59, 8.31, '2024-05-22', '2024-06-11', 22, NULL, 6, 2, 2, 2, NULL, NULL, NULL, NULL, 3.00, 100),
('3381', '23312030', 'T.ACO GALV.QUAD.', 'T.ACO GALV.QUAD. 120x3,00', 'MT', 36.00, 'EUR', 
 26.13, 10.67, 10.66, '2024-04-23', '2024-06-13', 22, NULL, 6, 2, 2, 2,  NULL,NULL, NULL, NULL, 3.00, 120);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3466', '23511010', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 10x1,00', 'MT', 0.00, 'EUR', 
 1.92, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 2, NULL, NULL, NULL, NULL, NULL, 1.00, 10),
('3467', '23511215', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 12x1,20', 'MT', 0.00, 'EUR', 
 1.75, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 2, NULL,  NULL,NULL, NULL, NULL, 1.20, 12),
('3468', '23511515', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 15x1,25', 'MT', 0.00, 'EUR', 
 2.62, 2.73, 2.73, '2017-10-10', '2018-12-28', 62, NULL, 6, 4, 2, NULL, NULL, NULL, NULL, NULL, 1.25, 15),
('3469', '23512012', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 20x1,50', 'MT', 0.00, 'EUR', 
 2.41, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 2, NULL,  NULL,NULL, NULL, NULL, 1.50, 20),
('3470', '23512515', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 25x1,50', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 2, NULL,  NULL,NULL, NULL, NULL, 1.50, 25),
('3472', '23513015', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 30x1,50', 'MT', 0.00, 'EUR', 
 4.11, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 2, NULL,  NULL,NULL, NULL, NULL, 1.50, 30),
('3473', '23514015', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 40x1,50', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 2, NULL,  NULL,NULL, NULL, NULL, 1.50, 40),
('3475', '23515015', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 50x1,50', 'MT', 0.00, 'EUR', 
 5.89, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 2, NULL,  NULL,NULL, NULL, NULL, 1.50, 50),
('3478', '23516015', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 60x1,50', 'MT', 0.00, 'EUR', 
 12.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 2, NULL, NULL, NULL, NULL, NULL, 1.50, 60),
('3480', '23518020', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 80x2,00', 'MT', 0.00, 'EUR', 
 12.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 2, NULL, NULL, NULL, NULL, NULL, 2.00, 80),
('3481', '23519930', 'TUBO INOX QUAD.304', 'TUBO INOX QUAD.304 100x2,00', 'MT', 0.00, 'EUR', 
 27.85, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 2, NULL,  NULL,NULL, NULL, NULL, 2.00, 100);






UPDATE
    t_product_catalog
SET
    width = diameter,
    height = diameter,
    length = 6000
WHERE
    type_id = 6
    AND shape_id = 2;


UPDATE
    t_product_catalog
SET
    length = 6000
WHERE
    type_id = 6;





-- TUBOS RETANGULARES



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('2483', '19999006', 'TERMINAL P/TUBO.RECT.', 'TERMINAL P/TUBO.RECT.80x40', 'UN', 178.00, 'EUR', 
 0.60, 0.11, 0.11, '2017-10-10', '2021-04-14', 15, NULL, 6, 1, 1, NULL, NULL, NULL, 80, 40, NULL, NULL),
('3059', '22140805', 'TUBO RECT. EST.', 'TUBO RECT. EST.140x80x5,00', 'MT', 0.00, 'EUR', 
 33.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 1, 1, NULL, NULL, NULL, 140, 80, 5.00, NULL),
('3060', '22140810', 'TUBO RECT. EST.', 'TUBO RECT. EST. 140x80x10mm', 'MT', 0.00, 'EUR', 
 69.31, 28.51, 28.51, '2017-10-10', NULL, 23, NULL, 6, 1, 1, NULL, NULL, NULL, 140, 80, 10.00, NULL),
('3061', '22151015', 'TUBO ACO RECT.', 'TUBO ACO RECT. 15x10x1,50', 'MT', 0.00, 'EUR', 
 1.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 15, 10, 1.50, NULL),
('3062', '22201015', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 20x10x1,50', 'MT', 3702.00, 'EUR', 
 1.61, 0.65, 0.65, '2024-04-29', '2024-06-06', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 20, 10, 1.50, NULL),
('3063', '22201515', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 20x15x1,50', 'MT', 0.00, 'EUR', 
 2.01, 1.14, 1.14, '2021-05-06', '2022-05-19', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 20, 15, 1.50, NULL),
('3064', '22221210', 'TUBO ACO RECT.', 'TUBO ACO RECT. 21x12,5x1,00', 'MT', 0.00, 'EUR', 
 0.99, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 21, 12.5, 1.00, NULL),
('3065', '22221212', 'TUBO ACO RECT.', 'TUBO ACO RECT. 21x12,5x1,25', 'MT', 0.00, 'EUR', 
 1.09, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 21, 12.5, 1.25, NULL),
('3066', '22221215', 'TUBO ACO RECT.', 'TUBO ACO RECT. 21x12,5x1,50', 'MT', 0.00, 'EUR', 
 2.01, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 21, 12.5, 1.50, NULL),
('3067', '22251015', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 25x10x1,50', 'MT', 0.00, 'EUR', 
 1.90, 0.98, 1.51, '2022-01-25', '2022-01-27', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 25, 10, 1.50, NULL),
('3068', '22251515', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 25x15x1,50', 'MT', 0.00, 'EUR', 
 2.01, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 25, 15, 1.50, NULL),
('3069', '22252015', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 25x20x1,50', 'MT', 0.00, 'EUR', 
 2.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 25, 20, 1.50, NULL),
('3070', '22301015', 'TUBO ACO RECT.', 'TUBO ACO RECT. 30x10x1,50', 'MT', 1890.00, 'EUR', 
 2.14, 0.86, 0.86, '2024-05-03', '2024-06-12', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 30, 10, 1.50, NULL),
('3071', '22301515', 'TUBO ACO RECT.', 'TUBO ACO RECT. 30x15x1,50', 'MT', 1500.00, 'EUR', 
 2.29, 0.94, 0.93, '2024-06-07', '2024-06-06', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 30, 15, 1.50, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3072', '22302010', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 30x20x1,00', 'MT', 0.00, 'EUR', 
 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 30, 20, 1.00, NULL),
('3073', '22302015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 30x20x1,50', 'MT', 1170.00, 'EUR', 
 2.40, 0.95, 0.94, '2024-05-28', '2024-06-13', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 30, 20, 1.50, NULL),
('3074', '22302020', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 30x20x2,00', 'MT', 630.00, 'EUR', 
 3.06, 1.32, 1.32, '2024-01-18', '2024-06-11', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 30, 20, 2.00, NULL),
('3075', '22321312', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 32x13x1,25', 'MT', 0.00, 'EUR', 
 1.73, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 32, 13, 1.25, NULL),
('3076', '22321315', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 32x13x1,50', 'MT', 0.00, 'EUR', 
 2.29, 0.20, 0.20, '2022-12-12', '2021-03-30', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 32, 13, 1.50, NULL),
('3077', '22321320', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 32x13x2,00', 'MT', 0.00, 'EUR', 
 2.93, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 32, 13, 2.00, NULL),
('3078', '22351515', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 35x15x1,50', 'ML', 0.00, 'EUR', 
 2.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 35, 15, 1.50, NULL),
('3080', '22352008', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 35x20x0,80', 'MT', 0.00, 'EUR', 
 1.39, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 35, 20, 0.80, NULL),
('3081', '22352010', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 35x20x1,00', 'MT', 0.00, 'EUR', 
 1.85, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 35, 20, 1.00, NULL),
('3082', '22352012', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 35x20x1,25', 'MT', 0.00, 'EUR', 
 2.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 35, 20, 1.25, NULL),
('3083', '22352015', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 35x20x1,50', 'MT', 774.00, 'EUR', 
 2.95, 1.42, 1.42, '2022-06-02', '2024-05-23', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 35, 20, 1.50, NULL),
('3084', '22352020', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 35x20x2,00', 'MT', 0.00, 'EUR', 
 3.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 35, 20, 2.00, NULL),
('3085', '22352510', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 35x25x1,00', 'MT', 0.00, 'EUR', 
 1.79, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 35, 25, 1.00, NULL);





    
WITH cte AS (
    SELECT 
        MIN(id) AS id
    FROM 
        mf_product_catalog
    WHERE 
        type_id = 6 AND shape_id = 1
    GROUP BY 
        product_code
)
DELETE FROM mf_product_catalog
WHERE id NOT IN (SELECT id FROM cte) AND type_id = 6 AND shape_id = 1;




WITH cte AS (
    SELECT 
        MIN(id) AS id
    FROM 
        t_product_catalog
    WHERE 
        type_id = 6 AND shape_id = 1
    GROUP BY 
        product_code
)
DELETE FROM t_product_catalog
WHERE id NOT IN (SELECT id FROM cte) AND type_id = 6 AND shape_id = 1;




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3086', '22352515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 35x25x1,50', 'MT', 0.00, 'EUR', 
 3.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 35, 25, 1.50, NULL),
('3087', '22401015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x10x1,50', 'MT', 1188.00, 'EUR', 
 2.60, 1.05, 1.06, '2024-05-24', '2024-06-14', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 10, 1.50, NULL),
('3088', '22401515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x15x1,50', 'MT', 972.00, 'EUR', 
 2.79, 1.12, 1.12, '2024-05-31', '2024-06-07', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 15, 1.50, NULL),
('3089', '22402012', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 40x20x1,25', 'MT', 0.00, 'EUR', 
 2.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 20, 1.25, NULL),
('3090', '22402015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x20x1,50', 'MT', 3600.00, 'EUR', 
 2.85, 1.15, 1.12, '2024-05-28', '2024-06-14', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 20, 1.50, NULL),
('3091', '22402020', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x20x2,00', 'MT', 1188.00, 'EUR', 
 3.70, 1.52, 1.51, '2024-06-07', '2024-06-12', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 20, 2.00, NULL),
('3092', '22402025', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x20x3,00', 'MT', 0.00, 'EUR', 
 4.93, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 20, 3.00, NULL),
('3093', '22402515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x25x1,50', 'MT', 0.00, 'EUR', 
 3.38, 0.00, 0.00, NULL, NULL, 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 25, 1.50, NULL),
('3094', '22402525', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x25x2,50', 'MT', 0.00, 'EUR', 
 4.28, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 25, 2.50, NULL),
('3095', '22402530', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x25x3,00', 'MT', 0.00, 'EUR', 
 5.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 25, 3.00, NULL),
('3096', '22402715', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x27x1,50', 'MT', 0.00, 'EUR', 
 3.41, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 27, 1.50, NULL),
('3097', '22403012', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x30x1,25', 'MT', 0.00, 'EUR', 
 2.66, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 30, 1.25, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3098', '22403015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x30x1,50', 'MT', 924.00, 'EUR', 
 3.41, 1.48, 1.45, '2024-02-09', '2024-06-14', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 30, 1.50, NULL),
('3099', '22403020', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x30x2,00', 'MT', 738.00, 'EUR', 
 4.34, 1.77, 1.77, '2024-06-07', '2024-05-27', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 30, 2.00, NULL),
('3100', '22452515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 45x25x1,50', 'MT', 0.00, 'EUR', 
 3.63, 2.55, 2.90, '2022-04-21', '2022-09-21', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 45, 25, 1.50, NULL),
('3101', '22452520', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 45x25x2,00', 'MT', 0.00, 'EUR', 
 4.64, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 45, 25, 2.00, NULL),
('3102', '22452530', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 40x30x3,00', 'MT', 0.00, 'EUR', 
 5.66, 1.45, 1.45, '2017-10-10', '2019-05-17', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 40, 30, 3.00, NULL),
('3103', '22500300', 'TUBO EST. RECT.', 'TUBO EST. RECT. 500x300x10', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 500, 300, 10.00, NULL),
('3104', '22501015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x10x1,50', 'MT', 1404.00, 'EUR', 
 3.09, 1.26, 1.26, '2024-06-07', '2024-06-13', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 10, 1.50, NULL),
('3105', '22501415', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x14x1,50', 'MT', 0.00, 'EUR', 
 3.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 14, 1.50, NULL),
('3106', '22501515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x15x1,50', 'MT', 0.00, 'EUR', 
 3.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 15, 1.50, NULL),
('3107', '22502012', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x20x1,25', 'MT', 0.00, 'EUR', 
 2.66, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 20, 1.25, NULL),
('3108', '22502015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x20x1,50', 'MT', 768.00, 'EUR', 
 3.44, 1.39, 1.38, '2024-04-26', '2024-06-13', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 20, 1.50, NULL),
('3109', '22502020', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x20x2,00', 'MT', 246.00, 'EUR', 
 4.38, 1.75, 1.77, '2023-11-13', '2024-06-07', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 20, 2.00, NULL),
('3110', '22502512', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x25x1,25', 'MT', 0.00, 'EUR', 
 2.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 25, 1.25, NULL),
('3111', '22502515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x25x1,50', 'MT', 0.00, 'EUR', 
 3.74, 1.68, 1.55, '2024-03-07', '2024-06-12', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 25, 1.50, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3112', '22502520', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x25x2,00', 'MT', 0.00, 'EUR', 
 4.78, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 25, 2.00, NULL),
('3113', '22503012', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x30x1,25', 'MT', 0.00, 'EUR', 
 2.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 30, 1.25, NULL),
('3114', '22503015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x30x1,50', 'MT', 5406.00, 'EUR', 
 3.83, 1.52, 1.50, '2024-05-27', '2024-06-14', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 30, 1.50, NULL),
('3115', '22503020', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x30x2,00', 'MT', 1722.00, 'EUR', 
 4.94, 1.96, 1.94, '2024-04-16', '2024-06-14', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 30, 2.00, NULL),
('3116', '22503025', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x30x2,50', 'MT', 0.00, 'EUR', 
 6.61, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 30, 2.50, NULL),
('3117', '22503030', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x30x3,00', 'MT', 510.00, 'EUR', 
 6.61, 2.65, 2.65, '2024-04-29', '2024-06-13', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 30, 3.00, NULL),
('3118', '22503040', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x30x4,00', 'MT', 0.00, 'EUR', 
 8.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 30, 4.00, NULL),
('3119', '22503515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x35x1,50', 'MT', 0.00, 'EUR', 
 4.54, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 35, 1.50, NULL),
('3120', '22503520', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x35x2,00', 'MT', 0.00, 'EUR', 
 5.84, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 35, 2.00, NULL),
('3121', '22503820', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x38x2,00', 'MT', 0.00, 'EUR', 
 5.86, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 38, 2.00, NULL),
('3122', '22504015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x40x1,50', 'MT', 0.00, 'EUR', 
 4.63, 0.00, 0.00, NULL, '2023-08-23', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 40, 1.50, NULL),
('3123', '22504030', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 50x40x2,50', 'MT', 0.00, 'EUR', 
 7.86, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 50, 40, 2.50, NULL),
('3124', '22552515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 55x25x1,50', 'MT', 0.00, 'EUR', 
 4.49, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 55, 25, 1.50, NULL),
('3125', '22552520', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 55x25x2,00', 'MT', 0.00, 'EUR', 
 5.73, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 55, 25, 2.00, NULL),
('3126', '22553515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 55x35x1,50', 'MT', 0.00, 'EUR', 
 4.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 55, 35, 1.50, NULL);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3127', '22553520', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 55x35x2,00', 'MT', 0.00, 'EUR', 
 5.64, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 55, 35, 2.00, NULL),
('3128', '22554520', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 55x45x2,00', 'MT', 0.00, 'EUR', 
 6.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 55, 45, 2.00, NULL),
('3129', '22554530', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 55x45x3,00', 'MT', 0.00, 'EUR', 
 9.86, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 55, 45, 3.00, NULL),
('3130', '22601015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x10x1,50', 'ML', 1260.00, 'EUR', 
 3.63, 1.47, 1.48, '2024-04-08', '2024-06-13', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 10, 1.50, NULL),
('3131', '22601515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x15x1,50', 'MT', 72.00, 'EUR', 
 3.70, 1.59, 1.57, '2024-05-03', '2024-06-03', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 15, 1.50, NULL),
('3132', '22602015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x20x1,50', 'MT', 672.00, 'EUR', 
 4.00, 1.57, 1.57, '2024-05-28', '2024-06-13', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 20, 1.50, NULL),
('3133', '22602020', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x20x2,00', 'ML', 66.00, 'EUR', 
 5.11, 2.10, 2.10, '2023-09-22', '2024-06-11', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 20, 2.00, NULL),
('3135', '22602515', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x25x1,50', 'MT', 0.00, 'EUR', 
 4.49, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 25, 1.50, NULL),
('3136', '22603015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x30x1,50', 'MT', 1470.00, 'EUR', 
 4.30, 1.79, 1.72, '2024-04-26', '2024-06-12', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 30, 1.50, NULL),
('3137', '22603020', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x30x2,00', 'MT', 1380.00, 'EUR', 
 5.64, 2.30, 2.26, '2024-04-29', '2024-06-14', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 30, 2.00, NULL),
('3138', '22603030', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x30x3,00', 'MT', 114.00, 'EUR', 
 7.55, 3.02, 3.02, '2024-04-15', '2024-06-13', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 30, 3.00, NULL),
('3139', '22603032', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x30x3,20', 'MT', 0.00, 'EUR', 
 9.68, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 30, 3.20, NULL),
('3140', '22604015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x40x1,50', 'MT', 1944.00, 'EUR', 
 4.73, 1.94, 1.93, '2024-06-07', '2024-06-11', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 40, 1.50, NULL),
('3141', '22604020', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x40x2,00', 'MT', 1296.00, 'EUR', 
 6.20, 2.58, 2.58, '2024-03-07', '2024-06-07', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 40, 2.00, NULL),
('3142', '22604030', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x40x3,00', 'MT', 144.00, 'EUR', 
 8.53, 3.45, 3.45, '2023-11-08', '2024-06-14', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 40, 3.00, NULL),
('3143', '22604040', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x40x4,00', 'MT', 138.00, 'EUR', 
 10.93, 4.38, 4.37, '2024-04-15', '2024-06-05', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 40, 4.00, NULL);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3144', '22604530', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x45x3,00', 'MT', 0.00, 'EUR', 
 9.86, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 45, 3.00, NULL),
('3145', '22605030', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 60x50x3,00', 'MT', 0.00, 'EUR', 
 9.86, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 60, 50, 3.00, NULL),
('3146', '22703015', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 70x30x1,50', 'MT', 0.00, 'EUR', 
 5.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 70, 30, 1.50, NULL),
('3147', '22703020', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 70x30x2,00', 'MT', 0.00, 'EUR', 
 6.53, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 70, 30, 2.00, NULL),
('3148', '22704015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 70x40x1,50', 'MT', 0.00, 'EUR', 
 5.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 70, 40, 1.50, NULL),
('3149', '22704030', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 70x40x3,00', 'MT', 0.00, 'EUR', 
 9.64, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 70, 40, 3.00, NULL),
('3150', '22705030', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 70x50x3,00', 'MT', 0.00, 'EUR', 
 10.39, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 70, 50, 3.00, NULL),
('3151', '22705031', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 70x50x4,00', 'MT', 0.00, 'EUR', 
 13.44, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 70, 50, 4.00, NULL),
('3152', '22755030', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 75x50x3,00', 'MT', 0.00, 'EUR', 
 11.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 75, 50, 3.00, NULL),
('3153', '22802015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 80x20x1,50', 'MT', 960.00, 'EUR', 
 5.34, 2.15, 2.09, '2024-05-24', '2024-06-05', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 20, 1.50, NULL),
('3154', '22803015', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 80x30x1,50', 'MT', 0.00, 'EUR', 
 5.79, 2.81, 2.81, '2022-10-20', '2023-12-18', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 30, 1.50, NULL),
('3155', '22803020', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 80x30x2,00', 'ML', 468.00, 'EUR', 
 7.41, 3.02, 3.02, '2024-06-06', '2018-10-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 30, 2.00, NULL),
('3156', '22804015', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 80x40x1,50', 'MT', 2040.00, 'EUR', 
 5.78, 2.36, 2.31, '2024-05-09', '2024-06-05', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 40, 1.50, NULL),
('3157', '22804020', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 80x40x2,00', 'MT', 1440.00, 'EUR', 
 7.50, 3.06, 3.00, '2024-05-03', '2024-06-12', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 40, 2.00, NULL),
('3158', '22804030', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 80x40x3,00', 'MT', 750.00, 'EUR', 
 10.39, 4.18, 4.24, '2024-06-07', '2024-06-13', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 40, 3.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3159', '22804040', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 80x40x4,00', 'MT', 228.00, 'EUR', 
 13.44, 5.49, 5.48, '2024-06-10', '2024-05-23', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 40, 4.00, NULL),
('3160', '22804050', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 80x40x5,00', 'MT', 336.00, 'EUR', 
 16.59, 6.77, 6.77, '2024-06-10', '2024-06-05', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 40, 5.00, NULL),
('3161', '22805030', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 80x50x3,00', 'MT', 0.00, 'EUR', 
 11.43, 3.24, 5.78, '2020-06-30', '2018-07-13', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 50, 3.00, NULL),
('3162', '22805050', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 80x50x5,00', 'ML', 0.00, 'EUR', 
 18.40, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 50, 5.00, NULL),
('3164', '22806020', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 80x60x2,00', 'ML', 492.00, 'EUR', 
 9.50, 4.32, 4.18, '2024-02-08', '2024-05-17', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 60, 2.00, NULL),
('3165', '22806030', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 80x60x3,00', 'MT', 102.00, 'EUR', 
 12.31, 5.17, 4.93, '2024-04-15', '2024-06-07', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 60, 3.00, NULL),
('3166', '22806040', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 80x60x4,00', 'MT', 0.00, 'EUR', 
 16.24, 8.83, 8.83, '2021-04-26', '2023-03-07', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 60, 4.00, NULL),
('3167', '22806050', 'TUBO AÇO RECT.', 'TUBO ACO RECT. 80x60x5,00', 'MT', 0.00, 'EUR', 
 20.09, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 80, 60, 5.00, NULL),
('3168', '22895030', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 90x50x3,00', 'MT', 0.00, 'EUR', 
 12.28, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 90, 50, 3.00, NULL),
('3169', '22895050', 'TUBO AÇO RECT.', 'TUBO AÇO RECT. 90x50x5,00', 'MT', 0.00, 'EUR', 
 20.24, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 90, 50, 5.00, NULL),
('3170', '22902015', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x20x1,50', 'MT', 312.00, 'EUR', 
 6.93, 2.91, 2.99, '2024-01-19', '2024-05-27', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 20, 1.50, NULL),
('3171', '22902020', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x20x2,00', 'MT', 0.00, 'EUR', 
 8.84, 3.87, 5.23, '2021-03-15', '2021-03-19', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 20, 2.00, NULL),
('3172', '22903015', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x30x1,50', 'MT', 0.00, 'EUR', 
 7.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 30, 1.50, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3173', '22903020', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x30x2,00', 'MT', 252.00, 'EUR', 
 8.93, 3.64, 3.64, '2024-04-11', '2024-06-11', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 30, 2.00, NULL),
('3174', '22904020', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x40x2,00', 'MT', 318.00, 'EUR', 
 9.01, 3.80, 3.82, '2024-02-27', '2024-05-17', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 40, 2.00, NULL),
('3175', '22904030', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x40x3,00', 'MT', 540.00, 'EUR', 
 12.28, 4.92, 4.81, '2024-05-22', '2024-06-03', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 40, 3.00, NULL),
('3176', '22904040', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x40x4,00', 'MT', 186.00, 'EUR', 
 15.98, 10.11, 10.11, '2022-04-21', '2024-03-05', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 40, 4.00, NULL),
('3177', '22904050', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x40x5,00', 'MT', 0.00, 'EUR', 
 20.20, 8.18, 8.18, '2023-10-06', NULL, 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 40, 5.00, NULL),
('3178', '22905050', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x50x5,00', 'MT', 192.00, 'EUR', 
 21.91, 8.94, 8.94, '2024-05-21', '2024-05-29', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 50, 5.00, NULL),
('3179', '22905120', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x50x2,00', 'MT', 1573.20, 'EUR', 
 9.60, 3.86, 3.84, '2024-05-28', '2024-06-13', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 50, 2.00, NULL),
('3180', '22905130', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x50x3,00', 'MT', 588.00, 'EUR', 
 13.41, 5.51, 5.37, '2024-04-26', '2024-06-12', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 50, 3.00, NULL),
('3181', '22905140', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x50x4,00', 'MT', 96.00, 'EUR', 
 17.46, 7.12, 7.12, '2024-05-17', '2024-05-24', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 50, 4.00, NULL),
('3182', '22905160', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x50x6,00', 'MT', 0.00, 'EUR', 
 26.41, 8.52, 8.52, '2019-05-13', '2019-05-15', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 50, 6.00, NULL),
('3183', '22906020', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x60x2,00', 'MT', 108.00, 'EUR', 
 11.40, 5.52, 5.46, '2022-09-19', '2024-06-13', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 60, 2.00, NULL),
('3184', '22906030', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x60x3,00', 'MT', 192.00, 'EUR', 
 14.38, 5.98, 5.98, '2024-03-08', '2024-06-13', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 60, 3.00, NULL),
('3185', '22906040', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x60x4,00', 'MT', 108.00, 'EUR', 
 18.74, 7.79, 7.79, '2024-03-08', '2024-05-24', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 60, 4.00, NULL);






INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3186', '22906050', 'TUBO AÇO RECT.', 'TUBO ACO.RECT.100x60x5,00', 'MT', 12.00, 'EUR', 
 23.59, 9.62, 9.62, '2024-04-11', '2024-05-16', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 60, 5.00, NULL),
('3187', '22906060', 'TUBO AÇO RECT.', 'TUBO ACO.RECT.100x60x8,00', 'MT', 0.00, 'EUR', 
 39.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 60, 8.00, NULL),
('3188', '22908030', 'TUBO AÇO RECT.', 'TUBO EST.RECT.150x100x3,00', 'MT', 174.00, 'EUR', 
 24.20, 9.87, 9.87, '2024-05-24', '2024-06-11', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 150, 100, 3.00, NULL),
('3189', '22908040', 'TUBO AÇO RECT.', 'TUBO EST.RECT.150x100x4,00', 'MT', 102.00, 'EUR', 
 31.55, 12.70, 12.62, '2024-04-29', '2024-05-23', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 150, 100, 4.00, NULL),
('3190', '22908060', 'TUBO AÇO RECT.', 'TUBO EST.RECT.150x100x5,00', 'MT', 0.00, 'EUR', 
 38.76, 13.01, 13.01, '2018-11-22', '2018-11-26', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 150, 100, 5.00, NULL),
('3191', '22918030', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x80x3,00', 'MT', 0.00, 'EUR', 
 16.28, 10.03, 10.42, '2022-03-25', '2022-12-15', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 80, 3.00, NULL),
('3192', '22918040', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.100x80x4,00', 'MT', 120.00, 'EUR', 
 21.31, 8.67, 8.61, '2023-10-09', '2024-05-22', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 100, 80, 4.00, NULL),
('3193', '22921060', 'TUBO AÇO RECT.', 'TUBO EST.RECT.150x100x6,00', 'MT', 0.00, 'EUR', 
 46.78, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 150, 100, 6.00, NULL),
('3194', '22921061', 'TUBO AÇO RECT.', 'TUBO EST.RECT.150x100x8,00', 'MT', 0.00, 'EUR', 
 62.51, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 150, 100, 8.00, NULL),
('3195', '22924020', 'TUBO AÇO RECT.', 'TUBO EST.RECT.120x40x2,00', 'MT', 294.00, 'EUR', 
 11.14, 5.26, 4.34, '2023-12-05', '2024-06-06', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 40, 2.00, NULL),
('3196', '22924030', 'TUBO AÇO RECT.', 'TUBO EST.RECT.120x40x3,00', 'MT', 156.00, 'EUR', 
 15.56, 7.97, 7.97, '2022-10-19', '2024-03-11', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 40, 3.00, NULL),
('3197', '22926020', 'TUBO AÇO RECT.', 'TUBO AÇO RECT.120x60x2,00', 'ML', 318.00, 'EUR', 
 11.65, 4.94, 4.97, '2024-03-15', '2024-06-04', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 60, 2.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3198', '22926030', 'TUBO EST.RECT.', 'TUBO EST.RECT.120x60x3,00', 'MT', 534.00, 'EUR', 
 16.28, 6.63, 6.38, '2024-05-24', '2024-06-06', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 60, 3.00, NULL),
('3199', '22926040', 'TUBO EST.RECT.', 'TUBO EST.RECT.120x60x4.00', 'MT', 384.00, 'EUR', 
 21.34, 8.65, 8.54, '2024-04-26', '2024-05-29', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 60, 4.00, NULL),
('3200', '22926050', 'TUBO EST.RECT.', 'TUBO EST.RECT.120x60x5,00', 'MT', 0.00, 'EUR', 
 26.38, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 60, 5.00, NULL),
('3201', '22926060', 'TUBO EST.RECT.', 'TUBO EST. RECT.120x60x6,00', 'ML', 0.00, 'EUR', 
 31.51, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 60, 6.00, NULL),
('3203', '22926080', 'TUBO EST.RECT.', 'TUBO EST.RECT.120x40x4,00', 'MT', 0.00, 'EUR', 
 20.26, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 40, 4.00, NULL),
('3205', '22928030', 'TUBO EST.RECT.', 'TUBO EST.RECT.120x80x3,00', 'MT', 12.00, 'EUR', 
 18.21, 7.43, 7.43, '2023-10-06', '2024-05-28', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 80, 3.00, NULL),
('3206', '22928040', 'TUBO EST.RECT.', 'TUBO EST.RECT.120x80x4,00', 'MT', 0.00, 'EUR', 
 23.79, 9.52, 9.52, '2024-05-09', '2024-05-29', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 80, 4.00, NULL),
('3207', '22928050', 'TUBO EST.RECT.', 'TUBO EST.RECT. 120x80x5,00', 'MT', 0.00, 'EUR', 
 29.66, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 80, 5.00, NULL),
('3208', '22928055', 'TUBO EST.RECT.', 'TUBO EST.RECT.120x80x6,00 ', 'MT', 0.00, 'EUR', 
 34.80, 13.92, 13.92, '2023-11-02', '2023-12-21', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 80, 6.00, NULL),
('3209', '22928059', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x60x5,00', 'MT', 0.00, 'EUR', 
 30.06, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 60, 5.00, NULL),
('3210', '2292806', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x60x5,00', 'MT', 0.00, 'EUR', 
 17.52, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 60, 5.00, NULL),
('3211', '22928060', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x60x3,00', 'MT', 222.00, 'EUR', 
 18.69, 7.99, 7.92, '2024-03-12', '2024-05-13', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 60, 3.00, NULL),
('3212', '22928061', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x60x4,00', 'MT', 72.00, 'EUR', 
 24.41, 9.50, 9.57, '2023-10-30', '2024-05-09', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 60, 4.00, NULL);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3213', '22928070', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x70x4,00', 'MT', 0.00, 'EUR', 
 26.44, 8.70, 8.70, '2018-06-30', '2018-06-19', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 70, 4.00, NULL),
('3214', '22928130', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x80x3,00', 'MT', 72.00, 'EUR', 
 21.18, 8.98, 8.98, '2024-03-12', '2024-06-12', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 80, 3.00, NULL),
('3215', '22928179', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x80x4,00', 'MT', 0.00, 'EUR', 
 26.93, 12.92, 21.54, '2022-11-03', '2022-11-15', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 80, 4.00, NULL),
('3216', '22928180', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x80x5,00', 'MT', 0.00, 'EUR', 
 33.14, 11.54, 11.54, '2019-05-16', '2019-05-22', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 80, 5.00, NULL),
('3217', '22928181', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x80x8,00', 'ML', 0.00, 'EUR', 
 100.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 80, 8.00, NULL),
('3219', '22928182', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x80x6,00', 'ML', 0.00, 'EUR', 
 39.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 80, 6.00, NULL),
('3220', '22928183', 'TUBO EST.RECT.', 'TUBO EST.RECT.140x100x3,00', 'ML', 0.00, 'EUR', 
 24.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 140, 100, 3.00, NULL),
('3221', '22928200', 'TUBO EST.RECT.', 'TUBO EST.RECT.120x100x4,00', 'MT', 0.00, 'EUR', 
 27.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 100, 4.00, NULL),
('3222', '22928201', 'TUBO EST.RECT.', 'TUBO EST.RECT.120x100x5,00', 'MT', 0.00, 'EUR', 
 33.51, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 100, 5.00, NULL),
('3223', '22928202', 'TUBO EST.RECT.', 'TUBO EST.RECT.120x100x6,00', 'MT', 0.00, 'EUR', 
 39.96, 26.24, 26.24, '2023-10-19', '2023-10-20', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 120, 100, 6.00, NULL),
('3226', '22938030', 'TUBO EST.RECT.', 'TUBO EST.RECT.160x80x3,00', 'MT', 480.00, 'EUR', 
 22.25, 8.91, 8.72, '2024-05-24', '2024-05-16', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 160, 80, 3.00, NULL),
('3227', '22938040', 'TUBO EST.RECT.', 'TUBO EST.RECT.160x80x4,00', 'MT', 192.00, 'EUR', 
 28.86, 12.08, 11.89, '2023-12-21', '2024-06-12', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 160, 80, 4.00, NULL),
('3228', '22938050', 'TUBO EST.RECT.', 'TUBO EST.RECT.160x80x5,00', 'MT', 0.00, 'EUR', 
 35.55, 12.26, 12.26, '2019-09-09', '2019-09-10', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 160, 80, 5.00, NULL),
('3229', '22938130', 'TUBO EST.RECT.', 'TUBO EST.RECT.160x120x3,00', 'MT', 0.00, 'EUR', 
 27.58, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 160, 120, 3.00, NULL),
('3230', '22938140', 'TUBO EST.RECT.', 'TUBO EST.RECT.160x120x4,00', 'ML', 0.00, 'EUR', 
 36.46, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 160, 120, 4.00, NULL),
('3231', '22948030', 'TUBO EST.RECT.', 'TUBO EST.RECT.180x100x3,00', 'MT', 0.00, 'EUR', 
 27.20, 8.20, 13.79, '2020-02-14', '2020-03-30', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 180, 100, 3.00, NULL),
('3232', '22948040', 'TUBO EST.RECT.', 'TUBO EST.RECT.180x100x4,00', 'MT', 0.00, 'EUR', 
 35.98, 11.92, 0.00, '2020-03-30', '2020-03-30', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 180, 100, 4.00, NULL),
('3233', '22948050', 'TUBO EST.RECT.', 'TUBO EST.RECT.180x100x5,00', 'MT', 0.00, 'EUR', 
 44.74, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 180, 100, 5.00, NULL),
('3234', '22948051', 'TUBO EST.RECT.', 'TUBO EST.RECT.180x100x6,00', 'MT', 0.00, 'EUR', 
 53.86, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 180, 100, 6.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3235', '22948052', 'TUBO EST.RECT.', 'TUBO EST.RECT.180x100x8,00', 'MT', 0.00, 'EUR', 
 71.55, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 180, 100, 8.00, NULL),
('3236', '22948053', 'TUBO EST.RECT.', 'TUBO EST.RECT.180x120x4,00', 'ML', 0.00, 'EUR', 
 38.55, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 180, 120, 4.00, NULL),
('3238', '22948060', 'TUBO EST.RECT.', 'TUBO EST.RECT.180x80x4,00', 'MT', 0.00, 'EUR', 
 33.20, 9.68, 16.83, '2020-05-05', '2020-05-07', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 180, 80, 4.00, NULL),
('3239', '22948061', 'TUBO EST.RECT.', 'TUBO EST.RECT.180x80x3,00', 'ML', 54.00, 'EUR', 
 25.28, 10.11, 10.11, '2024-04-19', '2024-05-20', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 180, 80, 3.00, NULL),
('3240', '22948062', 'TUBO EST.RECT.', 'TUBO EST.RECT.180x80x5,00', 'ML', 0.00, 'EUR', 
 40.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 180, 80, 5.00, NULL),
('3242', '22950840', 'TUBO EST.RECT.', 'TUBO EST.RECT.200x80x3,00', 'MT', 0.00, 'EUR', 
 27.20, 8.14, 13.79, '2020-04-15', '2020-06-17', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 80, 3.00, NULL),
('3243', '22950850', 'TUBO EST.RECT.', 'TUBO EST.RECT.200x80x5,00', 'ML', 0.00, 'EUR', 
 45.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 80, 5.00, NULL),
('3244', '22951030', 'TUBO EST.RECT.', 'TUBO EST.RECT.200x100x3,00', 'ML', 222.00, 'EUR', 
 29.34, 11.75, 11.50, '2024-04-29', '2024-06-13', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 100, 3.00, NULL),
('3245', '22951040', 'TUBO EST.RECT.', 'TUBO EST.RECT.200x100x4,00', 'MT', 426.00, 'EUR', 
 38.55, 15.76, 15.73, '2024-05-24', '2024-06-12', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 100, 4.00, NULL),
('3246', '22951050', 'TUBO EST.RECT.', 'TUBO EST.RECT.200x100x6,00', 'MT', 0.00, 'EUR', 
 55.74, 18.84, 18.84, '2018-12-03', '2019-12-27', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 100, 6.00, NULL),
('3247', '22951051', 'TUBO EST.RECT.', 'TUBO EST.RECT.200x100x12,00', 'ML', 0.00, 'EUR', 
 112.46, 36.97, 36.97, '2018-11-22', '2018-11-23', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 100, 12.00, NULL),
('3248', '22951060', 'TUBO EST.RECT.', 'TUBO EST.RECT.200x100x5,00', 'MT', 36.00, 'EUR', 
 46.24, 18.13, 18.13, '2024-04-29', '2024-05-10', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 100, 5.00, NULL),
('3249', '22951070', 'TUBO EST.RECT.', 'TUBO EST.RECT.200x120x5,00', 'MT', 0.00, 'EUR', 
 52.43, 15.48, 15.48, '2020-01-08', '2020-06-22', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 120, 5.00, NULL),
('3250', '22951071', 'TUBO EST.RECT.', 'TUBO EST.RECT.200x120x6,00', 'MT', 0.00, 'EUR', 
 62.39, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 120, 6.00, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3251', '22951072', 'TUBO EST. RECT.', 'TUBO EST. RECT.200x120x8,00', 'MT', 0.00, 'EUR', 
 84.16, 42.01, 42.01, '2023-06-05', '2023-06-06', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 120, 8.00, NULL),
('3252', '22951073', 'TUBO EST. RECT.', 'TUBO EST. RECT.200x120x10,0', 'MT', 0.00, 'EUR', 
 105.05, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 120, 10.00, NULL),
('3253', '22951080', 'TUBO EST. RECT.', 'TUBO EST. RECT.200x100x8,00', 'MT', 0.00, 'EUR', 
 78.18, 29.43, 21.70, '2018-11-23', '2018-11-23', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 100, 8.00, NULL),
('3254', '22951081', 'TUBO EST. RECT.', 'TUBO EST. RECT.200x150x5,00', 'MT', 0.00, 'EUR', 
 57.06, 16.91, 16.91, '2019-12-16', '2020-04-22', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 150, 5.00, NULL),
('3256', '22951082', 'TUBO EST. RECT.', 'TUBO EST. RECT.200x150x4,00', 'MT', 84.00, 'EUR', 
 46.01, 18.42, 18.41, '2024-04-19', '2024-05-02', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 150, 4.00, NULL),
('3258', '22951083', 'TUBO EST. RECT.', 'TUBO EST. RECT.200x150x6,00', 'MT', 0.00, 'EUR', 
 68.31, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 150, 6.00, NULL),
('3260', '22951084', 'TUBO EST. RECT.', 'TUBO EST. RECT.250x100x4,00', 'MT', 0.00, 'EUR', 
 46.01, 18.37, 20.97, '2021-03-29', '2022-09-28', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 250, 100, 4.00, NULL),
('3261', '22951085', 'TUBO EST. RECT.', 'TUBO EST. RECT 250x150x5,00', 'MT', 0.00, 'EUR', 
 65.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 250, 150, 5.00, NULL),
('3262', '22951086', 'TUBO EST. RECT.', 'TUBO EST. RECT.250x150x6,00', 'MT', 0.00, 'EUR', 
 78.15, 35.17, 35.17, '2023-09-13', '2023-09-14', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 250, 150, 6.00, NULL),
('3263', '22951088', 'TUBO EST. RECT.', 'TUBO EST. RECT.250x150x8,00', 'ML', 0.00, 'EUR', 
 108.49, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 250, 150, 8.00, NULL),
('3264', '22951089', 'TUBO EST. RECT.', 'TUBO EST. RECT.250x100x05 mm', 'MT', 0.00, 'EUR', 
 57.06, 26.48, 26.48, '2022-11-14', '2022-11-17', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 250, 100, 5.00, NULL),
('3265', '22951090', 'TUBO EST. RECT.', 'TUBO EST. RECT.400x200x12', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 400, 200, 12.00, NULL),
('3266', '22951100', 'TUBO EST. RECT.', 'TUBO EST. RECT.200x100x10', 'MT', 0.00, 'EUR', 
 95.56, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 100, 10.00, NULL),
('3268', '22952060', 'TUBO EST. RECT.', 'TUBO EST. RECT.300x200x6,00', 'MT', 0.00, 'EUR', 
 104.23, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 300, 200, 6.00, NULL),
('3269', '22952080', 'TUBO EST. RECT.', 'TUBO EST. RECT.300x200x8,00', 'MT', 0.00, 'EUR', 
 143.05, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 300, 200, 8.00, NULL);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3270', '22952090', 'TUBO EST. RECT.', 'TUBO EST. RECT.300x200x10', 'MT', 0.00, 'EUR', 
 181.34, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 300, 200, 10.00, NULL),
('3271', '22953015', 'TUBO EST. RECT.', 'TUBO EST.RECT.300x150x5,00', 'MT', 0.00, 'EUR', 
 74.70, 37.65, 37.65, '2023-03-06', '2023-03-07', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 300, 150, 5.00, NULL),
('3272', '22953050', 'TUBO EST. RECT.', 'TUBO EST. RECT.300x100x5,00', 'MT', 0.00, 'EUR', 
 66.13, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 300, 100, 5.00, NULL),
('3273', '22953060', 'TUBO EST. RECT.', 'TUBO EST. RECT.300x150x6,00', 'MT', 0.00, 'EUR', 
 94.40, 58.75, 58.75, '2021-11-12', '2021-11-15', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 300, 150, 6.00, NULL),
('3274', '22953080', 'TUBO EST. RECT.', 'TUBO EST.RECT.400x200x8,00', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 400, 200, 8.00, NULL),
('3276', '22953212', 'TUBO EST. RECT.', 'TUBO EST.RECT.300x100x4 mm', 'MT', 0.00, 'EUR', 
 52.05, 16.09, 26.38, '2020-06-12', '2020-06-17', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 300, 100, 4.00, NULL),
('3278', '22955020', 'TUBO EST. RECT.', 'TUBO EST. RECT.150x50x3,00', 'MT', 48.00, 'EUR', 
 18.46, 7.79, 7.53, '2024-04-12', '2024-06-14', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 150, 50, 3.00, NULL),
('3279', '22955030', 'TUBO EST. RECT.', 'TUBO EST. RECT.150x50x4,00', 'MT', 0.00, 'EUR', 
 24.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 150, 50, 4.00, NULL),
('3280', '22955050', 'TUBO EST. RECT.', 'TUBO EST. RECT.150x50x5,00', 'MT', 0.00, 'EUR', 
 30.06, 13.47, 13.47, '2023-05-30', '2023-11-30', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 150, 50, 5.00, NULL),
('3281', '22955091', 'TUBO EST. RECT.', 'TUBO EST. RECT.250x150x4,00', 'MT', 0.00, 'EUR', 
 53.38, 17.50, 17.50, '2019-05-08', '2019-05-10', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 250, 150, 4.00, NULL),
('3282', '22958030', 'TUBO EST. RECT.', 'TUBO EST.RECT.200x80x4,00', 'ML', 0.00, 'EUR', 
 35.98, 11.79, 11.79, '2018-06-30', '2018-06-29', 23, NULL, 6, 2, 1, NULL, NULL, NULL, 200, 80, 4.00, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3382', '23420101', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.20x10x1,50', 'MT', 1824.00, 'EUR', 
 1.73, 0.68, 0.68, '2024-05-28', '2024-06-12', 22, NULL, 6, 2, 1, 2, NULL, NULL, 20, 10, 1.50, NULL),
('3383', '23430101', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.30x10x1.50', 'M', 1044.00, 'EUR', 
 2.34, 0.97, 0.97, '2024-03-22', '2024-06-06', 22, NULL, 6, 2, 1, 2, NULL, NULL, 30, 10, 1.50, NULL),
('3386', '23430151', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.30x15x1,50', 'MT', 780.00, 'EUR', 
 2.46, 1.00, 0.99, '2024-05-09', '2024-06-07', 22, NULL, 6, 2, 1, 2, NULL, NULL, 30, 15, 1.50, NULL),
('3387', '23430201', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.30x20x1,50', 'MT', 726.00, 'EUR', 
 2.59, 1.04, 1.04, '2024-05-14', '2024-06-12', 22, NULL, 6, 2, 1, 2, NULL, NULL, 30, 20, 1.50, NULL),
('3388', '23435201', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.35x20x1,50', 'MT', 624.00, 'EUR', 
 2.99, 1.32, 1.31, '2024-03-05', '2024-06-06', 22, NULL, 6, 2, 1, 2, NULL, NULL, 35, 20, 1.50, NULL),
('3389', '23440101', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.40x10x1,50', 'MT', 0.00, 'EUR', 
 2.63, 1.07, 1.07, '2024-06-06', '2024-06-07', 22, NULL, 6, 2, 1, 2, NULL, NULL, 40, 10, 1.50, NULL),
('3390', '23440151', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.40x15x1,50', 'MT', 0.00, 'EUR', 
 2.94, 0.86, 0.86, '2017-12-21', '2018-05-07', 22, NULL, 6, 2, 1, 2, NULL, NULL, 40, 15, 1.50, NULL),
('3391', '23440201', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.40x20x1,50', 'MT', 2172.00, 'EUR', 
 3.09, 1.23, 1.21, '2024-05-24', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 40, 20, 1.50, NULL),
('3392', '23440202', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.40x20x2,00', 'MT', 1788.00, 'EUR', 
 4.08, 1.64, 1.63, '2024-04-26', '2024-06-07', 22, NULL, 6, 2, 1, 2, NULL, NULL, 40, 20, 2.00, NULL),
('3393', '23440301', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.40x30x1,50', 'MT', 1026.00, 'EUR', 
 3.64, 1.48, 1.48, '2024-06-07', '2024-06-07', 22, NULL, 6, 2, 1, 2, NULL, NULL, 40, 30, 1.50, NULL),
('3394', '23450101', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.50x10x1,50', 'ML', 780.00, 'EUR', 
 3.16, 1.31, 1.28, '2024-05-09', '2024-06-13', 22, NULL, 6, 2, 1, 2, NULL, NULL, 50, 10, 1.50, NULL),
('3396', '23450151', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.50x15x1,50', 'MT', 0.00, 'EUR', 
 3.64, 1.40, 1.40, '2024-05-31', '2024-06-03', 22, NULL, 6, 2, 1, 2, NULL, NULL, 50, 15, 1.50, NULL),
('3397', '23450201', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.50x20x1,50', 'MT', 1890.00, 'EUR', 
 3.64, 1.48, 1.48, '2024-06-10', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 50, 20, 1.50, NULL),
('3398', '23450202', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.50x20x2,00', 'MT', 300.00, 'EUR', 
 4.81, 1.98, 1.98, '2024-04-08', '2024-06-05', 22, NULL, 6, 2, 1, 2, NULL, NULL, 50, 20, 2.00, NULL);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3399', '23450301', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.50x30x1,50', 'MT', 5952.00, 'EUR', 
 4.18, 1.67, 1.64, '2024-05-24', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 50, 30, 1.50, NULL),
('3400', '23450302', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.50x30x2,00', 'MT', 2952.00, 'EUR', 
 5.49, 2.13, 2.20, '2024-05-27', '2024-06-13', 22, NULL, 6, 2, 1, 2, NULL, NULL, 50, 30, 2.00, NULL),
('3401', '23450401', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.50x40x1.50', 'MT', 0.00, 'EUR', 
 4.73, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 1, 2, NULL, NULL, 50, 40, 1.50, NULL),
('3402', '23450402', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.50x40x2,00', 'MT', 0.00, 'EUR', 
 6.21, 2.54, 2.54, '2024-05-24', '2024-06-05', 22, NULL, 6, 2, 1, 2, NULL, NULL, 50, 40, 2.00, NULL),
('3403', '23460101', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.60x10x1,50', 'MT', 1188.00, 'EUR', 
 3.79, 1.58, 1.55, '2024-06-07', '2024-05-31', 22, NULL, 6, 2, 1, 2, NULL, NULL, 60, 10, 1.50, NULL),
('3404', '23460201', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.60x20x1,50', 'MT', 1116.00, 'EUR', 
 4.26, 1.65, 1.67, '2024-05-27', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 60, 20, 1.50, NULL),
('3405', '23460301', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.60x30x1.50', 'MT', 2880.00, 'EUR', 
 4.73, 1.98, 1.99, '2024-03-19', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 60, 30, 1.50, NULL),
('3406', '23460302', 'T.ACO GAL.RECT.', 'T.AÇO GAL.RECT.60x30x2,00', 'MT', 1896.00, 'EUR', 
 6.21, 2.63, 2.62, '2024-04-03', '2024-06-13', 22, NULL, 6, 2, 1, 2, NULL, NULL, 60, 30, 2.00, NULL),
('3407', '23460401', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.60x40x1.50', 'MT', 4032.00, 'EUR', 
 5.26, 2.09, 2.06, '2024-05-24', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 60, 40, 1.50, NULL),
('3408', '23460402', 'T.ACO GAL.RECT.', 'T.AÇO GAL.RECT.60x40x2,00', 'MT', 1350.00, 'EUR', 
 6.94, 2.80, 2.78, '2024-05-14', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 60, 40, 2.00, NULL),
('3409', '23470401', 'T.ACO GAL.RECT.', 'T.AÇO GALV.RECT.80x60x2.00', 'ML', 276.00, 'EUR', 
 10.00, 4.09, 4.08, '2024-06-07', '2024-06-03', 22, NULL, 6, 2, 1, 2, NULL, NULL, 80, 60, 2.00, NULL),
('3411', '23480201', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.80x20x1,50', 'MT', 750.00, 'EUR', 
 6.22, 2.44, 2.44, '2024-05-24', '2024-06-13', 22, NULL, 6, 2, 1, 2, NULL, NULL, 80, 20, 1.50, NULL),
('3412', '23480202', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.80x20x2,00', 'MT', 0.00, 'EUR', 
 8.19, 0.00, 0.00, '1900-01-01', '1900-01-01', 22, NULL, 6, 2, 1, 2, NULL, NULL, 80, 20, 2.00, NULL),
('3413', '23480302', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.80x30x2,00', 'MT', 0.00, 'EUR', 
 7.83, 2.42, 2.42, '2019-11-27', '2019-12-03', 22, NULL, 6, 2, 1, 2, NULL, NULL, 80, 30, 2.00, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3414', '23480401', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.80x40x1.50', 'MT', 1314.00, 'EUR', 
 6.35, 2.52, 2.49, '2024-05-24', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 80, 40, 1.50, NULL),
('3415', '23480402', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.80x40x2.00', 'MT', 1548.00, 'EUR', 
 8.39, 3.39, 3.39, '2024-05-10', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 80, 40, 2.00, NULL),
('3416', '23480403', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.80x40x3.00', 'MT', 402.00, 'EUR', 
 12.28, 4.91, 4.91, '2024-04-26', '2024-06-12', 22, NULL, 6, 2, 1, 2, NULL, NULL, 80, 40, 3.00, NULL),
('3417', '23490401', 'T.ACO GAL.RECT.', 'T.AÇO GAL.RECT.100x40x1.50', 'MT', 0.00, 'EUR', 
 7.56, 2.42, 2.42, '2020-08-03', '2024-03-27', 22, NULL, 6, 2, 1, 2, NULL, NULL, 100, 40, 1.50, NULL),
('3418', '23490402', 'T.ACO GAL.RECT.', 'T.AÇO GAL.RECT.100x40x2.00', 'MT', 1440.00, 'EUR', 
 9.84, 3.96, 3.97, '2024-05-10', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 100, 40, 2.00, NULL),
('3419', '23490501', 'T.ACO GAL.RECT.', 'T.AÇO GALV.RECT.100x50x1.50', 'ML', 348.00, 'EUR', 
 8.18, 3.32, 3.34, '2024-06-07', '2024-06-13', 22, NULL, 6, 2, 1, 2, NULL, NULL, 100, 50, 1.50, NULL),
('3420', '23490502', 'T.ACO GAL.RECT.', 'T.AÇO GAL.RECT.100x50x2.00', 'MT', 2328.00, 'EUR', 
 10.56, 4.18, 4.23, '2024-05-28', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 100, 50, 2.00, NULL),
('3421', '23490503', 'T.ACO GAL.RECT.', 'T.AÇO GAL.RECT.100x50x3.00', 'MT', 942.00, 'EUR', 
 15.61, 6.34, 6.12, '2024-05-24', '2024-05-28', 22, NULL, 6, 2, 1, 2, NULL, NULL, 100, 50, 3.00, NULL),
('3422', '23490504', 'T.ACO GAL.RECT.', 'T.AÇO GALV.RECT.100x20x1.50', 'ML', 828.00, 'EUR', 
 7.51, 3.10, 2.90, '2024-01-08', '2024-06-13', 22, NULL, 6, 2, 1, 2, NULL, NULL, 100, 20, 1.50, NULL),
('3423', '23490602', 'T.ACO GAL.RECT.', 'T.AÇO GALV.RECT.100x60x2.00', 'ML', 972.00, 'EUR', 
 12.33, 4.95, 4.83, '2024-05-27', '2024-05-31', 22, NULL, 6, 2, 1, 2, NULL, NULL, 100, 60, 2.00, NULL),
('3424', '23490603', 'T.ACO GAL.RECT.', 'T.AÇO GALV.RECT.120x60x2.00', 'ML', 0.00, 'EUR', 
 11.42, 0.00, 0.00, '1900-01-01', '1900-01-01', 21, NULL, 6, 2, 1, 2, NULL, NULL, 120, 60, 2.00, NULL),
('3425', '23492602', 'T.ACO GAL.RECT.', 'T.AÇO GALV.RECT.120x60x2.00', 'MT', 1248.00, 'EUR', 
 13.00, 5.20, 5.10, '2024-05-22', '2024-06-14', 22, NULL, 6, 2, 1, 2, NULL, NULL, 120, 60, 2.00, NULL),
('3426', '23492603', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.120x60x3.00', 'MT', 120.00, 'EUR', 
 18.95, 7.88, 7.88, '2024-05-31', '2024-06-12', 22, NULL, 6, 2, 1, 2, NULL, NULL, 120, 60, 3.00, NULL),
('3427', '23494803', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.140x80x3', 'MT', 300.00, 'EUR', 
 23.80, 9.50, 9.48, '2024-05-31', '2024-05-23', 22, NULL, 6, 2, 1, 2, NULL, NULL, 140, 80, 3.00, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter
) VALUES
('3428', '23495993', 'T.AÇO GALV.RECT.', 'T.AÇO GALV.RECT.200x100x3,00', 'MT', 660.00, 'EUR', 
 34.16, 13.83, 14.21, '2024-05-21', '2024-06-11', 22, NULL, 6, 2, 1, 2, NULL, NULL, 200, 100, 3.00, NULL),
('3429', '23496803', 'T.ACO GAL.RECT.', 'T.ACO GAL.RECT.160x80x3,00', 'MT', 810.00, 'EUR', 
 26.90, 10.80, 11.10, '2024-05-31', '2024-06-11', 22, NULL, 6, 2, 1, 2, NULL, NULL, 160, 80, 3.00, NULL),
('3482', '23523015', 'TUBO INOX RECT.', 'TUBO INOX RECT.304 30x15x1,50', 'MT', 0.00, 'EUR', 
 3.73, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 30, 15, 1.50, NULL),
('3483', '23524010', 'TUBO INOX RECT.', 'TUBO INOX RECT 304 40x10x1,50', 'MT', 0.00, 'EUR', 
 4.17, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 40, 10, 1.50, NULL),
('3485', '23524012', 'TUBO INOX RECT.', 'TUBO INOX 304 RECT.40x30x1,50', 'ML', 0.00, 'EUR', 
 4.42, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 40, 30, 1.50, NULL),
('3486', '23524015', 'TUBO INOX RECT.', 'TUBO INOX RECT.304 40x15x1,50', 'MT', 0.00, 'EUR', 
 3.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 40, 15, 1.50, NULL),
('3487', '23524020', 'TUBO INOX RECT.', 'TUBO INOX RECT.304 40x20x1,50', 'MT', 0.00, 'EUR', 
 8.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 40, 20, 1.50, NULL),
('3488', '23525025', 'TUBO INOX RECT.', 'TUBO INOX RECT.304 50x25x1,50', 'MT', 0.00, 'EUR', 
 3.24, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 50, 25, 1.50, NULL),
('3489', '23525030', 'TUBO INOX RECT.', 'TUBO INOX 304 RECT.50x30x1,50', 'ML', 0.00, 'EUR', 
 7.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 50, 30, 1.50, NULL),
('3490', '23526020', 'TUBO INOX RECT.', 'TUBO INOX RECT.304 60x10x1,50', 'MT', 0.00, 'EUR', 
 4.48, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 60, 10, 1.50, NULL),
('3491', '23526040', 'TUBO INOX RECT.', 'TUBO INOX RECT.304 60x40x1,50', 'MT', 0.00, 'EUR', 
 5.97, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 60, 40, 1.50, NULL),
('3492', '23528040', 'TUBO INOX RECT.', 'TUBO INOX RECT.304 80x40x1,50', 'MT', 0.00, 'EUR', 
 7.35, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 80, 40, 1.50, NULL),
('3493', '23528042', 'TUBO INOX RECT.', 'TUBO RECT.INOX 304 80x40x2', 'MT', 0.00, 'EUR', 
 16.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 80, 40, 2.00, NULL),
('3494', '23528043', 'TUBO INOX RECT.', 'TUBO RECT.INOX 304 80x40x3', 'MT', 0.00, 'EUR', 
 24.81, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 4, 1, NULL, NULL, NULL, 80, 40, 3.00, NULL),
('7583', '46000017', 'TUBO PVC RECT.', 'TUBO RECTANG.PVC 80x40', 'MT', 0.00, 'EUR', 
 2.36, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 6, 1, NULL, NULL, NULL, 80, 40, NULL, NULL),
('10499', '70092151', 'TUBO COBRE RECT.', 'TUBO DE COBRE RECTO 1/2', 'UN', 0.00, 'EUR', 
 4.91, 2.41, 2.41, '2017-10-10', '1900-01-01', 62, NULL, 6, 5, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);



-- TUBOS CANALIZAÇÃO

ALTER TABLE mf_product_catalog ADD COLUMN nominal_dimension VARCHAR(50) NULL;
ALTER TABLE t_product_catalog ADD COLUMN nominal_dimension VARCHAR(50) NULL;


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('4083', '29106023', 'TUBO FERRO PRETO S/L', 'TUBO FERRO PRETO S/L 2\" 60,3x2,3', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 2.3, 60.3, 'DN50'),
('4084', '30000013', 'TUBO GALV. S/M', 'TUBO GALV. S/M 1/4', 'MT', 0.00, 'EUR', 
 2.96, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 13.5, 2.4, 13.5, 'DN8'),
('4085', '30000017', 'TUBO GALV. S/M', 'TUBO GALV. S/M 3/8', 'MT', 0.00, 'EUR', 
 2.96, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 17.2, 2.4, 17.2, 'DN10'),
('4086', '30000021', 'TUBO GALV. S/M', 'TUBO GALV. S/M 1/2', 'MT', 1146.00, 'EUR', 
 3.96, 1.53, 1.49, '2024-05-02', '2024-06-06', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 21.3, 2.6, 21.3, 'DN15'),
('4087', '30000026', 'TUBO GALV. S/M', 'TUBO GALV. S/M 3/4', 'MT', 612.00, 'EUR', 
 4.81, 1.92, 1.91, '2024-01-08', '2024-06-13', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 26.9, 2.6, 26.9, 'DN20'),
('4088', '30000033', 'TUBO GALV. S/M', 'TUBO GALV. S/M 1\"', 'MT', 732.00, 'EUR', 
 7.16, 2.64, 2.64, '2024-06-07', '2024-06-13', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 33.7, 3.2, 33.7, 'DN25'),
('4089', '30000042', 'TUBO GALV. S/M', 'TUBO GALV. S/M 1 1/4\"', 'MT', 312.00, 'EUR', 
 9.15, 3.68, 3.73, '2024-02-22', '2024-06-14', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 42.4, 3.2, 42.4, 'DN32'),
('4090', '30000048', 'TUBO GALV. S/M', 'TUBO GALV. S/M 1 1/2\"', 'MT', 564.00, 'EUR', 
 10.63, 4.09, 4.08, '2024-04-23', '2024-05-23', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 48.3, 3.2, 48.3, 'DN40'),
('4091', '30000060', 'TUBO GALV. S/M', 'TUBO GALV. S/M 2\"', 'MT', 972.00, 'EUR', 
 14.85, 5.84, 5.70, '2024-04-08', '2024-05-16', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 60.3, 3.6, 60.3, 'DN50'),
('4092', '30000075', 'TUBO GALV. S/M', 'TUBO GALV. S/M 2 1/2\"', 'MT', 306.00, 'EUR', 
 19.16, 8.56, 8.56, '2023-02-10', '2024-06-12', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 76.1, 3.6, 76.1, 'DN65'),
('4093', '30000089', 'TUBO GALV. S/M', 'TUBO GALV. S/M 3\"', 'MT', 210.00, 'EUR', 
 24.91, 8.43, 9.57, '2024-04-23', '2024-05-28', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 88.9, 4.0, 88.9, 'DN80'),
('4094', '30000101', 'TUBO GALV. S/M', 'TUBO GALV. S/M 3 1/2\"', 'MT', 120.00, 'EUR', 
 26.81, 10.57, 10.57, '2024-03-19', '2024-05-08', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 101.6, 4.0, 101.6, 'DN90'),
('4095', '30000115', 'TUBO GALV. S/M', 'TUBO GALV. S/M 4\"', 'MT', 96.00, 'EUR', 
 36.73, 14.98, 14.98, '2024-02-22', '2024-04-15', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 114.3, 4.0, 114.3, 'DN100'),
('4096', '30000140', 'TUBO GALV. S/M', 'TUBO GALV. S/M 5\"', 'MT', 60.00, 'EUR', 
 49.68, 21.33, 19.08, '2023-12-13', '2024-02-02', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 139.7, 5.0, 139.7, 'DN125'),
('4097', '30000166', 'TUBO GALV. S/M', 'TUBO GALV. S/M 6\"', 'MT', 72.00, 'EUR', 
 59.09, 20.62, 21.74, '2024-05-31', '2024-05-17', 31, NULL, 6, 1, 15, 2, 7, NULL, NULL, 165.1, 5.0, 165.1, 'DN150');




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('4098', '31000013', 'TUBO PRETO S/M', 'TUBO PRETO S/M 1/4\"', 'MT', 0.00, 'EUR', 
 2.26, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 13.5, 2.4, 13.5, 'DN8'),
('4099', '31000017', 'TUBO PRETO S/M', 'TUBO PRETO S/M 3/8\"', 'MT', 0.00, 'EUR', 
 2.26, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 17.2, 2.4, 17.2, 'DN10'),
('4100', '31000021', 'TUBO PRETO S/M', 'TUBO PRETO S/M 1/2\"', 'MT', 1002.00, 'EUR', 
 3.01, 1.29, 1.24, '2024-02-09', '2024-06-13', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 21.3, 2.6, 21.3, 'DN15'),
('4102', '31000026', 'TUBO PRETO S/M', 'TUBO PRETO S/M 3/4\"', 'MT', 1104.00, 'EUR', 
 3.59, 1.42, 1.38, '2024-06-07', '2024-06-12', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 26.9, 2.6, 26.9, 'DN20'),
('4103', '31000033', 'TUBO PRETO S/M', 'TUBO PRETO S/M 1\"', 'MT', 1320.00, 'EUR', 
 5.33, 2.11, 2.04, '2024-06-07', '2024-06-13', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 33.7, 3.2, 33.7, 'DN25'),
('4104', '31000042', 'TUBO PRETO S/M', 'TUBO PRETO S/M 1 1/4\"', 'MT', 690.00, 'EUR', 
 6.76, 2.86, 2.71, '2024-04-26', '2024-06-12', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 42.4, 3.2, 42.4, 'DN32'),
('4105', '31000048', 'TUBO PRETO S/M', 'TUBO PRETO S/M 1 1/2\"', 'MT', 1014.00, 'EUR', 
 7.79, 3.10, 2.99, '2024-05-31', '2024-06-11', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 48.3, 3.2, 48.3, 'DN40'),
('4106', '31000060', 'TUBO PRETO S/M', 'TUBO PRETO S/M 2\"', 'MT', 594.00, 'EUR', 
 10.97, 4.57, 4.39, '2024-05-09', '2024-05-14', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 60.3, 3.6, 60.3, 'DN50'),
('4107', '31000075', 'TUBO PRETO S/M', 'TUBO PRETO S/M 2 1/2\"', 'MT', 252.00, 'EUR', 
 14.20, 6.89, 6.59, '2023-02-10', '2024-04-11', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 76.1, 3.6, 76.1, 'DN65'),
('4108', '31000089', 'TUBO PRETO S/M', 'TUBO PRETO S/M 3\"', 'MT', 269.50, 'EUR', 
 18.54, 7.71, 7.56, '2024-04-12', '2024-06-06', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 88.9, 4.0, 88.9, 'DN80'),
('4109', '31000101', 'TUBO PRETO S/M', 'TUBO PRETO S/M 3 1/2\"', 'MT', 66.00, 'EUR', 
 20.31, 8.87, 8.94, '2024-02-08', '2024-06-11', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 101.6, 4.0, 101.6, 'DN90');


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('4112', '31000115', 'TUBO PRETO S/M', 'TUBO PRETO S/M 4\"', 'MT', 180.00, 'EUR', 
 27.53, 10.87, 10.57, '2024-05-31', '2024-05-28', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 114.3, 4.0, 114.3, 'DN100'),
('4113', '31000140', 'TUBO PRETO S/M', 'TUBO PRETO S/M 5\"', 'MT', 194.00, 'EUR', 
 37.48, 15.34, 15.29, '2024-04-12', '2024-05-15', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 139.7, 5.0, 139.7, 'DN125'),
('4114', '31000166', 'TUBO PRETO S/M', 'TUBO PRETO S/M 6\"', 'MT', 48.00, 'EUR', 
 44.70, 18.35, 18.42, '2024-01-08', '2024-06-12', 31, NULL, 6, 1, 15, 3, 7, NULL, NULL, 165.1, 5.0, 165.1, 'DN150');



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimensions
) VALUES
('4121', '32000013', 'TUBO GALV. S/L', 'TUBO GALV. S/L 1/4\"', 'MT', 0.00, 'EUR', 
 2.40, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 1.8, 13.5, 'DN8'),
('4122', '32000017', 'TUBO GALV. S/L', 'TUBO GALV. S/L 3/8\"', 'MT', 0.00, 'EUR', 
 2.40, 0.81, 0.81, '2019-09-18', '2021-03-29', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 1.8, 17.2, 'DN10'),
('4123', '32000021', 'TUBO GALV. S/L', 'TUBO GALV. S/L 1/2\"', 'MT', 1308.00, 'EUR', 
 3.10, 1.23, 1.14, '2024-06-07', '2024-06-07', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.0, 21.3, 'DN15'),
('4124', '32000026', 'TUBO GALV. S/L', 'TUBO GALV. S/L 3/4\"', 'MT', 1020.00, 'EUR', 
 4.25, 1.61, 1.61, '2023-11-21', '2024-06-11', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.3, 26.9, 'DN20'),
('4125', '32000033', 'TUBO GALV. S/L', 'TUBO GALV. S/L 1\"', 'MT', 1140.00, 'EUR', 
 5.90, 2.26, 2.17, '2024-06-07', '2024-06-13', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.6, 33.7, 'DN25'),
('4126', '32000042', 'TUBO GALV. S/L', 'TUBO GALV. S/L 1 1/4\"', 'MT', 1158.00, 'EUR', 
 7.50, 2.86, 2.76, '2024-06-07', '2024-06-07', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.6, 42.4, 'DN32'),
('4127', '32000048', 'TUBO GALV. S/L', 'TUBO GALV. S/L 1 1/2\"', 'MT', 996.00, 'EUR', 
 9.63, 3.55, 3.54, '2024-06-07', '2024-06-12', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.9, 48.3, 'DN40'),
('4128', '32000060', 'TUBO GALV. S/L', 'TUBO GALV. S/L 2\"', 'MT', 618.00, 'EUR', 
 12.05, 4.76, 4.63, '2024-04-08', '2024-06-12', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.9, 60.3, 'DN50'),
('4129', '32000075', 'TUBO GALV. S/L', 'TUBO GALV. S/L 2 1/2\"', 'MT', 306.00, 'EUR', 
 17.06, 6.40, 6.28, '2024-06-07', '2024-06-12', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 3.6, 76.1, 'DN65'),
('4130', '32000089', 'TUBO GALV. S/L', 'TUBO GALV. S/L 3\"', 'MT', 102.00, 'EUR', 
 19.99, 7.61, 7.36, '2024-06-07', '2024-06-12', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 4.0, 88.9, 'DN80'),
('4131', '32000101', 'TUBO GALV. S/L', 'TUBO GALV. S/L 3 1/2\"', 'MT', 180.00, 'EUR', 
 24.26, 10.58, 10.58, '2024-02-08', '2024-02-23', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 4.0, 101.6, 'DN90'),
('4132', '32000115', 'TUBO GALV. S/L', 'TUBO GALV. S/L 4\"', 'MT', 120.00, 'EUR', 
 29.35, 13.20, 12.91, '2024-02-08', '2024-04-12', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 4.5, 114.3, 'DN100'),
('4133', '32000140', 'TUBO GALV. S/L', 'TUBO GALV. S/L 5\"', 'MT', 0.00, 'EUR', 
 37.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 5.0, 139.7, 'DN125'),
('4134', '32000166', 'TUBO GALV. S/L', 'TUBO GALV. S/L 6\"', 'MT', 0.00, 'EUR', 
 45.44, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 5.0, 165.1, 'DN150');


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('4121', '32000013', 'TUBO GALV. S/L', 'TUBO GALV. S/L 1/4\"', 'MT', 0.00, 'EUR', 
 2.40, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 1.8, 13.5, 'DN8'),
('4122', '32000017', 'TUBO GALV. S/L', 'TUBO GALV. S/L 3/8\"', 'MT', 0.00, 'EUR', 
 2.40, 0.81, 0.81, '2019-09-18', '2021-03-29', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 1.8, 17.2, 'DN10'),
('4123', '32000021', 'TUBO GALV. S/L', 'TUBO GALV. S/L 1/2\"', 'MT', 1308.00, 'EUR', 
 3.10, 1.23, 1.14, '2024-06-07', '2024-06-07', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.0, 21.3, 'DN15'),
('4124', '32000026', 'TUBO GALV. S/L', 'TUBO GALV. S/L 3/4\"', 'MT', 1020.00, 'EUR', 
 4.25, 1.61, 1.61, '2023-11-21', '2024-06-11', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.3, 26.9, 'DN20'),
('4125', '32000033', 'TUBO GALV. S/L', 'TUBO GALV. S/L 1\"', 'MT', 1140.00, 'EUR', 
 5.90, 2.26, 2.17, '2024-06-07', '2024-06-13', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.6, 33.7, 'DN25'),
('4126', '32000042', 'TUBO GALV. S/L', 'TUBO GALV. S/L 1 1/4\"', 'MT', 1158.00, 'EUR', 
 7.50, 2.86, 2.76, '2024-06-07', '2024-06-07', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.6, 42.4, 'DN32'),
('4127', '32000048', 'TUBO GALV. S/L', 'TUBO GALV. S/L 1 1/2\"', 'MT', 996.00, 'EUR', 
 9.63, 3.55, 3.54, '2024-06-07', '2024-06-12', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.9, 48.3, 'DN40'),
('4128', '32000060', 'TUBO GALV. S/L', 'TUBO GALV. S/L 2\"', 'MT', 618.00, 'EUR', 
 12.05, 4.76, 4.63, '2024-04-08', '2024-06-12', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 2.9, 60.3, 'DN50'),
('4129', '32000075', 'TUBO GALV. S/L', 'TUBO GALV. S/L 2 1/2\"', 'MT', 306.00, 'EUR', 
 17.06, 6.40, 6.28, '2024-06-07', '2024-06-12', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 3.2, 76.1, 'DN65'),
('4130', '32000089', 'TUBO GALV. S/L', 'TUBO GALV. S/L 3\"', 'MT', 102.00, 'EUR', 
 19.99, 7.61, 7.36, '2024-06-07', '2024-06-12', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 3.2, 88.9, 'DN80'),
('4131', '32000101', 'TUBO GALV. S/L', 'TUBO GALV. S/L 3 1/2\"', 'MT', 180.00, 'EUR', 
 24.26, 10.58, 10.58, '2024-02-08', '2024-02-23', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 3.6, 101.6, 'DN90'),
('4132', '32000115', 'TUBO GALV. S/L', 'TUBO GALV. S/L 4\"', 'MT', 120.00, 'EUR', 
 29.35, 13.20, 12.91, '2024-02-08', '2024-04-12', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 3.6, 114.3, 'DN100'),
('4133', '32000140', 'TUBO GALV. S/L', 'TUBO GALV. S/L 5\"', 'MT', 0.00, 'EUR', 
 37.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 4, 139.7, 'DN125'),
('4134', '32000166', 'TUBO GALV. S/L', 'TUBO GALV. S/L 6\"', 'MT', 0.00, 'EUR', 
 45.44, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 2, 8, NULL, NULL, NULL, 4, 165.1, 'DN150');


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('4135', '33000013', 'TUBO PRETO S/L', 'TUBO PRETO S/L 1/4\"', 'MT', 0.00, 'EUR', 
 1.84, 0.00, 0.00, '1900-01-01', '1900-01-01', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 1.8, 13.5, 'DN8'),
('4136', '33000017', 'TUBO PRETO S/L', 'TUBO PRETO S/L 3/8\"', 'MT', 0.00, 'EUR', 
 1.84, 0.62, 0.63, '2018-06-30', '2020-04-21', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 1.8, 17.2, 'DN10'),
('4137', '33000021', 'TUBO PRETO S/L', 'TUBO PRETO S/L 1/2\"', 'MT', 1860.00, 'EUR', 
 2.36, 0.96, 0.95, '2024-05-08', '2024-06-06', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 2.0, 21.3, 'DN15'),
('4138', '33000026', 'TUBO PRETO S/L', 'TUBO PRETO S/L 3/4\"', 'MT', 966.00, 'EUR', 
 3.16, 1.28, 1.27, '2024-05-09', '2024-06-12', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 2.3, 26.9, 'DN20'),
('4139', '33000033', 'TUBO PRETO S/L', 'TUBO PRETO S/L 1\"', 'MT', 810.00, 'EUR', 
 4.38, 1.84, 1.80, '2024-02-09', '2024-06-12', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 2.6, 33.7, 'DN25'),
('4140', '33000042', 'TUBO PRETO S/L', 'TUBO PRETO S/L 1 1/4\"', 'MT', 1494.00, 'EUR', 
 5.55, 2.28, 2.26, '2024-04-08', '2024-06-07', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 2.6, 42.4, 'DN32'),
('4141', '33000048', 'TUBO PRETO S/L', 'TUBO PRETO S/L 1 1/2\"', 'MT', 702.00, 'EUR', 
 7.05, 2.89, 2.88, '2024-04-11', '2024-06-12', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 2.9, 48.3, 'DN40'),
('4142', '33000060', 'TUBO PRETO S/L', 'TUBO PRETO S/L 2\"', 'MT', 678.00, 'EUR', 
 8.91, 3.70, 3.64, '2024-03-19', '2024-06-13', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 2.9, 60.3, 'DN50'),
('4143', '33000075', 'TUBO PRETO S/L', 'TUBO PRETO S/L 2 1/2\"', 'MT', 432.00, 'EUR', 
 12.64, 5.12, 5.06, '2024-04-26', '2024-06-06', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 3.2, 76.1, 'DN65'),
('4144', '33000090', 'TUBO PRETO S/L', 'TUBO PRETO S/L 3\"', 'MT', 252.00, 'EUR', 
 14.89, 6.00, 5.72, '2024-05-31', '2024-05-28', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 3.2, 88.9, 'DN80'),
('4145', '33000101', 'TUBO PRETO S/L', 'TUBO PRETO S/L 3 1/2\"', 'MT', 156.00, 'EUR', 
 17.91, 7.05, 6.88, '2024-05-31', '2024-05-08', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 3.6, 101.6, 'DN90'),
('4146', '33000115', 'TUBO PRETO S/L', 'TUBO PRETO S/L 4\"', 'MT', 132.00, 'EUR', 
 22.00, 8.62, 8.45, '2024-05-31', '2024-06-04', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 3.6, 114.3, 'DN100'),
('4147', '33000140', 'TUBO PRETO S/L', 'TUBO PRETO S/L 5\"', 'MT', 48.00, 'EUR', 
 27.76, 13.10, 13.33, '2023-02-28', '2023-12-15', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 4.0, 139.7, 'DN125'),
('4148', '33000166', 'TUBO PRETO S/L', 'TUBO PRETO S/L 6\"', 'MT', 36.00, 'EUR', 
 33.24, 14.09, 14.09, '2024-02-22', '2024-06-11', 31, NULL, 6, 1, 15, 3, 8, NULL, NULL, NULL, 4.0, 165.1, 'DN150');


-- update tubos de canalizaçao
UPDATE
    t_product_catalog
SET
    width = diameter,
    height = diameter,
    length = 6000
WHERE
    type_id = 6
    AND shape_id = 15;

-- update varoes
UPDATE t_product_catalog 
SET 
    nominal_dimension = 'A500'
WHERE
    type_id = 1
        AND (description LIKE '%A 500%'
        OR description_full LIKE '%A 500%');
        
        
        
UPDATE t_product_catalog 
SET 
    nominal_dimension = 'A400'
WHERE
    type_id = 1
        AND (description LIKE '%A 400%'
        OR description_full LIKE '%A 400%');

UPDATE t_product_catalog 
SET 
    nominal_dimension = 'A235'
WHERE
    type_id = 1
        AND (description LIKE '%A 235%'
        OR description_full LIKE '%A 235%');


-- tubos decorativos

INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('4051', '27100030', 'TUBO DECOR. Y/100', 'TUBO DECOR. Y/100 30x1,5', 'UN', 0.00, 'EUR', 
 5.66, 2.09, 2.09, '2017-10-10', '2018-12-03', 25, NULL, 6, 1, 14, NULL, NULL, NULL, NULL, NULL, 1.5, 30, 'Y/100'),
('4052', '27100030', 'TUBO DECOR. Y/100', 'TUBO DECOR. Y/100 30x1,5', 'UN', 0.00, 'PTE', 
 625.00, 2.09, 2.09, '2017-10-10', '2018-12-03', 25, NULL, 6, 1, 14, NULL, NULL, NULL, NULL, NULL, 1.5, 30, 'Y/100'),
('4053', '27100035', 'TUBO DECOR. Y/100', 'TUBO DECOR. Y/100 35x1,5', 'UN', 0.00, 'EUR', 
 6.39, 2.35, 2.35, '2017-10-10', '2018-12-03', 25, NULL, 6, 1, 14, NULL, NULL, NULL, NULL, NULL, 1.5, 35, 'Y/100'),
('4054', '27100035', 'TUBO DECOR. Y/100', 'TUBO DECOR. Y/100 35x1,5', 'UN', 0.00, 'PTE', 
 705.00, 2.35, 2.35, '2017-10-10', '2018-12-03', 25, NULL, 6, 1, 14, NULL, NULL, NULL, NULL, NULL, 1.5, 35, 'Y/100'),
('4055', '27150035', 'TUBO DECOR. Y/150', 'TUBO DECOR. Y/150 35x1,5', 'UN', 0.00, 'EUR', 
 9.61, 0.00, 0.00, '1900-01-01', '1900-01-01', 25, NULL, 6, 1, 14, NULL, NULL, NULL, NULL, NULL, 1.5, 35, 'Y/150'),
('4056', '27150035', 'TUBO DECOR. Y/150', 'TUBO DECOR. Y/150 35x1,5', 'UN', 0.00, 'PTE', 
 1060.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 25, NULL, 6, 1, 14, NULL, NULL, NULL, NULL, NULL, 1.5, 35, 'Y/150'),
('4057', '27580020', 'TUBO DECOR. F/580', 'TUBO DECOR. F/580 20x1,5', 'UN', 20.00, 'EUR', 
 51.43, 4.30, 4.30, '2022-12-12', '2024-02-15', 25, NULL, 6, 1, 14, NULL, NULL, NULL, NULL, NULL, 1.5, 20, 'F/580'),
('4058', '27580025', 'TUBO DECOR. F/580', 'TUBO DECOR. F/580 25x1,5', 'UN', 20.00, 'EUR', 
 55.06, 35.06, 35.06, '2023-11-08', '2024-05-09', 25, NULL, 6, 1, 14, NULL, NULL, NULL, NULL, NULL, 1.5, 25, 'F/580'),
('4059', '27580030', 'TUBO DECOR. F/580', 'TUBO DECOR. F/580 30x1,5', 'UN', 28.00, 'EUR', 
 60.99, 42.09, 42.09, '2023-11-08', '2024-03-26', 25, NULL, 6, 1, 14, NULL, NULL, NULL, NULL, NULL, 1.5, 30, 'F/580'),
('4060', '27580035', 'TUBO DECOR. F/580', 'TUBO DECOR. F/580 35x1,5', 'UN', 0.00, 'EUR', 
 76.96, 19.07, 19.07, '2017-10-10', '2020-06-15', 25, NULL, 6, 1, 14, NULL, NULL, NULL, NULL, NULL, 1.5, 35, 'F/580');



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('4061', '27581030', 'TUBO DECOR. F/580', 'TUBO DECOR. F/580 30x1,5 GALV.', 'UN', 0.00, 'EUR', 
 64.61, 0.00, 0.00, '1900-01-01', '1900-01-01', 25, NULL, 6, 1, 14, 2, NULL,NULL, NULL, NULL, 1.5, 30, 'F/580'),
('4062', '27581030', 'TUBO DECOR. F/580', 'TUBO DECOR. F/580 30x1,5 GALV.', 'UN', 0.00, 'PTE', 
 2100.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 25, NULL, 6, 1, 14, 2, NULL,NULL, NULL, NULL, 1.5, 30, 'F/580'),
('4063', '27594025', 'TUBO DECOR. H/582', 'TUBO DECOR. H/582 x 25mm', 'UN', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 25, NULL, 6, 1, 14, NULL, NULL,NULL, NULL, NULL, NULL, 25, 'H/582'),
('4064', '27595020', 'TUBO DECOR. H/595', 'TUBO DECOR. H/595 20x1,5', 'UN', 3.00, 'EUR', 
 51.43, 11.07, 11.07, '2017-10-10', '2021-08-12', 25, NULL, 6, 1, 14, NULL, NULL,NULL, NULL, NULL, 1.5, 20, 'H/595'),
('4065', '27595025', 'TUBO DECOR. H/595', 'TUBO DECOR. H/595 25x1,5', 'UN', 0.00, 'EUR', 
 55.06, 12.30, 12.30, '2017-12-29', '2018-05-30', 25, NULL, 6, 1, 14, NULL, NULL,NULL, NULL, NULL, 1.5, 25, 'H/595'),
('4066', '27595030', 'TUBO DECOR. H/595', 'TUBO DECOR. H/595 30x1,5', 'UN', 0.00, 'EUR', 
 60.99, 10.46, 10.46, '2017-10-10', '1900-01-01', 25, NULL, 6, 1, 14, NULL, NULL,NULL, NULL, NULL, 1.5, 30, 'H/595'),
('4067', '27595035', 'TUBO DECOR. H/595', 'TUBO DECOR. H/595 35x1,5', 'UN', 5.00, 'EUR', 
 76.60, 19.16, 19.16, '2017-10-10', '2024-03-20', 25, NULL, 6, 1, 14, NULL, NULL,NULL, NULL, NULL, 1.5, 35, 'H/595'),
('4068', '27700020', 'TUBO DECOR. 5,80', 'TUBO DECOR. 5,80 T-20', 'UN', 6.00, 'EUR', 
 51.43, 6.15, 6.15, '2017-10-10', '2023-01-10', 25, NULL, 6, 1, 14, NULL, NULL, NULL,NULL, NULL, 5.80 , 20, 'T-20'),
('4069', '27700025', 'TUBO DECOR. 5,80', 'TUBO DECOR. 5,80 T-25', 'UN', 0.00, 'EUR', 
 55.06, 43.95, 43.95, '2023-03-13', '2023-03-16', 25, NULL, 6, 1, 14, NULL, NULL,NULL, NULL, NULL, 5.80 , 25, 'T-25'),
('4070', '27700030', 'TUBO DECOR. 5,80', 'TUBO DECOR. 5,80 T-30', 'UN', 11.00, 'EUR', 
 60.99, 37.43, 37.43, '2023-05-02', '2024-04-05', 25, NULL, 6, 1, 14, NULL, NULL,NULL, NULL, NULL, 5.80 , 30, 'T-30'),
('4071', '27700035', 'TUBO DECOR. 5,80', 'TUBO DECOR. 5,80 T-35', 'UN', 0.00, 'EUR', 
 76.96, 9.95, 9.95, '2019-05-02', '2020-04-17', 25, NULL, 6, 1, 14, NULL, NULL,NULL, NULL, NULL, 5.80 , 35, 'T-35'),
('4072', '27710030', 'TUBO DECOR. 5,80 GALV.', 'TUBO DECOR. 5,80 T-30 GALV.', 'UN', 0.00, 'EUR', 
 76.96, 0.00, 0.00, '1900-01-01', '1900-01-01', 25, NULL, 6, 1, 14, 2, NULL, NULL,NULL, NULL, 5.80 ,30, 'GALV T-30'),
('4073', '27710030', 'TUBO DECOR. 5,80 GALV.', 'TUBO DECOR. 5,80 T-30 GALV.', 'UN', 0.00, 'PTE', 
 2050.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 25, NULL, 6, 1, 14, 2, NULL,NULL, NULL, NULL, 5.80 , 30, 'GALV T-30');




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('3293', '23000223', 'TUBO CORRIMAO', 'TUBO CORRIMAO 223', 'MT', 444.00, 'EUR', 
 5.16, 2.30, 2.06, '2023-09-25', '2024-06-12', 24, NULL, 6, 1, 12, NULL, NULL, NULL, NULL,NULL, NULL, NULL, '223'),
('3294', '23000345', 'TUBO CORRIMAO', 'TUBO CORRIMAO 345', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 24, NULL, 6, 1, 12, NULL, NULL, NULL, NULL,NULL, NULL, NULL, '345'),
('3295', '23000346', 'TUBO CORRIMAO', 'TUBO CORRIMAO 346', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 24, NULL, 6, 1, 12, NULL, NULL, NULL, NULL,NULL, NULL, NULL, '346'),
('3296', '23000348', 'TUBO CORRIMAO', 'TUBO CORRIMAO 348', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 24, NULL, 6, 1, 12, NULL, NULL, NULL, NULL, NULL,NULL, NULL, '348'),
('3297', '23000627', 'TUBO CORRIMAO', 'TUBO CORRIMAO 627', 'MT', 348.00, 'EUR', 
 4.52, 2.04, 2.04, '2022-12-12', '2024-04-22', 24, NULL, 6, 1, 12, NULL, NULL, NULL, NULL, NULL,NULL, NULL, '627'),
('3298', '23000628', 'TUBO CORRIMAO', 'TUBO CORRIMAO 628', 'MT', 396.00, 'EUR', 
 4.23, 2.06, 2.06, '2022-12-12', '2024-05-29', 24, NULL, 6, 1, 12, NULL, NULL, NULL, NULL,NULL, NULL, NULL, '628'),
('3299', '23000629', 'TUBO CORRIMAO', 'TUBO CORRIMAO 629', 'MT', 372.00, 'EUR', 
 3.91, 1.57, 1.57, '2023-03-30', '2024-06-12', 24, NULL, 6, 1, 12, NULL, NULL, NULL, NULL, NULL,NULL, NULL, '629');








-- REDE

-- REVER AS MEDIDAS!!


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1801', '19141108', 'REDE NO (OVELHEIRA)', 'REDE NO (OVELHEIRA) C/ 2,00MT', 'MT', 0.00, 'EUR', 
 2.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 17, NULL, NULL, 2.00, NULL, NULL, NULL, NULL, NULL),
('1802', '19141109', 'REDE NO', 'REDE NO 1,20x9x30', 'MT', 0.00, 'EUR', 
 2.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 17, NULL, NULL, 1.20, 9.00, 30.00, NULL, NULL, NULL),
('1803', '19141110', 'REDE NO', 'REDE NO 1,00x9x15', 'MT', 0.00, 'EUR', 
 1.32, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 17, NULL, NULL, 1.00, 9.00, 15.00, NULL, NULL, NULL),
('1804', '19141111', 'REDE NO', 'REDE NO 1,20x9x15', 'MT', 1500.00, 'EUR', 
 1.36, 0.60, 0.59, '2024-04-03', '2024-05-09', 15, NULL, 9, 1, 17, NULL, NULL, 1.20, 9.00, 15.00, NULL, NULL, NULL),
('1805', '19141112', 'REDE NO', 'REDE NO 1,20x10x15', 'MT', 0.00, 'EUR', 
 1.41, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 17, NULL, NULL, 1.20, 10.00, 15.00, NULL, NULL, NULL),
('1806', '19141114', 'REDE NO', 'REDE NO 1,30x10x15', 'MT', 0.00, 'EUR', 
 1.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 17, NULL, NULL, 1.30, 10.00, 15.00, NULL, NULL, NULL),
('1807', '19141115', 'REDE NO', 'REDE NO 1,40x11x15', 'MT', 1150.00, 'EUR', 
 1.53, 0.71, 0.71, '2024-04-03', '2024-05-21', 15, NULL, 9, 1, 17, NULL, NULL, 1.40, 11.00, 15.00, NULL, NULL, NULL),
('1808', '19141116', 'REDE NO', 'REDE NO 1,00x8x15', 'MT', 2600.00, 'EUR', 
 1.24, 0.51, 0.50, '2023-09-26', '2024-05-16', 15, NULL, 9, 1, 17, NULL, NULL, 1.00, 8.00, 15.00, NULL, NULL, NULL),
('1809', '19141117', 'REDE NO', 'REDE NO 1,00x8x30', 'MT', 0.00, 'EUR', 
 1.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 17, NULL, NULL, 1.00, 8.00, 30.00, NULL, NULL, NULL),
('1810', '19141118', 'REDE NO', 'REDE NO 0,80x8x15', 'MT', 0.00, 'EUR', 
 1.15, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 17, NULL, NULL, 0.80, 8.00, 15.00, NULL, NULL, NULL),
('1811', '19141119', 'REDE NO', 'REDE NO 0,65x6x15', 'MT', 0.00, 'EUR', 
 0.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 17, NULL, NULL, 0.65, 6.00, 15.00, NULL, NULL, NULL),
('1812', '19141120', 'REDE NO', 'REDE NO 0,60x5x15', 'MT', 0.00, 'EUR', 
 0.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 17, NULL, NULL, 0.60, 5.00, 15.00, NULL, NULL, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1823', '19501110', 'REDE ZINCADA', 'REDE ZINCADA A11 M50', 'M2', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 11, 4,NULL,  NULL, NULL, NULL, NULL, 11.00, 'M50'),
('1824', '19501150', 'REDE ZINCADA', 'REDE ZINCADA A11 M50', 'M2', 0.00, 'EUR', 
 0.00, 2.42, 2.42, '2024-05-22', '2024-05-24', 15, NULL, 9, 1, 11, 4, NULL, NULL, NULL, NULL, NULL, 11.00, 'M50'),
('1825', '19501309', 'REDE ZINCADA', 'REDE ZINCADA A.13 M.60', 'M2', 0.00, 'EUR', 
 1.92, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 11, 4, NULL, NULL, NULL, NULL, NULL, 13.00, 'M60'),
('1826', '19501309', 'REDE ZINCADA', 'REDE ZINCADA A.13 M.60', 'UN', 0.00, 'EUR', 
 1.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 11, 4, NULL, NULL, NULL, NULL, NULL, 13.00, 'M60'),
('1827', '19501310', 'REDE ZINCADA', 'REDE ZINCADA A13 M/50', 'M2', 0.00, 'EUR', 
 2.28, 1.25, 1.25, '2017-10-10', '2018-11-27', 15, NULL, 9, 1, 11, 4, NULL, NULL, NULL, NULL, NULL, 13.00, 'M50'),
('1828', '19501311', 'REDE ZINCADA', 'REDE ZINCADA A/12 M/60', 'M2', 0.00, 'EUR', 
 2.37, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 11, 4, NULL, NULL, NULL, NULL, NULL, 12.00, 'M60'),
('1829', '19501411', 'REDE ZINCADA', 'REDE ZINCADA A12 M/50', 'M2', 0.00, 'EUR', 
 2.91, 2.25, 2.25, '2023-10-30', '2023-10-30', 15, NULL, 9, 1, 11, 4,NULL,  NULL, NULL, NULL, NULL, 12.00, 'M50'),
('1830', '19501412', 'REDE ZINCADA', 'REDE ZINCADA A14 M/50', 'M2', 0.00, 'EUR', 
 1.71, 0.94, 0.94, '2017-10-10', '2020-04-17', 15, NULL, 9, 1, 11, 4, NULL, NULL, NULL, NULL, NULL, 14.00, 'M50'),
('1831', '19501413', 'REDE ZINCADA', 'REDE ZINCADA A14 M/60', 'M2', 0.00, 'EUR', 
 1.43, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 11, 4, NULL, NULL, NULL, NULL, NULL, 14.00, 'M60'),
('1832', '19501414', 'REDE ZINCADA', 'REDE ZINCADA A12 M/40', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 11, 4,NULL,  NULL, NULL, NULL, NULL, 12.00, 'M40');



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1833', '19501520', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1/2" 1,25MT', 'M2', 0.00, 'EUR', 
 2.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, NULL,1.25, NULL, NULL, NULL, 12.70, NULL),
('1834', '19501521', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1/2" 0,50MT', 'M2', 200.00, 'EUR', 
 2.00, 0.79, 0.79, '2021-04-13', '2024-03-27', 15, NULL, 9, 8, 19, NULL,NULL, 0.50, NULL, NULL, NULL, 12.70, NULL),
('1835', '19501522', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1/2" 0,75MT', 'M2', 0.00, 'EUR', 
 2.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, NULL,0.75, NULL, NULL, NULL, 12.70, NULL),
('1836', '19501523', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1/2" 1,00MT', 'M2', 0.00, 'EUR', 
 2.00, 0.73, 0.73, '2021-02-26', '2024-05-28', 15, NULL, 9, 8, 19, NULL, NULL,1.00, NULL, NULL, NULL, 12.70, NULL),
('1837', '19501524', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1/2" 1,50MT', 'M2', 0.00, 'EUR', 
 2.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, NULL,1.50, NULL, NULL, NULL, 12.70, NULL),
('1838', '19501525', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1/2" 2,00MT', 'M2', 0.00, 'EUR', 
 2.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL,NULL, 2.00, NULL, NULL, NULL, 12.70, NULL),
('1839', '19501526', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 3/4" 0,50MT', 'M2', 500.00, 'EUR', 
 1.65, 0.92, 0.92, '2022-03-10', '2021-11-29', 15, NULL, 9, 8, 19, NULL, NULL,0.50, NULL, NULL, NULL, 19.05, NULL),
('1840', '19501527', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 3/4" 0,75MT', 'M2', 0.00, 'EUR', 
 1.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, NULL,0.75, NULL, NULL, NULL, 19.05, NULL),
('1841', '19501528', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 3/4" 1,00MT', 'M2', 0.00, 'EUR', 
 1.65, 0.94, 0.94, '2023-11-27', '2024-03-19', 15, NULL, 9, 8, 19, NULL, NULL,1.00, NULL, NULL, NULL, 19.05, NULL),
('1842', '19501529', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 3/4" 1,25MT', 'M2', 0.00, 'EUR', 
 1.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, NULL,1.25, NULL, NULL, NULL, 19.05, NULL),
('1843', '19501530', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 3/4" 1,50MT', 'M2', 0.00, 'EUR', 
 1.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, NULL,1.50, NULL, NULL, NULL, 19.05, NULL),
('1844', '19501531', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 3/4" 2,00MT', 'M2', 0.00, 'EUR', 
 1.65, 0.66, 0.66, '2018-02-15', '2018-02-20', 15, NULL, 9, 8, 19, NULL,NULL, 2.00, NULL, NULL, NULL, 19.05, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1845', '19501533', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1" 0,50MT', 'M2', 150.00, 'EUR', 
 1.35, 0.47, 0.47, '2020-08-26', '2021-11-19', 15, NULL, 9, 8, 19, NULL, 0.50, NULL, NULL, NULL, 25.40, NULL),
('1846', '19501534', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1" 0,75MT', 'M2', 0.00, 'EUR', 
 1.35, 0.32, 0.32, '2017-10-10', '2017-12-19', 15, NULL, 9, 8, 19, NULL, 0.75, NULL, NULL, NULL, 25.40, NULL),
('1847', '19501535', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1" 1,00MT', 'M2', 400.00, 'EUR', 
 1.35, 0.76, 0.76, '2023-02-10', '2024-01-12', 15, NULL, 9, 8, 19, NULL, 1.00, NULL, NULL, NULL, 25.40, NULL),
('1848', '19501536', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1" 1,25MT', 'M2', 0.00, 'EUR', 
 1.35, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 1.25, NULL, NULL, NULL, 25.40, NULL),
('1849', '19501537', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1" 1,50MT', 'M2', 450.00, 'EUR', 
 1.35, 0.76, 0.76, '2023-11-27', '2024-05-28', 15, NULL, 9, 8, 19, NULL, 1.50, NULL, NULL, NULL, 25.40, NULL),
('1850', '19501538', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 1" 2,00MT', 'M2', 0.00, 'EUR', 
 1.35, 0.54, 0.54, '2020-08-26', '2021-11-22', 15, NULL, 9, 8, 19, NULL, 2.00, NULL, NULL, NULL, 25.40, NULL),
('1851', '19501542', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/4" 0,50MT', 'M2', 0.00, 'EUR', 
 1.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 0.50, NULL, NULL, NULL, 31.75, NULL),
('1852', '19501543', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/4" 0,75MT', 'M2', 0.00, 'EUR', 
 1.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 0.75, NULL, NULL, NULL, 31.75, NULL),
('1853', '19501544', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/4" 1,00MT', 'M2', 0.00, 'EUR', 
 1.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 1.00, NULL, NULL, NULL, 31.75, NULL),
('1854', '19501545', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/4" 1,25MT', 'M2', 0.00, 'EUR', 
 1.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 1.25, NULL, NULL, NULL, 31.75, NULL),
('1855', '19501546', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/4" 1,50MT', 'M2', 0.00, 'EUR', 
 1.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 1.50, NULL, NULL, NULL, 31.75, NULL),
('1856', '19501547', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/4" 2,00MT', 'M2', 0.00, 'EUR', 
 1.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 2.00, NULL, NULL, NULL, 31.75, NULL),
('1857', '19501548', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/2" 0,50MT', 'M2', 25.00, 'EUR', 
 1.07, 0.27, 0.27, '2017-10-10', '2020-11-04', 15, NULL, 9, 8, 19, NULL, 0.50, NULL, NULL, NULL, 38.10, NULL),
('1858', '19501549', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/2" 0,75MT', 'M2', 0.00, 'EUR', 
 1.07, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 0.75, NULL, NULL, NULL, 38.10, NULL),
('1859', '19501550', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/2" 1,00MT', 'M2', 0.00, 'EUR', 
 1.07, 0.31, 0.31, '2017-10-10', '2018-11-27', 15, NULL, 9, 8, 19, NULL, 1.00, NULL, NULL, NULL, 38.10, NULL),
('1860', '19501551', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/2" 1,25MT', 'M2', 0.00, 'EUR', 
 1.07, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 1.25, NULL, NULL, NULL, 38.10, NULL),
('1861', '19501552', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/2" 1,50MT', 'M2', 0.00, 'EUR', 
 1.07, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 1.50, NULL, NULL, NULL, 38.10, NULL),
('1862', '19501553', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 11/2" 2,00MT', 'M2', 0.00, 'EUR', 
 1.07, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 2.00, NULL, NULL, NULL, 38.10, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1863', '19501560', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 2" 0,50MT', 'M2', 25.00, 'EUR', 
 0.85, 0.24, 0.24, '2017-10-10', '2023-02-03', 15, NULL, 9, 8, 19, NULL, 0.50, NULL, NULL, NULL, 50.80, NULL),
('1864', '19501561', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 2" 0,75MT', 'M2', 0.00, 'EUR', 
 0.85, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 0.75, NULL, NULL, NULL, 50.80, NULL),
('1865', '19501562', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 2" 1,00MT', 'M2', 500.00, 'EUR', 
 0.85, 0.48, 0.48, '2023-11-27', '2024-04-30', 15, NULL, 9, 8, 19, NULL, 1.00, NULL, NULL, NULL, 50.80, NULL),
('1866', '19501563', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 2" 1,25MT', 'M2', 0.00, 'EUR', 
 0.85, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 19, NULL, 1.25, NULL, NULL, NULL, 50.80, NULL),
('1867', '19501564', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 2" 1,50MT', 'M2', 3075.00, 'EUR', 
 0.85, 0.48, 0.48, '2023-11-27', '2024-04-26', 15, NULL, 9, 8, 19, NULL, 1.50, NULL, NULL, NULL, 50.80, NULL),
('1868', '19501565', 'REDE MALHA HEXAGONAL', 'REDE MALHA HEXAGONAL 2" 2,00MT', 'M2', 900.00, 'EUR', 
 0.85, 0.49, 0.48, '2023-11-27', '2024-05-27', 15, NULL, 9, 8, 19, NULL, 2.00, NULL, NULL, NULL, 50.80, NULL);
 
 
INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1869', '19501814', 'REDE MOSQ.GALV.', 'REDE MOSQ.GALV. 18x14x0,24x1000', 'M2', 0.00, 'EUR', 
 0.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 9, 1, 11, 2,NULL,  1000.00, 18.00, 14.00, 0.24, NULL, NULL),
('1870', '19501815', 'REDE MOSQ.PLAST.', 'REDE MOSQ.PLAST. 120x30m', 'M2', 0.00, 'EUR', 
 0.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 9, 7, 11, NULL, NULL, 30.00, 120.00, NULL, NULL, NULL, NULL),
('1871', '19501816', 'REDE TECIDO SOMBRA', 'REDE TECIDO SOMBRA TS 15', 'M2', 0.00, 'EUR', 
 0.40, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 9, 1, 11, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 15.00),
('1872', '19502500', 'REDE NERVOMETAL', 'REDE NERVOMETAL 2500x600x0,3', 'M2', 0.00, 'EUR', 
 3.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 9, 1, 11, NULL, 2, 2500.00, 600.00, NULL, 0.30, NULL, NULL),
('1873', '19509012', 'REDE FULLGARDEN', 'REDE FULLGARDEN C/1,20 BRANCA', 'RL', 0.00, 'EUR', 
 39.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 9, 1, 11, NULL,NULL,  NULL, NULL, NULL, NULL, NULL, 1.20),
('1874', '19510010', 'REDE JARDITOR PLAST.', 'REDE JARDITOR PLAST. 100x75 1,00 (25MT)', 'M2', 0.00, 'EUR', 
 2.10, 0.75, 0.75, '2017-10-10', '2020-02-21', 15, NULL, 9, 7, 11, NULL, NULL, 25.00, 100.00, 75.00, NULL, NULL, NULL),
('1875', '19510010', 'REDE JARDITOR PLAST.', 'REDE JARDITOR PLAST. 100x75 1,00 (25MT)', 'MT', 0.00, 'EUR', 
 2.10, 0.75, 0.75, '2017-10-10', '2020-02-21', 15, NULL, 9, 7, 11, NULL, NULL, 25.00, 100.00, 75.00, NULL, NULL, NULL),
('1876', '19510012', 'REDE JARDITOR PLAST.', 'REDE JARDITOR PLAST. 100x75 1,20 (25MT)', 'M2', 0.00, 'EUR', 
 2.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL, 25.00, 100.00, 75.00, NULL, NULL, 1.20),
('1877', '19510012', 'REDE JARDITOR PLAST.', 'REDE JARDITOR PLAST. 100x75 1,20 (25MT)', 'MT', 0.00, 'EUR', 
 2.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL, 25.00, 100.00, 75.00, NULL, NULL, 1.20),
('1878', '19510015', 'REDE JARDITOR PLAST.', 'REDE JARDITOR PLAST. 100x75 1,50 (25MT)', 'MT', 0.00, 'EUR', 
 3.10, 1.09, 1.09, '2017-10-10', '2019-03-20', 15, NULL, 9, 7, 11, NULL, NULL, 25.00, 100.00, 75.00, NULL, NULL, 1.50),
('1879', '19510016', 'REDE JARDITOR PLAST.', 'REDE JARDITOR PLAST. 100x50 1 MT (25MT)', 'ML', 375.00, 'EUR', 
 2.50, 1.18, 1.18, '2024-06-12', '2024-06-05', 15, NULL, 9, 7, 11, NULL, NULL, 25.00, 100.00, 50.00, NULL, NULL, 1.00),
('1880', '19510017', 'REDE JARDITOR PLAST.', 'REDE JARDITOR PLAST. 100x50 1,20 (25MT)', 'ML', 200.00, 'EUR', 
 3.00, 1.43, 1.39, '2024-03-25', '2024-06-06', 15, NULL, 9, 7, 11, NULL,NULL,  25.00, 100.00, 50.00, NULL, NULL, 1.20),
('1881', '19510018', 'REDE JARDITOR PLAST.', 'REDE JARDITOR PLAST. 100x50 1,50 (25MT)', 'ML', 275.00, 'EUR', 
 3.70, 1.82, 1.79, '2023-07-13', '2024-06-05', 15, NULL, 9, 7, 11, NULL, NULL, 25.00, 100.00, 50.00, NULL, NULL, 1.50);





UPDATE t_product_catalog
SET length = length * 1000
WHERE type_id = 9 AND length < 100;



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1882', '19511080', 'REDE PLAST.', 'REDE PLAST.12x08x40 0,50', 'M2', 0.00, 'EUR', 
 7.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 500, 12000, 8000, 40, NULL, NULL),
('1883', '19511081', 'REDE PLAST.', 'REDE PLAST.12x08x40 0,75', 'M2', 0.00, 'EUR', 
 7.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,750, 12000, 8000, 40, NULL, NULL),
('1884', '19511082', 'REDE PLAST.', 'REDE PLAST.12x08x40 1,00', 'M2', 0.00, 'EUR', 
 7.10, 3.68, 5.26, '2021-11-17', '2021-11-22', 15, NULL, 9, 7, 11, NULL, NULL,1000, 12000, 8000, 40, NULL, NULL),
('1885', '19511083', 'REDE PLAST.', 'REDE PLAST.12x08x40 1,25', 'M2', 0.00, 'EUR', 
 7.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1250, 12000, 8000, 40, NULL, NULL),
('1886', '19511084', 'REDE PLAST.', 'REDE PLAST.12x08x40 1,50', 'M2', 0.00, 'EUR', 
 7.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1500, 12000, 8000, 40, NULL, NULL),
('1887', '19511085', 'REDE PLAST.', 'REDE PLAST.12x08x40 1,75', 'M2', 0.00, 'EUR', 
 7.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1750, 12000, 8000, 40, NULL, NULL),
('1888', '19511086', 'REDE PLAST.', 'REDE PLAST.12x08x40 2,00', 'M2', 0.00, 'EUR', 
 7.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2000, 12000, 8000, 40, NULL, NULL),
('1889', '19511087', 'REDE PLAST.', 'REDE PLAST.12x08x40 2,25', 'M2', 0.00, 'EUR', 
 7.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2250, 12000, 8000, 40, NULL, NULL),
('1890', '19511088', 'REDE PLAST.', 'REDE PLAST.12x08x40 2,50', 'M2', 0.00, 'EUR', 
 7.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2500, 12000, 8000, 40, NULL, NULL),
('1891', '19511089', 'REDE PLAST.', 'REDE PLAST.12x08x40 2,75', 'M2', 0.00, 'EUR', 
 7.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2750, 12000, 8000, 40, NULL, NULL),
('1892', '19511090', 'REDE PLAST.', 'REDE PLAST.12x08x40 3,00', 'M2', 0.00, 'EUR', 
 7.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,3000, 12000, 8000, 40, NULL, NULL),
('1893', '19512080', 'REDE PLAST.', 'REDE PLAST.12x08x50 0,50', 'M2', 0.00, 'EUR', 
 5.50, 1.76, 1.76, '2017-10-10', '2021-09-01', 15, NULL, 9, 7, 11, NULL, NULL,500, 12000, 8000, 50, NULL, NULL),
('1894', '19512081', 'REDE PLAST.', 'REDE PLAST.12x08x50 0,75', 'M2', 0.00, 'EUR', 
 5.50, 2.13, 2.13, '2017-10-10', '2022-05-02', 15, NULL, 9, 7, 11, NULL, NULL,750, 12000, 8000, 50, NULL, NULL),
('1895', '19512082', 'REDE PLAST.', 'REDE PLAST.12x08x50 1,00', 'M2', 0.00, 'EUR', 
 5.50, 3.65, 5.22, '2022-08-09', '2022-08-11', 15, NULL, 9, 7, 11, NULL,NULL, 1000, 12000, 8000, 50, NULL, NULL),
('1896', '19512083', 'REDE PLAST.', 'REDE PLAST.12x08x50 1,25', 'M2', 0.00, 'EUR', 
 5.50, 3.03, 3.03, '2023-06-02', '2023-06-05', 15, NULL, 9, 7, 11, NULL, NULL,1250, 12000, 8000, 50, NULL, NULL),
('1897', '19512084', 'REDE PLAST.', 'REDE PLAST.12x08x50 1,50', 'M2', 0.00, 'EUR', 
 5.50, 3.51, 3.26, '2023-03-01', '2023-03-01', 15, NULL, 9, 7, 11, NULL, NULL,1500, 12000, 8000, 50, NULL, NULL),
('1898', '19512085', 'REDE PLAST.', 'REDE PLAST.12x08x50 1,75', 'M2', 0.00, 'EUR', 
 5.50, 3.26, 3.26, '2023-03-23', '2023-03-24', 15, NULL, 9, 7, 11, NULL, NULL,1750, 12000, 8000, 50, NULL, NULL),
('1899', '19512086', 'REDE PLAST.', 'REDE PLAST.12x08x50 2,00', 'M2', 0.00, 'EUR', 
 5.50, 3.51, 3.51, '2022-11-17', '2022-11-21', 15, NULL, 9, 7, 11, NULL, NULL,2000, 12000, 8000, 50, NULL, NULL),
('1900', '19512087', 'REDE PLAST.', 'REDE PLAST.12x08x50x5,00', 'M2', 0.00, 'EUR', 
 5.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,5000, 12000, 8000, 50, NULL, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1901', '19512089', 'REDE PLAST.', 'REDE PLAST.12x08x50 2,50', 'M2', 0.00, 'EUR', 
 5.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2500, 12000, 8000, 50, NULL, NULL),
('1904', '19512090', 'REDE PLAST.', 'REDE PLAST.12x08x60 4,80', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,4800, 12000, 8000, 60, NULL, NULL),
('1905', '19512091', 'REDE PLAST.', 'REDE PLAST.12x08x50 3,00', 'M2', 0.00, 'EUR', 
 5.50, 4.05, 4.05, '2023-09-19', '2023-09-26', 15, NULL, 9, 7, 11, NULL,NULL, 3000, 12000, 8000, 50, NULL, NULL),
('1906', '19512091', 'REDE PLAST.', 'REDE PLAST.12x08x50 3,00', 'MT', 0.00, 'EUR', 
 5.50, 4.05, 4.05, '2023-09-19', '2023-09-26', 15, NULL, 9, 7, 11, NULL,NULL, 3000, 12000, 8000, 50, NULL, NULL),
('1907', '19512180', 'REDE PLAST.', 'REDE PLAST.12x08x60 0,50', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,500, 12000, 8000, 60, NULL, NULL),
('1908', '19512181', 'REDE PLAST.', 'REDE PLAST.12x08x60 0,75', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 750, 12000, 8000, 60, NULL, NULL),
('1909', '19512182', 'REDE PLAST.', 'REDE PLAST.12x08x60 1,00', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 1000, 12000, 8000, 60, NULL, NULL),
('1910', '19512183', 'REDE PLAST.', 'REDE PLAST.12x08x60 1,25', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1250, 12000, 8000, 60, NULL, NULL),
('1911', '19512184', 'REDE PLAST.', 'REDE PLAST.12x08x60 1,50', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1500, 12000, 8000, 60, NULL, NULL),
('1912', '19512185', 'REDE PLAST.', 'REDE PLAST.12x08x60 1,75', 'M2', 0.00, 'EUR', 
 4.60, 1.54, 1.54, '2019-04-10', '2021-06-01', 15, NULL, 9, 7, 11, NULL,NULL, 1750, 12000, 8000, 60, NULL, NULL),
('1913', '19512186', 'REDE PLAST.', 'REDE PLAST.12x08x60 2,00', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2000, 12000, 8000, 60, NULL, NULL),
('1914', '19512187', 'REDE PLAST.', 'REDE PLAST.12x08x60 2,25', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2250, 12000, 8000, 60, NULL, NULL),
('1915', '19512188', 'REDE PLAST.', 'REDE PLAST.12x08x60 2,50', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2500, 12000, 8000, 60, NULL, NULL),
('1916', '19512189', 'REDE PLAST.', 'REDE PLAST.12x08x60 2,80', 'M2', 0.00, 'EUR', 
 4.60, 1.54, 1.54, '2019-01-15', '2019-01-18', 15, NULL, 9, 7, 11, NULL,NULL, 2800, 12000, 8000, 60, NULL, NULL),
('1917', '19512190', 'REDE PLAST.', 'REDE PLAST.12x08x60 4,00', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 4000, 12000, 8000, 60, NULL, NULL),
('1918', '19512191', 'REDE PLAST.', 'REDE PLAST.12x08x60 1,60', 'KG', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1600, 12000, 8000, 60, NULL, NULL),
('1919', '19512191', 'REDE PLAST.', 'REDE PLAST.12x08x60 1,60', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 1600, 12000, 8000, 60, NULL, NULL),
('1920', '19513010', 'REDE PLAST.', 'REDE PLAST.13x10x40 0,50', 'M2', 0.00, 'EUR', 
 5.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,500, 13000, 10000, 40, NULL, NULL),
('1921', '19513011', 'REDE PLAST.', 'REDE PLAST.13x10x40 0,75', 'M2', 0.00, 'EUR', 
 5.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 750, 13000, 10000, 40, NULL, NULL),
('1922', '19513012', 'REDE PLAST.', 'REDE PLAST.13x10x40 1,00', 'M2', 0.00, 'EUR', 
 5.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1000, 13000, 10000, 40, NULL, NULL);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1923', '19513013', 'REDE PLAST.', 'REDE PLAST.13x10x40 1,25', 'M2', 0.00, 'EUR', 
 5.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 1250, 13000, 10000, 40, NULL, NULL),
('1924', '19513014', 'REDE PLAST.', 'REDE PLAST.13x10x40 1,50', 'M2', 0.00, 'EUR', 
 5.95, 2.96, 4.23, '2021-08-12', '2021-08-16', 15, NULL, 9, 7, 11, NULL,NULL, 1500, 13000, 10000, 40, NULL, NULL),
('1925', '19513015', 'REDE PLAST.', 'REDE PLAST.13x10x40 1,75', 'M2', 0.00, 'EUR', 
 5.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 1750, 13000, 10000, 40, NULL, NULL),
('1926', '19513016', 'REDE PLAST.', 'REDE PLAST.13x10x40 2,00', 'M2', 0.00, 'EUR', 
 5.95, 3.07, 3.07, '2023-06-29', '2023-07-11', 15, NULL, 9, 7, 11, NULL, NULL,2000, 13000, 10000, 40, NULL, NULL),
('1927', '19513017', 'REDE PLAST.', 'REDE PLAST.13x10x40 2,25', 'M2', 0.00, 'EUR', 
 5.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2250, 13000, 10000, 40, NULL, NULL),
('1928', '19513018', 'REDE PLAST.', 'REDE PLAST.13x10x40 2,50', 'M2', 0.00, 'EUR', 
 5.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2500, 13000, 10000, 40, NULL, NULL),
('1929', '19513019', 'REDE PLAST.', 'REDE PLAST.13x10x40 3,00', 'M2', 0.00, 'EUR', 
 5.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,3000, 13000, 10000, 40, NULL, NULL),
('1930', '19513110', 'REDE PLAST.', 'REDE PLAST.13x10x50 0,50', 'M2', 12.50, 'EUR', 
 4.60, 2.49, 2.49, '2023-03-23', '2023-09-25', 15, NULL, 9, 7, 11, NULL, NULL,500, 13000, 10000, 50, NULL, NULL),
('1931', '19513111', 'REDE PLAST.', 'REDE PLAST.13x10x50 0,75', 'M2', 150.00, 'EUR', 
 4.60, 2.26, 2.26, '2024-01-26', '2024-05-09', 15, NULL, 9, 7, 11, NULL, NULL,750, 13000, 10000, 50, NULL, NULL),
('1932', '19513112', 'REDE PLAST.', 'REDE PLAST.13x10x50 1,00', 'M2', 0.00, 'EUR', 
 4.60, 2.34, 2.34, '2024-05-20', '2024-06-04', 15, NULL, 9, 7, 11, NULL, NULL,1000, 13000, 10000, 50, NULL, NULL),
('1933', '19513113', 'REDE PLAST.', 'REDE PLAST.13x10x50 1,25', 'M2', 218.75, 'EUR', 
 4.60, 2.34, 2.34, '2024-05-20', '2024-06-03', 15, NULL, 9, 7, 11, NULL, NULL,1250, 13000, 10000, 50, NULL, NULL),
('1934', '19513114', 'REDE PLAST.', 'REDE PLAST.13x10x50 1,50', 'M2', 112.50, 'EUR', 
 4.60, 2.26, 2.26, '2024-03-27', '2024-06-12', 15, NULL, 9, 7, 11, NULL, NULL,1500, 13000, 10000, 50, NULL, NULL),
('1935', '19513115', 'REDE PLAST.', 'REDE PLAST.13x10x50 1,75', 'M2', 262.50, 'EUR', 
 4.60, 2.80, 3.44, '2023-07-19', '2024-04-05', 15, NULL, 9, 7, 11, NULL,NULL, 1750, 13000, 10000, 50, NULL, NULL),
('1936', '19513116', 'REDE PLAST.', 'REDE PLAST.13x10x50 2,00', 'M2', 400.00, 'EUR', 
 4.60, 2.26, 2.26, '2024-01-16', '2024-06-13', 15, NULL, 9, 7, 11, NULL, NULL,2000, 13000, 10000, 50, NULL, NULL),
('1937', '19513117', 'REDE PLAST.', 'REDE PLAST.13x10x50 2,25', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2250, 13000, 10000, 50, NULL, NULL),
('1938', '19513118', 'REDE PLAST.', 'REDE PLAST.13x10x50 2,50', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2500, 13000, 10000, 50, NULL, NULL),
('1939', '19513119', 'REDE PLAST.', 'REDE PLAST.13x10x50 2,75', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2750, 13000, 10000, 50, NULL, NULL),
('1940', '19513120', 'REDE PLAST.', 'REDE PLAST.13x10x50 3,00', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 3000, 13000, 10000, 50, NULL, NULL),
('1941', '19513121', 'REDE PLAST.', 'REDE PLAST.13x10x50 3,50', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 3500, 13000, 10000, 50, NULL, NULL),
('1942', '19513122', 'REDE PLAST.', 'REDE PLAST.13x10x50 4,00', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 4000, 13000, 10000, 50, NULL, NULL),
('1943', '19513123', 'REDE PLAST.', 'REDE PLAST.13x10x50 3,75', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 3750, 13000, 10000, 50, NULL, NULL);



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1944', '19513210', 'REDE PLAST.', 'REDE PLAST.13x10x60 0,50', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,500, 13000, 10000, 60, NULL, NULL),
('1945', '19513211', 'REDE PLAST.', 'REDE PLAST.13x10x60 0,75', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,750, 13000, 10000, 60, NULL, NULL),
('1946', '19513212', 'REDE PLAST.', 'REDE PLAST.13x10x60 1,00', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1000, 13000, 10000, 60, NULL, NULL),
('1947', '19513213', 'REDE PLAST.', 'REDE PLAST.13x10x60 1,25', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 1250, 13000, 10000, 60, NULL, NULL),
('1948', '19513214', 'REDE PLAST.', 'REDE PLAST.13x10x60 1,50', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 1500, 13000, 10000, 60, NULL, NULL),
('1949', '19513215', 'REDE PLAST.', 'REDE PLAST.13x10x60 1,75', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1750, 13000, 10000, 60, NULL, NULL),
('1950', '19513216', 'REDE PLAST.', 'REDE PLAST.13x10x60 2,00', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2000, 13000, 10000, 60, NULL, NULL),
('1951', '19513217', 'REDE PLAST.', 'REDE PLAST.13x10x60 2,25', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2250, 13000, 10000, 60, NULL, NULL),
('1952', '19513218', 'REDE PLAST.', 'REDE PLAST.13x10x60 2,50', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2500, 13000, 10000, 60, NULL, NULL),
('1953', '19513219', 'REDE PLAST.', 'REDE PLAST.13x10x60 2,75', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2750, 13000, 10000, 60, NULL, NULL),
('1954', '19513220', 'REDE PLAST.', 'REDE PLAST.13x10x60 3,00', 'M2', 0.00, 'EUR', 
 3.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,3000, 13000, 10000, 60, NULL, NULL),
('1955', '19514010', 'REDE PLAST.', 'REDE PLAST.14x11x40 0,50', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,500, 14000, 11000, 40, NULL, NULL),
('1956', '19514011', 'REDE PLAST.', 'REDE PLAST.14x11x40 0,75', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 750, 14000, 11000, 40, NULL, NULL),
('1957', '19514012', 'REDE PLAST.', 'REDE PLAST.14x11x40 1,00', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1000, 14000, 11000, 40, NULL, NULL),
('1958', '19514013', 'REDE PLAST.', 'REDE PLAST.14x11x40 1,25', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1250, 14000, 11000, 40, NULL, NULL),
('1959', '19514014', 'REDE PLAST.', 'REDE PLAST.14x11x40 1,50', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1500, 14000, 11000, 40, NULL, NULL),
('1960', '19514015', 'REDE PLAST.', 'REDE PLAST.14x11x40 1,75', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,1750, 14000, 11000, 40, NULL, NULL),
('1961', '19514016', 'REDE PLAST.', 'REDE PLAST.14x11x40 2,00', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2000, 14000, 11000, 40, NULL, NULL),
('1962', '19514017', 'REDE PLAST.', 'REDE PLAST.14x11x40 2,25', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2250, 14000, 11000, 40, NULL, NULL),
('1963', '19514018', 'REDE PLAST.', 'REDE PLAST.14x11x40 2,50', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2500, 14000, 11000, 40, NULL, NULL),
('1964', '19514019', 'REDE PLAST.', 'REDE PLAST.14x11x40 2,75', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2750, 14000, 11000, 40, NULL, NULL);





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1965', '19514020', 'REDE PLAST.', 'REDE PLAST.14x11x40 3,00', 'M2', 0.00, 'EUR', 
 4.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,3000, 14000, 11000, 40, NULL, NULL),
('1966', '19514110', 'REDE PLAST.', 'REDE PLAST.14x11x50 0,50', 'M2', 0.00, 'EUR', 
 3.60, 1.67, 1.67, '2021-11-17', '2023-09-26', 15, NULL, 9, 7, 11, NULL,NULL, 500, 14000, 11000, 50, NULL, NULL),
('1967', '19514111', 'REDE PLAST.', 'REDE PLAST.14x11x50 0,75', 'M2', 75.00, 'EUR', 
 3.60, 1.70, 1.70, '2024-01-24', '2024-03-18', 15, NULL, 9, 7, 11, NULL, NULL,750, 14000, 11000, 50, NULL, NULL),
('1968', '19514112', 'REDE PLAST.', 'REDE PLAST.14x11x50 1,00', 'M2', 175.00, 'EUR', 
 3.60, 1.71, 1.70, '2024-03-27', '2024-06-03', 15, NULL, 9, 7, 11, NULL,NULL, 1000, 14000, 11000, 50, NULL, NULL),
('1969', '19514113', 'REDE PLAST.', 'REDE PLAST.14x11x50 1,25', 'M2', 125.00, 'EUR', 
 3.60, 1.71, 1.70, '2024-04-02', '2024-05-31', 15, NULL, 9, 7, 11, NULL, NULL,1250, 14000, 11000, 50, NULL, NULL),
('1970', '19514114', 'REDE PLAST.', 'REDE PLAST.14x11x50 1,50', 'M2', 300.00, 'EUR', 
 3.60, 1.74, 1.74, '2024-05-20', '2024-06-12', 15, NULL, 9, 7, 11, NULL, NULL,1500, 14000, 11000, 50, NULL, NULL),
('1971', '19514115', 'REDE PLAST.', 'REDE PLAST.14x11x50 1,75', 'M2', 175.00, 'EUR', 
 3.60, 1.74, 1.74, '2024-05-20', '2024-05-23', 15, NULL, 9, 7, 11, NULL, NULL,1750, 14000, 11000, 50, NULL, NULL),
('1972', '19514116', 'REDE PLAST.', 'REDE PLAST.14x11x50 2,00', 'M2', 250.00, 'EUR', 
 3.60, 1.76, 1.76, '2024-01-16', '2024-06-03', 15, NULL, 9, 7, 11, NULL,NULL, 2000, 14000, 11000, 50, NULL, NULL),
('1973', '19514117', 'REDE PLAST.', 'REDE PLAST.14x11x50 2,20', 'M2', 0.00, 'EUR', 
 3.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,2200, 14000, 11000, 50, NULL, NULL),
('1974', '19514120', 'REDE PLAST.', 'REDE PLAST.14x11x50 2,50', 'M2', 0.00, 'EUR', 
 3.60, 1.26, 1.26, '2018-11-07', '2018-11-13', 15, NULL, 9, 7, 11, NULL, NULL,2500, 14000, 11000, 50, NULL, NULL),
('1975', '19514121', 'REDE PLAST.', 'REDE PLAST.14x11x50 3,00', 'M2', 0.00, 'EUR', 
 3.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 3000, 14000, 11000, 50, NULL, NULL),
('1976', '19514122', 'REDE PLAST.', 'REDE PLAST.14x11x50 2,75', 'M2', 0.00, 'EUR', 
 3.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2750, 14000, 11000, 50, NULL, NULL),
('1977', '19514123', 'REDE PLAST.', 'REDE PLAST.14x11x50 2,25', 'M2', 0.00, 'EUR', 
 3.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2250, 14000, 11000, 50, NULL, NULL),
('1978', '19514124', 'REDE PLAST.', 'REDE PLAST.14x11x50 2,50', 'M2', 0.00, 'EUR', 
 3.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2500, 14000, 11000, 50, NULL, NULL),
('1979', '19514125', 'REDE PLAST.', 'REDE PLAST.14x11x50 2,75', 'M2', 0.00, 'EUR', 
 3.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 2750, 14000, 11000, 50, NULL, NULL),
('1980', '19514126', 'REDE PLAST.', 'REDE PLAST.14x11x50 3,00', 'M2', 0.00, 'EUR', 
 3.60, 1.26, 1.26, '2018-11-07', '2018-11-13', 15, NULL, 9, 7, 11, NULL, NULL,3000, 14000, 11000, 50, NULL, NULL),
('1981', '19514127', 'REDE PLAST.', 'REDE PLAST.14x11x50 3,50', 'M2', 0.00, 'EUR', 
 3.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL,NULL, 3500, 14000, 11000, 50, NULL, NULL),
('1982', '19514128', 'REDE PLAST.', 'REDE PLAST.14x11x50 4,00', 'M2', 0.00, 'EUR', 
 3.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,4000, 14000, 11000, 50, NULL, NULL),
('1983', '19514129', 'REDE PLAST.', 'REDE PLAST.14x11x50 4,50', 'M2', 0.00, 'EUR', 
 3.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 7, 11, NULL, NULL,4500, 14000, 11000, 50, NULL, NULL),
('1984', '19514210', 'REDE PLAST.', 'REDE PLAST.14x11x60 0,50', 'M2', 0.00, 'EUR', 
 3.00, 1.09, 1.09, '2017-10-10', '2020-05-25', 15, NULL, 9, 7, 11, NULL, NULL,500, 14000, 11000, 60, NULL, NULL);






INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1996', '19610000', 'PAINEL REDE/TREMIDA', 'PAINEL REDE/TREMIDA QUAD. 50x50 0,76', 'M2', 0.00, 'EUR', 
 4.08, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 2, NULL, NULL,760, 50, 50, NULL, NULL, NULL),
('2023', '19650419', 'ROLOS REDE ELECT/GALV.', 'ROLOS REDE ELECT/GALV. 19x19x1x1000mm', 'M2', 0.00, 'EUR', 
 11.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 11, 2, NULL,1000, 19, 19, 1, NULL, NULL),
('2024', '19650420', 'ROLOS REDE ELECT/GALV.', 'ROLOS REDE ELECT/GALV. 76x13x2.4x1000mm', 'M2', 0.00, 'EUR', 
 11.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 11, 2, NULL, 1000, 76, 13, 2.4, NULL, NULL),
('2025', '19650421', 'ROLOS REDE ELECT/GALV.', 'ROLOS REDE ELECT/GALV. 19x19x1,4x1000mm', 'M2', 0.00, 'EUR', 
 5.60, 1.66, 1.66, '2017-10-10', '2017-11-14', 15, NULL, 9, 1, 11, 2, NULL,1000, 19, 19, 1.4, NULL, NULL),
('2026', '19650422', 'ROLOS REDE ELECT/GALV.', 'ROLOS REDE ELECT/GALV. 25x25x2x1000mm', 'M2', 0.00, 'EUR', 
 7.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 11, 2, NULL,1000, 25, 25, 2, NULL, NULL),
('2046', '19650832', 'PAINEL REDE MALHAMOR', 'PAINEL REDE MALHAMOR VERDE 50x50x5mm C/2100x2000', 'UN', 0.00, 'EUR', 
 40.90, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 8, 11, 8, NULL,2100, 2000, NULL, 5, NULL, NULL),
('2053', '19651495', 'REDE TREM.GALV.', 'REDE TREM.GALV.40x40x3', 'M2', 0.00, 'EUR', 
 5.75, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 9, 1, 11, 2, NULL, NULL,40, 40, 3, NULL, NULL),
('2054', '19651640', 'ROLOS DE REDE ELECRO GALV.', 'ROLOS DE REDE ELECRO GALV.13x13x1,00 mm', 'RL', 0.00, 'EUR', 
 51.74, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 9, 1, 11, 2, NULL, NULL,13, 13, 1, NULL, NULL);


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2232', '19710130', 'MALHA ELECT-SOLDADA FAR', 'MALHA ELECT-SOLDADA FAR 30 (ROLO)', 'M2', 600.00, 'EUR', 
 0.93, 0.54, 0.56, '2024-05-24', '2024-05-31', 16, NULL, 9, 8, 20, NULL, NULL, 30.00, NULL, NULL, NULL, NULL, '30'),
('2233', '19710131', 'MALHA ELECT-SOLDADA FAR', 'MALHA ELECT-SOLDADA FAR 30 (PAINEL)', 'M2', 0.00, 'EUR', 
 0.93, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, 30.00, NULL, NULL, NULL, NULL, '30'),
('2234', '19710131', 'MALHA ELECT-SOLDADA FAR', 'MALHA ELECT-SOLDADA FAR 30 (PAINEL)', 'UN', 0.00, 'EUR', 
 0.93, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, 30.00, NULL, NULL, NULL, NULL, '30'),
('2235', '19710134', 'MALHA ELECT-SOLDADA FAR', 'MALHA ELECT-SOLDADA FAR 34 (ROLO)', 'M2', 1200.00, 'EUR', 
 1.19, 0.65, 0.65, '2024-03-07', '2024-05-22', 16, NULL, 9, 8, 20, NULL, NULL, 34.00, NULL, NULL, NULL, NULL, '34'),
('2236', '19710135', 'MALHA ELECT-SOLDADA FAR', 'MALHA ELECT-SOLDADA FAR 34 (PAINEL)', 'M2', 0.00, 'EUR', 
 1.19, 0.87, 1.01, '2022-01-06', '2022-03-10', 16, NULL, 9, 8, 20, NULL, NULL, 34.00, NULL, NULL, NULL, NULL, '34'),
('2237', '19710135', 'MALHA ELECT-SOLDADA FAR', 'MALHA ELECT-SOLDADA FAR 34 (PAINEL)', 'UN', 0.00, 'EUR', 
 1.19, 0.87, 1.01, '2022-01-06', '2022-03-10', 16, NULL, 9, 8, 20, NULL, NULL, 34.00, NULL, NULL, NULL, NULL, '34'),
('2238', '19710138', 'MALHA ELECT-SOLDADA FAR', 'MALHA ELECT-SOLDADA FAR 38 (ROLO)', 'M2', 1320.00, 'EUR', 
 1.48, 0.89, 0.89, '2024-05-24', '2024-05-21', 16, NULL, 9, 8, 20, NULL, NULL, 38.00, NULL, NULL, NULL, NULL, '38'),
('2239', '19710139', 'MALHA ELECT-SOLDADA FAR', 'MALHA ELECT-SOLDADA FAR 38 (PAINEL)', 'M2', 0.00, 'EUR', 
 1.48, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, 38.00, NULL, NULL, NULL, NULL, '38'),
('2240', '19710139', 'MALHA ELECT-SOLDADA FAR', 'MALHA ELECT-SOLDADA FAR 38 (PAINEL)', 'UN', 0.00, 'EUR', 
 1.48, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, 38.00, NULL, NULL, NULL, NULL, '38'),
('2241', '19710142', 'MALHA ELECT-SOLDADA FNR', 'MALHA ELECT-SOLDADA FNR 40 (ROLO)', 'M2', 1440.00, 'EUR', 
 1.64, 0.98, 0.98, '2024-05-24', '2024-04-26', 16, NULL, 9, 8, 20, NULL, NULL, 40.00, NULL, NULL, NULL, NULL, '40'),
('2242', '19710143', 'MALHA ELECT-SOLDADA FNR', 'MALHA ELECT-SOLDADA FNR 40 (PAINEL)', 'M2', 1742.40, 'EUR', 
 1.64, 0.98, 0.98, '2024-05-24', '2024-06-12', 16, NULL, 9, 8, 20, NULL, NULL, 40.00, NULL, NULL, NULL, NULL, '40'),
('2243', '19710143', 'MALHA ELECT-SOLDADA FNR', 'MALHA ELECT-SOLDADA FNR 40 (PAINEL)', 'UN', 1742.40, 'EUR', 
 1.64, 0.98, 0.98, '2024-05-24', '2024-06-12', 16, NULL, 9, 8, 20, NULL, NULL, 40.00, NULL, NULL, NULL, NULL, '40');




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2247', '19710150', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD. FNR 50 (ROLO)', 'M2', 0.00, 'EUR', 
 2.55, 1.23, 1.23, '2019-12-31', '2020-03-10', 16, NULL, 9, 8, 20, NULL, NULL, 50.00, NULL, NULL, NULL, NULL, '50'),
('2248', '19710151', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 50 (PAINEL)', 'M2', 2131.20, 'EUR', 
 2.55, 1.47, 1.48, '2024-05-24', '2024-06-06', 16, NULL, 9, 8, 20, NULL, NULL, 50.00, NULL, NULL, NULL, NULL, '50'),
('2249', '19710151', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 50 (PAINEL)', 'UN', 2131.20, 'EUR', 
 2.55, 1.47, 1.48, '2024-05-24', '2024-06-06', 16, NULL, 9, 8, 20, NULL, NULL, 50.00, NULL, NULL, NULL, NULL, '50'),
('2250', '19710155', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 55 (PAINEL)', 'M2', 0.00, 'EUR', 
 2.94, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, 55.00, NULL, NULL, NULL, NULL, '55'),
('2251', '19710160', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 60 (PAINEL)', 'M2', 763.20, 'EUR', 
 3.38, 2.05, 2.06, '2024-03-25', '2024-06-13', 16, NULL, 9, 8, 20, NULL, NULL, 60.00, NULL, NULL, NULL, NULL, '60'),
('2252', '19710165', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 65 (PAINEL)', 'M2', 0.00, 'EUR', 
 3.85, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, 65.00, NULL, NULL, NULL, NULL, '65'),
('2253', '19710170', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 70 (PAINEL)', 'M2', 1137.60, 'EUR', 
 4.51, 2.83, 2.62, '2024-01-11', '2023-11-17', 16, NULL, 9, 8, 20, NULL, NULL, 70.00, NULL, NULL, NULL, NULL, '70'),
('2254', '19710176', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 75 (PAINEL)', 'M2', 0.00, 'EUR', 
 5.21, 2.21, 3.29, '2020-07-16', '2021-12-20', 16, NULL, 9, 8, 20, NULL, NULL, 75.00, NULL, NULL, NULL, NULL, '75'),
('2255', '19710180', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 80 (PAINEL)', 'M2', 1785.60, 'EUR', 
 5.95, 3.81, 3.82, '2024-05-24', '2024-06-04', 16, NULL, 9, 8, 20, NULL, NULL, 80.00, NULL, NULL, NULL, NULL, '80'),
('2256', '19710182', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 85 (PAINEL)', 'M2', 0.00, 'EUR', 
 6.59, 3.82, 3.82, '2017-10-10', '2017-10-25', 16, NULL, 9, 8, 20, NULL, NULL, 85.00, NULL, NULL, NULL, NULL, '85'),
('2257', '19710190', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 90 (PAINEL)', 'M2', 0.00, 'EUR', 
 7.42, 3.29, 3.29, '2019-07-17', '2019-07-18', 16, NULL, 9, 8, 20, NULL, NULL, 90.00, NULL, NULL, NULL, NULL, '90'),
('2258', '19710210', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FNR 100(PAINEL)', 'M2', 0.00, 'EUR', 
 9.06, 5.76, 5.76, '2023-07-11', '2023-07-12', 16, NULL, 9, 8, 20, NULL, NULL, 100.00, NULL, NULL, NULL, NULL, '100'),
('2259', '19710330', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FCQ 30 (ROLO)', 'M2', 9871.00, 'EUR', 
 0.93, 0.53, 0.53, '2024-06-12', '2024-06-14', 16, NULL, 9, 8, 20, NULL, NULL, 30.00, NULL, NULL, NULL, NULL, '30'),
('2260', '19710331', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FCQ 30 (PAINEL)', 'M2', 0.00, 'EUR', 
 0.93, 0.43, 0.43, '2019-10-23', '2019-10-23', 16, NULL, 9, 8, 20, NULL, NULL, 30.00, NULL, NULL, NULL, NULL, '30'),
('2261', '19710331', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FCQ 30 (PAINEL)', 'UN', 0.00, 'EUR', 
 0.93, 0.43, 0.43, '2019-10-23', '2019-10-23', 16, NULL, 9, 8, 20, NULL, NULL, 30.00, NULL, NULL, NULL, NULL, '30'),
('2262', '19710332', 'MALHA ELECT-SOLD.', 'MALHA ELECT-SOLD.FCQ 30 ( 1/2 ROLOS )', 'M2', 3360.00, 'EUR', 
 0.93, 0.53, 0.53, '2024-06-12', '2024-06-14', 16, NULL, 9, 8, 20, NULL, NULL, 30.00, NULL, NULL, NULL, NULL, '30');




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2263', '19710332', 'MALHA ELECT-SOLD.FCQ 30', 'MALHA ELECT-SOLD.FCQ 30 ( 1/2 ROLOS )', 'UN', 3360.00, 'EUR', 
 0.93, 0.53, 0.53, '2024-06-12', '2024-06-14', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '30'),
('2264', '19710337', 'MALHA ELECT-SOLD.FCQ 38', 'MALHA ELECT-SOLD.FCQ 38 (PAINEL EMPAL. 6x2.2m)', 'M2', 396.00, 'EUR', 
 1.48, 0.83, 0.83, '2024-05-24', '2024-06-12', 16, NULL, 9, 8, 20, NULL,NULL, 6000.00, 2200.00,  NULL, NULL, NULL, '38'),
('2265', '19710338', 'MALHA ELECT-SOLD.FCQ 38', 'MALHA ELECT-SOLD.FCQ 38 (ROLO)', 'M2', 2160.00, 'EUR', 
 1.48, 0.85, 0.86, '2024-05-07', '2024-05-31', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '38'),
('2266', '19710339', 'MALHA ELECT-SOLD.FCQ 38', 'MALHA ELECT-SOLD.FCQ 38 (PAINEL)', 'M2', -43.20, 'EUR', 
 1.48, 0.86, 0.86, '2024-05-14', '2024-06-14', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '38'),
('2267', '19710339', 'MALHA ELECT-SOLD.FCQ 38', 'MALHA ELECT-SOLD.FCQ 38 (PAINEL)', 'UN', -43.20, 'EUR', 
 1.48, 0.86, 0.86, '2024-05-14', '2024-06-14', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '38'),
('2268', '19710340', 'MALHA ELECT-SOLD.FND 40', 'MALHA ELECT-SOLD.FND 40 (ROLO)', 'M2', 0.00, 'EUR', 
 1.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '40'),
('2269', '19710341', 'MALHA ELECT-SOLD.FND 40', 'MALHA ELECT-SOLD.FND 40 (PAINEL)', 'M2', 0.00, 'EUR', 
 1.65, 0.79, 0.79, '2018-10-11', '2018-10-25', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '40'),
('2270', '19710345', 'MALHA ELECT-SOLD.FND 45', 'MALHA ELECT-SOLD.FND 45 (ROLO)', 'M2', 0.00, 'EUR', 
 2.07, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '45'),
('2271', '19710346', 'MALHA ELECT-SOLD.FND 45', 'MALHA ELECT-SOLD.FND 45 (PAINEL)', 'M2', 0.00, 'EUR', 
 2.07, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '45'),
('2272', '19710346', 'MALHA ELECT-SOLD.FND 45', 'MALHA ELECT-SOLD.FND 45 (PAINEL)', 'UN', 0.00, 'EUR', 
 2.07, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '45'),
('2273', '19710350', 'MALHA ELECT-SOLD.FND 50', 'MALHA ELECT-SOLD.FND 50 (ROLO)', 'M2', 0.00, 'EUR', 
 2.55, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50'),
('2274', '19710351', 'MALHA ELECT-SOLD.FND 50', 'MALHA ELECT-SOLD.FND 50 (PAINEL)', 'M2', 1051.20, 'EUR', 
 2.55, 1.46, 1.46, '2024-05-03', '2024-06-05', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50'),
('2275', '19710351', 'MALHA ELECT-SOLD.FND 50', 'MALHA ELECT-SOLD.FND 50 (PAINEL)', 'UN', 1051.20, 'EUR', 
 2.55, 1.46, 1.46, '2024-05-03', '2024-06-05', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50'),
('2276', '19710352', 'MALHA ELECT-SOLD.FND 50', 'MALHA ELECT-SOLD.FND 50 (PAINEL EMPAL. 6x2.2m)', 'M2', 1320.00, 'EUR', 
 2.55, 1.30, 1.30, '2024-03-15', '2024-06-04', 16, NULL, 9, 8, 20, NULL,NULL, 6000.00, 2200.00, NULL, NULL, NULL, '50');







INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2277', '19710360', 'MALHA ELECT-SOLD.FND 60', 'MALHA ELECT-SOLD.FND 60 (PAINEL)', 'M2', 0.00, 'EUR', 
 3.66, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '60'),
('2278', '19710370', 'MALHA ELECT-SOLD.FND 70', 'MALHA ELECT-SOLD.FND 70 (PAINEL)', 'M2', 0.00, 'EUR', 
 4.99, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '70'),
('2279', '19710380', 'MALHA ELECT-SOLD.FND 80', 'MALHA ELECT-SOLD.FND 80 (PAINEL)', 'M2', 0.00, 'EUR', 
 6.51, 3.17, 3.17, '2018-11-13', '2022-05-31', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '80'),
('2280', '19710390', 'MALHA ELECT-SOLD.FND 90', 'MALHA ELECT-SOLD.FND 90 (PAINEL)', 'M2', 0.00, 'EUR', 
 8.25, 3.17, 3.17, '2019-09-20', '2019-09-24', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '90'),
('2281', '19710398', 'MALHA ELECT-SOLD.FND 100', 'MALHA ELECT-SOLD.FND 100 (PAINEL EMPAL. 6x2.2m)', 'M2', 0.00, 'EUR', 
 10.16, 4.89, 4.89, '2024-01-31', '2024-05-24', 16, NULL, 9, 8, 20, NULL,  NULL,6000.00, 2200.00, NULL, NULL, NULL, '100'),
('2282', '19710399', 'MALHA ELECT-SOLD.FND 100', 'MALHA ELECT-SOLD.FND 100 (PAINEL)', 'M2', 0.00, 'EUR', 
 10.16, 4.34, 4.34, '2020-08-07', '2021-01-13', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '100'),
('2283', '19710400', 'MALHA ELECT-SOLD.FND 120', 'MALHA ELECT-SOLD.FND 120 (PAINEL)', 'M2', 0.00, 'EUR', 
 14.64, 6.59, 6.59, '2023-07-27', '2023-09-19', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '120'),
('2284', '19710401', 'MALHA ELECT-SOLD.FND 120', 'MALHA ELECT-SOLD.FND 120 (PAINEL EMPAL. 6x2.2m)', 'M2', 0.00, 'EUR', 
 14.64, 6.49, 6.49, '2024-01-31', '2024-05-22', 16, NULL, 9, 8, 20, NULL, NULL,6000.00, 2200.00,  NULL, NULL, NULL, '120'),
('2285', '19710430', 'MALHA ELECT-SOLD.FAQ 30', 'MALHA ELECT-SOLD.FAQ 30 (ROLO)', 'M2', 960.00, 'EUR', 
 1.37, 0.79, 0.82, '2024-05-24', '2024-06-04', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '30'),
('2286', '19710431', 'MALHA ELECT-SOLD.FAQ 30', 'MALHA ELECT-SOLD.FAQ 30 (PAINEL)', 'M2', 0.00, 'EUR', 
 1.37, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '30'),
('2287', '19710431', 'MALHA ELECT-SOLD.FAQ 30', 'MALHA ELECT-SOLD.FAQ 30 (PAINEL)', 'UN', 0.00, 'EUR', 
 1.37, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '30'),
('2288', '19710438', 'MALHA ELECT-SOLD.FAQ 38', 'MALHA ELECT-SOLD.FAQ 38 (ROLO)', 'M2', 960.00, 'EUR', 
 2.22, 1.33, 1.33, '2024-05-24', '2024-06-11', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '38'),
('2289', '19710439', 'MALHA ELECT-SOLD.FAQ 38', 'MALHA ELECT-SOLD.FAQ 38 (PAINEL)', 'M2', 0.00, 'EUR', 
 2.22, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '38'),
('2290', '19710439', 'MALHA ELECT-SOLD.FAQ 38', 'MALHA ELECT-SOLD.FAQ 38 (PAINEL)', 'UN', 0.00, 'EUR', 
 2.22, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '38'),
('2291', '1971044', 'MALHA ELECT-SOLD.FNQ 40', 'MALHA ELECT-SOLD.FNQ 40 (PAINEL)', 'M2', 0.00, 'EUR', 
 2.45, 1.06, 1.06, '2020-04-16', '2020-04-17', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '40');




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2292', '19710440', 'MALHA ELECT-SOLD.FNQ 40', 'MALHA ELECT-SOLD.FNQ 40 (ROLO)', 'M2', 0.00, 'EUR', 
 2.45, 1.19, 0.00, '2020-04-17', '2020-04-17', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '40'),
('2293', '19710441', 'MALHA ELECT-SOLD.FNQ 40', 'MALHA ELECT-SOLD.FNQ 40 (PAINEL)', 'M2', 0.00, 'EUR', 
 2.45, 1.19, 1.19, '2018-09-07', '2018-09-07', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '40'),
('2294', '19710445', 'MALHA ELECT-SOLD.FNQ 45', 'MALHA ELECT-SOLD.FNQ 45 (ROLO)', 'M2', 0.00, 'EUR', 
 3.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '45'),
('2295', '19710446', 'MALHA ELECT-SOLD.FNQ 45', 'MALHA ELECT-SOLD.FNQ 45 (PAINEL)', 'M2', 0.00, 'EUR', 
 3.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '45'),
('2296', '19710446', 'MALHA ELECT-SOLD.FNQ 45', 'MALHA ELECT-SOLD.FNQ 45 (PAINEL)', 'UN', 0.00, 'EUR', 
 3.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '45'),
('2297', '19710450', 'MALHA ELECT-SOLD.FNQ 50', 'MALHA ELECT-SOLD.FNQ 50 (ROLO)', 'M2', 384.00, 'EUR', 
 3.82, 2.29, 2.29, '2024-05-24', '2024-06-13', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50'),
('2298', '19710451', 'MALHA ELECT-SOLD.FNQ 50', 'MALHA ELECT-SOLD.FNQ 50 (PAINEL)', 'M2', 1483.20, 'EUR', 
 3.82, 2.21, 2.22, '2024-05-03', '2024-06-14', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50'),
('2299', '19710451', 'MALHA ELECT-SOLD.FNQ 50', 'MALHA ELECT-SOLD.FNQ 50 (PAINEL)', 'UN', 1483.20, 'EUR', 
 3.82, 2.21, 2.22, '2024-05-03', '2024-06-14', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50'),
('2300', '19710460', 'MALHA ELECT-SOLD.FNQ 60', 'MALHA ELECT-SOLD.FNQ 60 (PAINEL)', 'M2', 0.00, 'EUR', 
 5.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '60'),
('2301', '19710470', 'MALHA ELECT-SOLD.FNQ 70', 'MALHA ELECT-SOLD.FNQ 70 (PAINEL)', 'M2', 0.00, 'EUR', 
 7.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '70'),
('2302', '19710482', 'MALHA ELECT-SOLD.FNQ 80', 'MALHA ELECT-SOLD.FNQ 80 (PAINEL)', 'M2', 0.00, 'EUR', 
 9.77, 4.01, 5.99, '2020-09-02', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '80'),
('2303', '19710490', 'MALHA ELECT-SOLD.FNQ 90', 'MALHA ELECT-SOLD.FNQ 90 (PAINEL)', 'M2', 0.00, 'EUR', 
 12.38, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '90'),
('2304', '19710499', 'MALHA ELECT-SOLD.FNQ 100', 'MALHA ELECT-SOLD.FNQ 100 (PAINEL)', 'M2', 0.00, 'EUR', 
 15.27, 7.43, 7.43, '2019-05-06', '2019-05-10', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '100'),
('2305', '19710500', 'MALHA ELECT-SOLD.FNQ 120', 'MALHA ELECT-SOLD.FNQ 120 (PAINEL)', 'M2', 0.00, 'EUR', 
 22.02, 12.78, 12.78, '2024-02-01', '2024-02-22', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '120');




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2307', '19710650', 'MALHA ELECT-SOLD.FNC 50', 'MALHA ELECT-SOLD.FNC 50 (PAINEL)', 'M2', 0.00, 'EUR', 
 3.18, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50'),
('2308', '19710660', 'MALHA ELECT-SOLD.FNC 60', 'MALHA ELECT-SOLD.FNC 60 (PAINEL)', 'M2', 0.00, 'EUR', 
 4.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '60'),
('2309', '19710670', 'MALHA ELECT-SOLD.FNC 70', 'MALHA ELECT-SOLD.FNC 70 (PAINEL)', 'M2', 0.00, 'EUR', 
 5.28, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '70'),
('2310', '19710680', 'MALHA ELECT-SOLD.FNC 80', 'MALHA ELECT-SOLD.FNC 80 (PAINEL)', 'M2', 0.00, 'EUR', 
 7.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '80'),
('2311', '19710690', 'MALHA ELECT-SOLD.FNC 90', 'MALHA ELECT-SOLD.FNC 90 (PAINEL)', 'M2', 0.00, 'EUR', 
 8.68, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '90'),
('2312', '19710699', 'MALHA ELECT-SOLD.FNC 100', 'MALHA ELECT-SOLD.FNC 100 (PAINEL)', 'M2', 0.00, 'EUR', 
 10.48, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '100'),
('2313', '19711100', 'MALHA ELECT-SOLD.AC 100', 'MALHA ELECT-SOLD.AC 100 (PAINEL)', 'M2', 0.00, 'EUR', 
 14.39, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '100'),
('2314', '19711160', 'MALHA ELECT-SOLD.AC 60', 'MALHA ELECT-SOLD.AC 60 (PAINEL)', 'M2', 0.00, 'EUR', 
 4.94, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '60'),
('2315', '19711170', 'MALHA ELECT-SOLD.AC 70', 'MALHA ELECT-SOLD.AC 70 (PAINEL)', 'M2', 0.00, 'EUR', 
 2.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '70'),
('2316', '19711182', 'MALHA ELECT-SOLD.AC 82', 'MALHA ELECT-SOLD.AC 82 (PAINEL)', 'M2', 0.00, 'EUR', 
 8.64, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '82'),
('2317', '19711190', 'MALHA ELECT-SOLD.AC 90', 'MALHA ELECT-SOLD.AC 90 (PAINEL)', 'M2', 0.00, 'EUR', 
 10.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '90'),
('2318', '19711195', 'MALHA ELEC/SOLD.200x200x5x5', 'MALHA ELEC/SOLD.200x200x5x5', 'M2', 0.00, 'EUR', 
 1.66, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL,NULL, 200.00, 200.00, NULL, 5.00, NULL, '5x5'),
('2319', '19711196', 'MALHA ELEC/SOLD.200x200x6x6', 'MALHA ELEC/SOLD.200x200x6x6', 'M2', 1280.40, 'EUR', 
 2.38, 1.42, 1.43, '2024-04-23', '2024-06-06', 16, NULL, 9, 8, 20, NULL,NULL, 200.00, 200.00, NULL, 6.00, NULL, '6x6'),
('2320', '19711200', 'MALHA ELEC/SOLD.200x200x8X8', 'MALHA ELEC/SOLD.200x200x8X8', 'M2', 2099.40, 'EUR', 
 4.03, 2.37, 2.35, '2024-05-24', '2024-06-12', 16, NULL, 9, 8, 20, NULL, NULL,200.00, 200.00, NULL, 8.00, NULL, '8X8'),
('2321', '19711201', 'MALHA ELEC/SOLD.200x200x10X10', 'MALHA ELEC/SOLD.200x200x10X10', 'M2', 1069.20, 'EUR', 
 6.28, 3.68, 3.68, '2024-05-24', '2024-06-14', 16, NULL, 9, 8, 20, NULL, NULL,200.00, 200.00, NULL, 10.00, NULL, '10X10'),
('2322', '19711212', 'MALHA ELEC/SOLD.200x200x12X12', 'MALHA ELEC/SOLD.200x200x12X12', 'M2', 39.60, 'EUR', 
 8.57, 5.10, 5.13, '2024-03-13', '2024-06-06', 16, NULL, 9, 8, 20, NULL,NULL, 200.00, 200.00, NULL, 12.00, NULL, '12X12'),
('2323', '19712030', 'MALHA- ELECT.SOLDADA M DQ 30', 'MALHA- ELECT.SOLDADA M DQ 30', 'MT', 0.00, 'EUR', 
 7.98, 0.00, 0.00, '1900-01-01', '1900-01-01', 16, NULL, 9, 8, 20, NULL, NULL, NULL,NULL, NULL, NULL, NULL, '30');




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('10945', '70279637', 'REDE P/EMULSOR COLUNA', 'REDE P/EMULSOR COLUNA', 'UN', 0.00, 'EUR', 
 0.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 9, 1, 11, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
('10946', '70279637', 'REDE P/EMULSOR COLUNA', 'REDE P/EMULSOR COLUNA', 'UN', 0.00, 'PTE', 
 160.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 9, 1, 11, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
('10947', '70280155', 'REDE P/EMLS COLUN BIC GIR', 'REDE P/EMLS COLUN BIC GIR', 'UN', 0.00, 'EUR', 
 0.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 9, 1, 11, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
('10948', '70280155', 'REDE P/EMLS COLUN BIC GIR', 'REDE P/EMLS COLUN BIC GIR', 'UN', 0.00, 'PTE', 
 160.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 9, 1, 11, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);




-- PAINEIS



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1624', '18315138', 'CUMIEIRA RECORTADA P/PAINEL SANDWICH', 'CUMIEIRA RECORTADA P/PAINEL SANDWICH (COBERTURA)', 'M2', 0.00, 'EUR', 
 5.39, 4.50, 4.50, '2023-03-28', '2023-04-28', 62, NULL, 10, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
('1625', '18315138', 'CUMIEIRA RECORTADA P/PAINEL SANDWICH', 'CUMIEIRA RECORTADA P/PAINEL SANDWICH (COBERTURA)', 'MT', 0.00, 'EUR', 
 11.00, 4.50, 4.50, '2023-03-28', '2023-04-28', 62, NULL, 10, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
('1628', '18315141', 'PAINEL COBERT. 30mm(Económico) 3/ONDAS', 'PAINEL COBERT. 30mm(Económico) 3/ONDAS', 'M2', 0.00, 'EUR', 
 15.89, 4.60, 4.60, '2017-10-10', '2018-12-07', 62, NULL, 10, 1, NULL, NULL, NULL, NULL, NULL, NULL, 30.00, NULL, '30'),
('1629', '18315142', 'PAINEL SANDWICH 5/ONDAS', 'PAINEL SANDWICH 5/ONDAS PC5 1000x30MM', 'M2', 0.00, 'EUR', 
 17.40, 15.60, 15.60, '2023-03-16', '2023-04-28', 62, NULL, 10, 1, NULL, NULL, NULL, 1000.00, NULL, NULL, 30.00, NULL, '30'),
('1630', '18315143', 'PAINEIS SANDWICH PW', 'PAINEIS SANDWICH PW 1000x40 (fachada) FIX/VISTA', 'M2', 0.00, 'EUR', 
 0.00, 25.00, 25.00, '2023-01-06', '2023-01-06', 62, NULL, 10, 1, NULL, NULL, NULL, 1000.00, NULL, NULL, 40.00, NULL, '40'),
('1631', '18315144', 'TOPOS P/PAINEL POLIUT.', 'TOPOS P/PAINEL POLIUT.2000 P/COBERT.(REMATES)', 'UN', 0.00, 'EUR', 
 10.40, 4.25, 4.25, '2017-10-10', '2018-12-07', 62, NULL, 10, 1, NULL, NULL, NULL, 2000.00, NULL, NULL, NULL, NULL, '2000'),
('1632', '18315145', 'PAINEL SANDWICH 5/ONDAS', 'PAINEL SANDWICH 5/ONDAS PC5 1000x80mm', 'M2', 0.00, 'EUR', 
 21.50, 16.65, 16.65, '2019-08-13', '2019-10-14', 62, NULL, 10, 1, NULL, NULL, NULL, 1000.00, NULL, NULL, 80.00, NULL, '80'),
('1649', '18315162', 'PAINEL FACHADA BOX', 'PAINEL FACHADA BOX 1000x30mm', 'M2', 0.00, 'EUR', 
 16.00, 6.00, 6.00, '2019-01-15', '2019-01-16', 62, NULL, 10, 1, NULL, NULL, NULL, 1000.00, NULL, NULL, 30.00, NULL, '30'),
('1655', '18315168', 'PAINEL  FACHADA 3 ONDAS INV.', 'PAINEL  FACHADA 3 ONDAS INV. FIX OCULTA 1000x30mm', 'M2', 0.00, 'EUR', 
 15.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL, NULL, 1000.00, NULL, NULL, 30.00, NULL, '30'),
('1656', '18315169', 'PAINEL COBERT.', 'PAINEL COBERT.50mm (económico) 3/ONDAS', 'M2', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL, NULL, NULL, NULL, NULL, 50.00, NULL, '50'),
('1790', '18600814', 'METAL DIST.', 'METAL DIST. 60x23x3x4 (3x1 PAINEL)', 'M2', 0.00, 'EUR', 
 31.60, 10.90, 10.92, '2018-12-14', '2019-07-08', 15, NULL, 10, 1, NULL, NULL, NULL, 60.00, 23.00, 3.00, 4.00, NULL, NULL),
('1997', '19610001', 'PAINEL.QUAD.', 'PAINEL.QUAD.34x38 -30x2-5  ENTREGIRADO NEGRO 6x1', 'M2', 0.00, 'EUR', 
 24.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 2, NULL, NULL, NULL, NULL, 30.00, 2.00, 5.00, NULL),
('1998', '19620000', 'PAINEL NERVOM.', 'PAINEL NERVOM. 2500x600x0,4', 'UN', 0.00, 'EUR', 
 3.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, NULL, NULL, 2, 2500.00, 600.00, NULL, 0.40, NULL, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('1999', '19621060', 'PAINEL VEDAÇAO TOP/GALVANIZADO', 'PAINEL VEDAÇAO TOP/GALVANIZADO 0,60MT', 'UN', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, NULL, 2, NULL, 0.60, NULL, NULL, NULL, NULL, '0.60'),
('2000', '19621084', 'PAINEL VEDAÇAO TOP/GALVANIZADO', 'PAINEL VEDAÇAO TOP/GALVANIZADO 0,84MT', 'UN', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, NULL, 2,NULL,  0.84, NULL, NULL, NULL, NULL, '0.84'),
('2001', '19621100', 'PAINEL VEDAÇAO TOP/GALVANIZADO', 'PAINEL VEDAÇAO TOP/GALVANIZADO 1,040MT', 'UN', 0.00, 'EUR', 
 15.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, NULL, 2, NULL, 1.04, NULL, NULL, NULL, NULL, '1.04'),
('2027', '19650423', 'PAINEL ELECT/GALV.', 'PAINEL ELECT/GALV.2000x1000 30X30X3X3mm', 'M2', 1334.00, 'EUR', 
 11.75, 6.48, 6.46, '2024-04-12', '2024-06-13', 15, NULL, 10, 1, 20, 2, NULL,2000.00, 1000.00, NULL, 3.00, NULL, '30X30'),
('2028', '19650424', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 50x50x3  2,00x1,00MT', 'M2', 0.00, 'EUR', 
 14.65, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2, NULL,2000.00, 1000.00, NULL, 3.00, NULL, '50X50'),
('2029', '19650425', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 50x50x5 C/ 2000x1000  mm', 'M2', 0.00, 'EUR', 
 14.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2, NULL,2000.00, 1000.00, NULL, 5.00, NULL, '50X50'),
('2030', '19650426', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 50x50x4  2,60x1,50MT', 'M2', 1185.60, 'EUR', 
 9.10, 4.78, 4.78, '2024-06-12', '2024-06-14', 15, NULL, 10, 1, 20, 2, NULL,2600.00, 1500.00, NULL, 4.00, NULL, '50X50'),
('2031', '19650427', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 50x50x4  3,00x2,00 MT', 'M2', 2358.00, 'EUR', 
 9.10, 4.41, 4.41, '2024-06-12', '2024-06-13', 15, NULL, 10, 1, 20, 2, NULL,3000.00, 2000.00, NULL, 4.00, NULL, '50X50'),
('2032', '19650428', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 50x50x4  2600x2000', 'M2', 0.00, 'EUR', 
 9.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2, NULL,2600.00, 2000.00, NULL, 4.00, NULL, '50X50'),
('2033', '19650430', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 50x50x4  3,00x1,50MT', 'M2', 153.00, 'EUR', 
 9.10, 4.55, 4.52, '2024-03-25', '2024-06-14', 15, NULL, 10, 1, 20, 2, NULL,3000.00, 1500.00, NULL, 4.00, NULL, '50X50'),
('2034', '19650520', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 50x50x5  2,60x2,00MT', 'M2', 0.00, 'EUR', 
 14.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2,NULL, 2600.00, 2000.00, NULL, 5.00, NULL, '50X50'),
('2035', '19650526', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 50x50x5  2,60x1,50MT', 'M2', 444.60, 'EUR', 
 14.60, 7.94, 7.96, '2023-10-28', '2024-06-13', 15, NULL, 10, 1, 20, 2, NULL,2600.00, 1500.00, NULL, 5.00, NULL, '50X50'),
('2036', '19650530', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 50x50x5  3,00x1,50MT', 'M2', 504.00, 'EUR', 
 14.60, 7.76, 7.76, '2022-05-18', '2024-06-12', 15, NULL, 10, 1, 20, 2, NULL,3000.00, 1500.00, NULL, 5.00, NULL, '50X50'),
('2037', '19650626', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV.100x50x5 2,60x1,50MT', 'M2', 585.00, 'EUR', 
 10.95, 5.98, 5.98, '2024-06-12', '2024-03-27', 15, NULL, 10, 1, 20, 2, NULL,2600.00, 1500.00, NULL, 5.00, NULL, '100X50'),
('2038', '19650629', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV.100x50x5 2,60x2,00', 'M2', 0.00, 'EUR', 
 10.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2, NULL,2600.00, 2000.00, NULL, 5.00, NULL, '100X50'),
('2039', '19650630', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV.100x50x5 3,00x1,50MT', 'M2', 0.00, 'EUR', 
 10.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2,NULL, 3000.00, 1500.00, NULL, 5.00, NULL, '100X50');



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2040', '19650635', 'PAINEL ELECT.PRÉ/GALV.', 'PAINEL ELECT.PRÉ/GALV.150x50x5 3,00x2,00MT', 'M2', 0.00, 'EUR', 
 12.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2, NULL,3000.00, 2000.00, NULL, 5.00, NULL, '150X50'),
('2041', '19650726', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV.100x50x4 3,00x1,50 MT', 'M2', 0.00, 'EUR', 
 7.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2, NULL,3000.00, 1500.00, NULL, 4.00, NULL, '100X50'),
('2042', '19650730', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV.200x50x4 3,00x1,50MT', 'M2', 0.00, 'EUR', 
 7.10, 3.54, 3.54, '2023-03-22', '2023-03-24', 15, NULL, 10, 1, 20, 2, NULL,3000.00, 1500.00, NULL, 4.00, NULL, '200X50'),
('2043', '19650826', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV.200x50x5 2,60x1,50MT', 'M2', 744.90, 'EUR', 
 8.41, 4.59, 4.59, '2024-02-23', '2024-05-17', 15, NULL, 10, 1, 20, 2, NULL,2600.00, 1500.00, NULL, 5.00, NULL, '200X50'),
('2044', '19650830', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV.200x50x5 3,00x1,50MT', 'M2', 0.00, 'EUR', 
 8.41, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2, NULL,3000.00, 1500.00, NULL, 5.00, NULL, '200X50'),
('2045', '19650831', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV.150x50x5x5 C/ 2000x1500 MM', 'M2', 0.00, 'EUR', 
 17.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2, NULL,2000.00, 1500.00, NULL, 5.00, NULL, '150X50'),
('2047', '19650833', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 30x30x3x3 mm 2,60x1,50 mt', 'M2', 0.00, 'EUR', 
 11.75, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2, NULL,2600.00, 1500.00, NULL, 3.00, NULL, '30X30'),
('2048', '19650833', 'PAINEL ELECT.GALV.', 'PAINEL ELECT.GALV. 30x30x3x3 mm 2,60x1,50 mt', 'UN', 0.00, 'EUR', 
 11.75, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, 20, 2,NULL, 2600.00, 1500.00, NULL, 3.00, NULL, '30X30');


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2049', '19650900', 'MODULOS/PAINEIS GALV.', 'MODULOS/PAINEIS GALV. P/OBRAS (3540x1900)', 'UN', 107.00, 'EUR', 
 38.00, 18.39, 17.81, '2023-12-12', '2024-06-04', 15, NULL, 10, 1, NULL, 2, NULL,3540.00, 1900.00, NULL, NULL, NULL, '3540X1900'),
('2051', '19650902', 'BASES EM BETAO', 'BASES EM BETAO P/PAINEL DE VEDAÇAO', 'UN', 9.00, 'EUR', 
 9.00, 4.00, 4.00, '2024-05-24', '2024-06-04', 15, NULL, 10, 1, NULL, NULL, NULL,NULL, NULL, NULL, NULL, NULL, NULL),
('2128', '19660500', 'PAINEL HERC.', 'PAINEL HERC. 2500x500 VERDE', 'UN', 0.00, 'EUR', 
 26.00, 9.38, 9.38, '2017-10-10', '2018-11-30', 15, NULL, 10, 1, 21, 8,NULL, 2500.00, 500.00, NULL, NULL, NULL, '2500X500'),
('2129', '19660501', 'PAINEL HERC.', 'PAINEL HERC. 2500x500 BRANCO', 'UN', 0.00, 'EUR', 
 26.00, 9.76, 9.76, '2017-10-10', '2018-11-28', 15, NULL, 10, 1, 21, 9,NULL, 2500.00, 500.00, NULL, NULL, NULL, '2500X500'),
('2130', '19660502', 'PORTÃO PAINEL VED.', 'PORTÃO PAINEL VED. COMP. VERDE 1000X1000', 'UN', 0.00, 'EUR', 
 180.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, NULL, 8, NULL,1000.00, 1000.00, NULL, NULL, NULL, '1000X1000');





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2131', '19660640', 'PAINEL HERC.', 'PAINEL HERC. 2500x600 VERDE', 'UN', 110.00, 'EUR', 
 20.90, 10.93, 10.68, '2024-06-12', '2024-05-29', 15, NULL, 10, 1, 21, 8,  NULL,2500.00, 600.00, NULL, NULL, NULL, '2500X600'),
('2132', '19660641', 'PAINEL HERC.', 'PAINEL HERC. 2500x600 BRANCO', 'UN', 99.00, 'EUR', 
 20.90, 11.48, 10.68, '2023-10-28', '2024-05-10', 15, NULL, 10, 1, 21, 9,  NULL,2500.00, 600.00, NULL, NULL, NULL, '2500X600'),
('2133', '19660642', 'PAINEL HERC.', 'PAINEL HERC. 2500x600 CINZA', 'UN', 132.00, 'EUR', 
 20.90, 11.95, 10.68, '2024-03-25', '2024-06-03', 15, NULL, 10, 1, 21, 10, NULL, 2500.00, 600.00, NULL, NULL, NULL, '2500X600'),
('2134', '19660840', 'PAINEL HERC.', 'PAINEL HERC. 2500x800 VERDE', 'UN', 84.00, 'EUR', 
 25.85, 13.74, 13.21, '2023-11-15', '2024-05-29', 15, NULL, 10, 1, 21, 8,  NULL,2500.00, 800.00, NULL, NULL, NULL, '2500X800'),
('2135', '19660841', 'PAINEL HERC.', 'PAINEL HERC. 2500x800 BRANCO', 'UN', 77.00, 'EUR', 
 25.85, 13.62, 13.55, '2023-07-26', '2024-06-07', 15, NULL, 10, 1, 21, 9,  NULL,2500.00, 800.00, NULL, NULL, NULL, '2500X800'),
('2136', '19660842', 'PAINEL HERC.', 'PAINEL HERC. 2500x800 CINZA', 'UN', 92.00, 'EUR', 
 25.85, 14.78, 13.21, '2024-04-03', '2024-06-07', 15, NULL, 10, 1, 21, 10, NULL, 2500.00, 800.00, NULL, NULL, NULL, '2500X800'),
('2137', '19661040', 'PAINEL HERC.', 'PAINEL HERC. 2500x1000 VERDE', 'UN', 124.00, 'EUR', 
 29.30, 15.05, 15.01, '2024-06-12', '2024-06-04', 15, NULL, 10, 1, 21, 8,  NULL,2500.00, 1000.00, NULL, NULL, NULL, '2500X1000'),
('2138', '19661041', 'PAINEL HERC.', 'PAINEL HERC. 2500x1000 BRANCO', 'UN', 103.00, 'EUR', 
 29.30, 15.39, 15.01, '2024-03-25', '2024-05-17', 15, NULL, 10, 1, 21, 9,  NULL,2500.00, 1000.00, NULL, NULL, NULL, '2500X1000'),
('2139', '19661042', 'PAINEL HERC.', 'PAINEL HERC. 2500x1000 CINZA', 'UN', 184.00, 'EUR', 
 29.30, 15.01, 15.01, '2024-06-12', '2024-06-13', 15, NULL, 10, 1, 21, 10, NULL, 2500.00, 1000.00, NULL, NULL, NULL, '2500X1000'),
('2140', '19661124', 'PAINEL HERC.', 'PAINEL HERC. 2500x1200 VERDE', 'UN', 89.00, 'EUR', 
 34.80, 18.46, 17.96, '2024-03-25', '2024-06-13', 15, NULL, 10, 1, 21, 8,  NULL,2500.00, 1200.00, NULL, NULL, NULL, '2500X1200'),
('2141', '19661125', 'PAINEL HERC.', 'PAINEL HERC. 2500x1200 BRANCO', 'UN', 136.00, 'EUR', 
 34.80, 20.07, 20.43, '2023-03-22', '2024-06-13', 15, NULL, 10, 1, 21, 9,  NULL,2500.00, 1200.00, NULL, NULL, NULL, '2500X1200'),
('2142', '19661126', 'PAINEL HERC.', 'PAINEL HERC. 2500x1200 CINZA', 'UN', 191.00, 'EUR', 
 34.80, 18.07, 17.96, '2024-06-12', '2024-06-11', 15, NULL, 10, 1, 21, 10, NULL, 2500.00, 1200.00, NULL, NULL, NULL, '2500X1200'),
('2143', '19661540', 'PAINEL HERC.', 'PAINEL HERC. 2500x1500 VERDE', 'UN', 52.00, 'EUR', 
 44.10, 21.75, 21.59, '2024-03-25', '2024-06-14', 15, NULL, 10, 1, 21, 8, NULL, 2500.00, 1500.00, NULL, NULL, NULL, '2500X1500'),
('2144', '19661541', 'PAINEL HERC.', 'PAINEL HERC. 2500x1500 BRANCO', 'UN', 158.00, 'EUR', 
 44.10, 24.00, 25.91, '2023-03-14', '2024-05-20', 15, NULL, 10, 1, 21, 9,  NULL,2500.00, 1500.00, NULL, NULL, NULL, '2500X1500'),
('2145', '19661542', 'PAINEL HERC.', 'PAINEL HERC. 2500x1500 CINZA', 'UN', 135.00, 'EUR', 
 44.10, 21.60, 21.59, '2024-03-25', '2024-06-06', 15, NULL, 10, 1, 21, 10, NULL, 2500.00, 1500.00, NULL, NULL, NULL, '2500X1500'),
('2146', '19661543', 'PAINEL HERC.', 'PAINEL HERC.2500X1700 CINZA', 'UN', 106.00, 'EUR', 
 48.20, 23.48, 23.48, '2024-06-12', '2024-05-08', 15, NULL, 10, 1, 21, 10,  NULL,2500.00, 1700.00, NULL, NULL, NULL, '2500X1700'),
('2147', '19661740', 'PAINEL HERC.', 'PAINEL HERC. 2500x1700 VERDE', 'UN', 91.00, 'EUR', 
 48.20, 24.12, 23.48, '2024-03-25', '2024-05-03', 15, NULL, 10, 1, 21, 8,  NULL,2500.00, 1700.00, NULL, NULL, NULL, '2500X1700');





INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2148', '19661741', 'PAINEL HERC.', 'PAINEL HERC. 2500x1700 BRANCO', 'UN', 10.00, 'EUR', 
 48.20, 19.12, 19.34, '2018-07-04', '2023-02-20', 15, NULL, 10, 1, 21, 9,  NULL,2500.00, 1700.00, NULL, NULL, NULL, '2500X1700'),
('2149', '19662000', 'PAINEL HERC.', 'PAINEL HERC. 2500x2000 VERDE', 'UN', 105.00, 'EUR', 
 56.60, 28.47, 28.41, '2024-06-12', '2024-06-12', 15, NULL, 10, 1, 21, 8,  NULL,2500.00, 2000.00, NULL, NULL, NULL, '2500X2000'),
('2150', '19662001', 'PAINEL HERC.', 'PAINEL HERC. 2500x2000 BRANCO', 'UN', 132.00, 'EUR', 
 56.60, 28.00, 28.41, '2024-02-21', '2024-05-09', 15, NULL, 10, 1, 21, 9, NULL, 2500.00, 2000.00, NULL, NULL, NULL, '2500X2000'),
('2151', '19662002', 'PAINEI HERC.', 'PAINEI HERC. 2500x2000 CINZA', 'UN', 133.00, 'EUR', 
 56.60, 28.41, 28.41, '2024-04-03', '2024-05-08', 15, NULL, 10, 1, 21, 10, NULL, 2500.00, 2000.00, NULL, NULL, NULL, '2500X2000'),
('2152', '19662003', 'PAINEL MOR.', 'PAINEL MOR 2500x1500 PRETO', 'UN', 0.00, 'EUR', 
 40.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, NULL, 3,  NULL,2500.00, 1500.00, NULL, NULL, NULL, '2500X1500'),
('2153', '19662005', 'PAINEL MOR.', 'PAINEL MOR 2500x2430 VERDE', 'UN', 0.00, 'EUR', 
 40.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 15, NULL, 10, 1, NULL, 8, NULL, 2500.00, 2430.00, NULL, NULL, NULL, '2500X2430'),
('2154', '19662006', 'PAINEL SECURIPLUS.', 'PAINEL SECURIPLUS 2500x1200mm CINZA', 'UN', 0.00, 'EUR', 
 47.60, 42.73, 42.73, '2018-11-19', '2018-08-01', 15, NULL, 10, 1, NULL, 10, NULL, 2500.00, 1200.00, NULL, NULL, NULL, '2500X1200'),
('2490', '19999013', 'FIXADOR DE PAINEIS.', 'FIXADOR DE PAINEIS C/PERNO', 'UN', 0.00, 'EUR', 
 1.30, 0.67, 0.67, '2017-10-10', '2022-07-19', 61, NULL, 10, 1, NULL, NULL, NULL,  NULL,NULL, NULL, NULL, NULL, NULL);




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('2665', '19999300', 'PAINEIS QUAD. GALV.', 'PAINEIS QUAD. GALV.(ESTRADOS) 1000x1000x30', 'UN', 34.00, 'EUR', 
 65.50, 35.32, 35.10, '2024-04-17', '2024-06-13', 2, NULL, 10, 1, 2, 2,NULL,  1000.00, 1000.00, 30.00, NULL, NULL, '1000X1000X30'),
('2667', '19999302', 'PAINEIS QUAD. GALV.', 'PAINEIS QUAD. GALV.(ESTRADOS) 2000x1000x30', 'UN', 0.00, 'EUR', 
 121.50, 66.71, 66.70, '2024-04-17', '2024-06-12', 2, NULL, 10, 1, 2, 2, NULL, 2000.00, 1000.00, 30.00, NULL, NULL, '2000X1000X30'),
('2669', '19999304', 'PAINEIS QUAD. GALV.', 'PAINEIS QUAD. GALV.(ESTRADOS) 3000x1000x30', 'UN', 96.00, 'EUR', 
 175.90, 99.02, 98.99, '2024-04-17', '2024-06-14', 2, NULL, 10, 1, 2, 2,NULL,  3000.00, 1000.00, 30.00, NULL, NULL, '3000X1000X30'),
('2671', '19999306', 'PAINEIS QUAD. GALV.', 'PAINEIS QUAD. GALV.(ESTRADOS) 860x460x30', 'UN', 0.00, 'EUR', 
 48.00, 30.35, 30.35, '2022-05-06', '2022-05-11', 2, NULL, 10, 1, 2, 2,NULL,  860.00, 460.00, 30.00, NULL, NULL, '860X460X30'),
('2672', '19999308', 'PAINEIS QUAD.', 'PAINEIS QUAD.(ESTRADOS) 6000x1000x30 NEGRO', 'UN', 0.00, 'EUR', 
 198.00, 0.00, 0.00, NULL, NULL, 2, NULL, 10, 1, 2, NULL,NULL,  6000.00, 1000.00, 30.00, NULL, NULL, '6000X1000X30'),
('2673', '19999309', 'PAINEIS QUAD. GALV.', 'PAINEIS QUAD. GALV.(ESTRADOS) 6000x1000x30', 'UN', 52.00, 'EUR', 
 368.50, 199.07, 198.55, '2024-04-17', '2024-06-11', 2, NULL, 10, 1, 2, 2, NULL, 6000.00, 1000.00, 30.00, NULL, NULL, '6000X1000X30');




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('13600', '90150160', 'PAINEL ACRIL.FRONTAL', 'PAINEL ACRIL.FRONTAL 160', 'UN', 0.00, 'EUR', 
 105.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL, NULL, 160.00, NULL, NULL, NULL, NULL, '160'),
('13601', '90150160', 'PAINEL ACRIL.FRONTAL', 'PAINEL ACRIL.FRONTAL 160', 'UN', 0.00, 'PTE', 
 21100.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  160.00, NULL, NULL, NULL, NULL, '160'),
('13602', '90150170', 'PAINEL ACRIL.FRONTAL', 'PAINEL ACRIL.FRONTAL 170', 'UN', 0.00, 'EUR', 
 108.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  170.00, NULL, NULL, NULL, NULL, '170'),
('13603', '90150170', 'PAINEL ACRIL.FRONTAL', 'PAINEL ACRIL.FRONTAL 170', 'UN', 0.00, 'PTE', 
 21800.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL, NULL, 170.00, NULL, NULL, NULL, NULL, '170'),
('13604', '90150270', 'PAINEL ACRIL.LATERAL', 'PAINEL ACRIL.LATERAL 70', 'UN', 0.00, 'EUR', 
 59.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  70.00, NULL, NULL, NULL, NULL, '70'),
('13605', '90150270', 'PAINEL ACRIL.LATERAL', 'PAINEL ACRIL.LATERAL 70', 'UN', 0.00, 'PTE', 
 12000.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  70.00, NULL, NULL, NULL, NULL, '70'),
('13606', '90150275', 'PAINEL ACRIL.LATERAL', 'PAINEL ACRIL.LATERAL 75', 'UN', 0.00, 'EUR', 
 62.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL, NULL, 75.00, NULL, NULL, NULL, NULL, '75'),
('13607', '90150275', 'PAINEL ACRIL.LATERAL', 'PAINEL ACRIL.LATERAL 75', 'UN', 0.00, 'PTE', 
 12600.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL, NULL, 75.00, NULL, NULL, NULL, NULL, '75'),
('13608', '90150280', 'PAINEL ACRIL.LATERAL', 'PAINEL ACRIL.LATERAL 80 BRANCO', 'UN', 0.00, 'EUR', 
 67.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, 9,NULL,  80.00, NULL, NULL, NULL, NULL, '80'),
('13609', '90150280', 'PAINEL ACRIL.LATERAL', 'PAINEL ACRIL.LATERAL 80 BRANCO', 'UN', 0.00, 'PTE', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, 9, NULL, 80.00, NULL, NULL, NULL, NULL, '80'),
('13610', '90151130', 'PAINEL ACRIL.OASIS', 'PAINEL ACRIL.OASIS 130', 'UN', 0.00, 'EUR', 
 181.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  130.00, NULL, NULL, NULL, NULL, '130'),
('13611', '90151130', 'PAINEL ACRIL.OASIS', 'PAINEL ACRIL.OASIS 130', 'UN', 0.00, 'PTE', 
 36400.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  130.00, NULL, NULL, NULL, NULL, '130');




INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('13612', '90152160', 'PAINEL ACRIL.FRON.COR', 'PAINEL ACRIL.FRON.COR 160', 'UN', 0.00, 'EUR', 
 117.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL, NULL, 160.00, NULL, NULL, NULL, NULL, '160'),
('13613', '90152160', 'PAINEL ACRIL.FRON.COR', 'PAINEL ACRIL.FRON.COR 160', 'UN', 0.00, 'PTE', 
 23600.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  160.00, NULL, NULL, NULL, NULL, '160'),
('13614', '90152170', 'PAINEL ACRIL.FRON.COR', 'PAINEL ACRIL.FRON.COR 170', 'UN', 0.00, 'EUR', 
 121.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  170.00, NULL, NULL, NULL, NULL, '170'),
('13615', '90152170', 'PAINEL ACRIL.FRON.COR', 'PAINEL ACRIL.FRON.COR 170', 'UN', 0.00, 'PTE', 
 24400.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  170.00, NULL, NULL, NULL, NULL, '170'),
('13616', '90153070', 'PAINEL ACRIL.LATE.COR', 'PAINEL ACRIL.LATE.COR 70', 'UN', 0.00, 'EUR', 
 66.80, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL, 70.00, NULL, NULL, NULL, NULL, '70'),
('13617', '90153070', 'PAINEL ACRIL.LATE.COR', 'PAINEL ACRIL.LATE.COR 70', 'UN', 0.00, 'PTE', 
 13400.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  70.00, NULL, NULL, NULL, NULL, '70'),
('13618', '90153075', 'PAINEL ACRIL.LATE.COR', 'PAINEL ACRIL.LATE.COR 75', 'UN', 0.00, 'EUR', 
 70.30, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  75.00, NULL, NULL, NULL, NULL, '75'),
('13619', '90153075', 'PAINEL ACRIL.LATE.COR', 'PAINEL ACRIL.LATE.COR 75', 'UN', 0.00, 'PTE', 
 14100.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  75.00, NULL, NULL, NULL, NULL, '75'),
('13620', '90153130', 'PAINEL ACRIL.OASI.COR', 'PAINEL ACRIL.OASI.COR 130', 'UN', 0.00, 'EUR', 
 203.50, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  130.00, NULL, NULL, NULL, NULL, '130'),
('13621', '90153130', 'PAINEL ACRIL.OASI.COR', 'PAINEL ACRIL.OASI.COR 130', 'UN', 0.00, 'PTE', 
 40800.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  130.00, NULL, NULL, NULL, NULL, '130'),
('13628', '90153183', 'PAINEL ACRIL.FRONTAL', 'PAINEL ACRIL.FRONTAL A/18P 180', 'UN', 0.00, 'EUR', 
 112.20, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL, NULL, 180.00, NULL, NULL, NULL, NULL, '180'),
('13629', '90153183', 'PAINEL ACRIL.FRONTAL', 'PAINEL ACRIL.FRONTAL A/18P 180', 'UN', 0.00, 'PTE', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL, NULL, 180.00, NULL, NULL, NULL, NULL, '180'),
('13632', '90153185', 'PAINEL ACRILICO FRONTAL', 'PAINEL ACRILICO FRONTAL 180 COR', 'UN', 0.00, 'EUR', 
 125.70, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  180.00, NULL, NULL, NULL, NULL, '180'),
('13633', '90153185', 'PAINEL ACRILICO FRONTAL', 'PAINEL ACRILICO FRONTAL 180 COR', 'UN', 0.00, 'PTE', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL,NULL,  180.00, NULL, NULL, NULL, NULL, '180'),
('13946', '98900001', 'PAINEL ALUMISOL', 'PAINEL ALUMISOL 1200x1200x50', 'M2', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 10, 1, NULL, NULL, NULL, 1200.00, 1200.00, NULL, 50.00, NULL, '1200x1200x50');







-- CORREÇOES DE ITEMS ANTERIORES


INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('3538', '23610005', 'VARAO RED.', 'VARAO RED. INOX 304 5mm', 'KG', 0.00, 'EUR', 
 3.82, 3.17, 3.17, '2017-10-10', '2018-12-28', 62, NULL, 1, 4, 11, NULL, NULL, NULL, NULL, NULL, NULL, 5.00, '5mm'),
('3539', '23610006', 'VARAO RED.', 'VARAO RED. INOX 304 6mm', 'KG', 0.00, 'EUR', 
 2.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 4, 11, NULL, NULL, NULL, NULL, NULL, NULL, 6.00, '6mm'),
('3540', '23610008', 'VARÃO RED.', 'VARÃO RED. INOX 304 8mm', 'KG', 0.00, 'EUR', 
 3.33, 4.10, 4.10, '2017-10-10', '2018-12-28', 62, NULL, 1, 4, 11, NULL, NULL, NULL, NULL, NULL, NULL, 8.00, '8mm'),
('3541', '23610010', 'VARAO RED.', 'VARAO RED. INOX 304 10mm', 'KG', 0.00, 'EUR', 
 2.63, 2.65, 2.65, '2017-10-10', '2018-12-28', 62, NULL, 1, 4, 11, NULL, NULL, NULL, NULL, NULL, NULL, 10.00, '10mm'),
('3542', '23610010', 'VARAO RED.', 'VARAO RED. INOX 304 10mm', 'UN', 0.00, 'EUR', 
 2.63, 2.65, 2.65, '2017-10-10', '2018-12-28', 62, NULL, 1, 4, 11, NULL, NULL, NULL, NULL, NULL, NULL, 10.00, '10mm'),
('3543', '23610012', 'VARAO RED.', 'VARAO RED. INOX 304 12mm', 'KG', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 4, 11, NULL, NULL, NULL, NULL, NULL, NULL, 12.00, '12mm'),
('3544', '23610016', 'VARAO RED.', 'VARAO RED. INOX 304 16mm', 'KG', 0.00, 'EUR', 
 2.40, 3.71, 3.71, '2017-10-10', '2018-12-28', 62, NULL, 1, 4, 11, NULL, NULL, NULL, NULL, NULL, NULL, 16.00, '16mm'),
('3545', '23610020', 'VARAO RED.', 'VARAO RED. INOX 304 20mm', 'KG', 0.00, 'EUR', 
 2.40, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 4, 11, NULL, NULL, NULL, NULL, NULL, NULL, 20.00, '20mm'),
('3546', '23610025', 'VARAO RED.', 'VARAO RED. INOX 304 40mm', 'KG', 0.00, 'EUR', 
 3.92, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 4, 11, NULL, NULL, NULL, NULL, NULL, NULL, 40.00, '40mm'),
('3547', '23610025', 'VARAO RED.', 'VARAO RED. INOX 304 40mm', 'UN', 0.00, 'EUR', 
 3.57, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 4, 11, NULL, NULL, NULL, NULL, NULL, NULL, 40.00, '40mm');



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('3579', '23670010', 'VARAO INOX RED.', 'VARAO INOX RED. 316 10mm', 'KG', 0.00, 'EUR', 
 6.20, 5.42, 5.42, '2017-10-10', '2018-12-28', 62, NULL, 1, 4, 11, NULL,  NULL, NULL, NULL, NULL, NULL, 10.00, '10mm'),
('3580', '23670016', 'VARAO INOX RED.', 'VARAO INOX RED. 316 16mm', 'MT', 0.00, 'EUR', 
 6.40, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 4, 11, NULL,  NULL, NULL, NULL, NULL, NULL, 16.00, '16mm'),
('3581', '23670016', 'VARAO INOX RED.', 'VARAO INOX RED. 316 16mm', 'UN', 0.00, 'EUR', 
 6.40, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 4, 11,  NULL, NULL, NULL, NULL, NULL, NULL, 16.00, '16mm'),
('6577', '39000059', 'VARAO ROSCADO', 'VARAO ROSCADO ZINCADO M6 4,8 1MT', 'UN', 0.00, 'EUR', 
 0.25, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 1, 11, 4,  NULL, NULL, NULL, NULL, NULL, 6.00, 'M6'),
('6578', '39000060', 'VARAO ROSCADO', 'VARAO ROSCADO M14 C/1M', 'UN', 0.00, 'EUR', 
 1.14, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 1, 11,  NULL, NULL, NULL, NULL, NULL, NULL, 14.00, 'M14'),
('6579', '39000061', 'VARAO ROSCADO', 'VARAO ROSCADO M16 C/1M', 'UN', 0.00, 'EUR', 
 1.34, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 1, 11,  NULL, NULL, NULL, NULL, NULL, NULL, 16.00, 'M16'),
('6580', '39000062', 'VARAO ROSCADO', 'VARAO ROSCADO M8 C/1M', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 1, 11,  NULL, NULL, NULL, NULL, NULL, NULL, 8.00, 'M8'),
('6581', '39000063', 'VARAO ROSCADO', 'VARAO ROSCADO M12 C/1M', 'MT', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 1, 11,  NULL, NULL, NULL, NULL, NULL, NULL, 12.00, 'M12'),
('6582', '39000064', 'VARAO ROSCADO', 'VARAO ROSCADO M10 C/1MT', 'UN', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 1, 1, 11,  NULL, NULL, NULL, NULL, NULL, NULL, 10.00, 'M10');



INSERT INTO t_product_catalog (
    id, product_code, description, description_full, unit, stock_current, currency, 
    price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, 
    price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, 
    length, width, height, thickness, diameter, nominal_dimension
) VALUES
('13934', '98800015', 'VARAO RED. LISO', 'VARAO RED. LISO C/1 5MM', 'KG', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, NULL, NULL, NULL, NULL, 5.00, '5mm'),
('13935', '98800015', 'VARAO RED. LISO', 'VARAO RED. LISO C/1 5MM', 'KG', 0.00, 'PTE', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, NULL, NULL, NULL, NULL, 5.00, '5mm'),
('13936', '98800016', 'VARAO RED. LISO', 'VARAO RED. LISO C/1 10MM', 'KG', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, NULL, NULL, NULL, NULL, 10.00, '10mm'),
('13937', '98800016', 'VARAO RED. LISO', 'VARAO RED. LISO C/1 10MM', 'KG', 0.00, 'PTE', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, NULL, NULL, NULL, NULL, 10.00, '10mm'),
('13938', '98800017', 'VARAO RED. LISO', 'VARAO RED. LISO C/1 8MM', 'KG', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, NULL, NULL, NULL, NULL, 8.00, '8mm'),
('13939', '98800017', 'VARAO RED. LISO', 'VARAO RED. LISO C/1 8MM', 'KG', 0.00, 'PTE', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, NULL, NULL, NULL, NULL, 8.00, '8mm'),
('13940', '98800018', 'VARAO RED. LISO', 'VARAO RED. LISO C/1 6MM', 'KG', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, NULL, NULL, NULL, NULL, 6.00, '6mm'),
('13941', '98800018', 'VARAO RED. LISO', 'VARAO RED. LISO C/1 6MM', 'KG', 0.00, 'PTE', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, NULL, NULL, NULL, NULL, 6.00, '6mm'),
('13942', '98800019', 'VARAO RED. LISO', 'VARAO RED. LISO C/1 4MM', 'KG', 0.00, 'EUR', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, NULL, NULL, NULL, NULL, 4.00, '4mm'),
('13943', '98800019', 'VARAO RED. LISO', 'VARAO RED. LISO C/1 4MM', 'KG', 0.00, 'PTE', 
 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 12, NULL, 1, 1, 11, NULL, 1, NULL, NULL, NULL, NULL, 4.00, '4mm');



WITH cte AS (
    SELECT 
        MIN(id) AS id
    FROM 
        t_product_catalog
    WHERE 
        type_id = 1
    GROUP BY 
        product_code
)
DELETE FROM t_product_catalog
WHERE id NOT IN (SELECT id FROM cte) AND type_id = 1;




WITH cte AS (
    SELECT 
        MIN(id) AS id
    FROM 
        mf_product_catalog
    WHERE 
        type_id =1
    GROUP BY 
        product_code
)
DELETE FROM t_product_catalog
WHERE id NOT IN (SELECT id FROM cte) AND type_id = 1;



-- MAIS TUBOS

INSERT INTO t_product_catalog (
    id, product_code, description, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, 
    diameter, nominal_dimension
) VALUES
('4159', '33002123', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 0.97, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.30, NULL, 21.30, '21,3x2,3'),
('4160', '33002123', 'TUBO AÇO S/COST', 'MT', 0.00, 'PTE', 195.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.30, NULL, 21.30, '21,3x2,3'),
('4161', '33002137', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 2.12, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.73, NULL, 21.30, '21,3x3,73'),
('4162', '33002137', 'TUBO AÇO S/COST', 'MT', 0.00, 'PTE', 425.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.73, NULL, 21.30, '21,3x3,73'),
('4163', '33002230', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 3.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.00, NULL, 22.00, '22x3'),
('4164', '33002530', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 3.29, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.00, NULL, 25.00, '25x3'),
('4165', '33002726', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 1.76, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.60, NULL, 26.90, '26,9x2,60'),
('4166', '33002830', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 3.64, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.00, NULL, 28.00, '28x3'),
('4167', '33003026', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 2.95, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.60, NULL, 30.00, '30x2,60'),
('4168', '33003029', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 3.93, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.00, NULL, 30.00, '30x3'),
('4169', '33003326', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.60, NULL, 33.70, '33,7x2,6'),
('4170', '33003326', 'TUBO AÇO S/COST', 'MT', 0.00, 'PTE', 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.60, NULL, 33.70, '33,7x2,6'),
('4171', '33003340', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 3.34, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 4.00, NULL, 33.70, '33,7x4,0'),
('4172', '33003340', 'TUBO AÇO S/COST', 'MT', 0.00, 'PTE', 670.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 4.00, NULL, 33.70, '33,7x4,0'),
('4173', '33003429', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 2.10, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.20, NULL, 33.70, '33,7x3,20'),
('4174', '33003829', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 3.37, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.60, NULL, 38.00, '38x2,6'),
('4175', '33003829', 'TUBO AÇO S/COST', 'MT', 0.00, 'PTE', 515.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.60, NULL, 38.00, '38x2,6'),
('4176', '33003850', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 5.00, NULL, 38.00, '38x5,0');





INSERT INTO t_product_catalog (
    id, product_code, description, unit, stock_current, currency, price_pvp, price_avg, 
    price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, 
    material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, 
    diameter, nominal_dimension
) VALUES
('4177', '33003850', 'TUBO AÇO S/COST', 'MT', 0.00, 'PTE', 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 5.00, NULL, 38.00, '38x5,0'),
('4178', '33004226', 'TUBO AÇO S/COST', 'MT', 6.85, 'EUR', 4.81, 4.84, 4.84, '2017-10-10', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.20, NULL, 42.40, '42,4x3,2'),
('4179', '33004226', 'TUBO AÇO S/COST', 'MT', 6.85, 'PTE', 0.00, 4.84, 4.84, '2017-10-10', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.20, NULL, 42.40, '42,4x3,2'),
('4180', '33004232', 'TUBO AÇO S/COST', 'KG', 0.00, 'EUR', 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.60, NULL, 42.40, '42,4x2,6'),
('4181', '33004240', 'TUBO AÇO S/COST', 'MT', 6.17, 'EUR', 5.20, 7.40, 7.40, '2017-10-10', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 4.85, NULL, 42.40, '42,4x4,85'),
('4182', '33004826', 'TUBO AÇO S/COST', 'MT', 5.82, 'EUR', 6.45, 3.97, 3.97, '2017-10-10', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.60, NULL, 48.30, '48,3x2,6'),
('4183', '33004826', 'TUBO AÇO S/COST', 'MT', 5.82, 'PTE', 0.00, 3.97, 3.97, '2017-10-10', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.60, NULL, 48.30, '48,3x2,6'),
('4184', '33004832', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 6.02, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.20, NULL, 48.30, '48,3x3,2'),
('4185', '33004840', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 3.60, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 3.60, NULL, 48.30, '48,3x3,60'),
('4186', '33005126', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 4.61, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.60, NULL, 51.00, '51x2,6'),
('4187', '33005127', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 4.35, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.70, NULL, 51.00, '51x2,7 (ST35.81)'),
('4188', '33005129', 'TUBO AÇO S/COST', 'MT', 0.00, 'EUR', 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 
 '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, 2.90, NULL, 51.00, '51x2,9 (ST35.81)');
 
 
-- Comando UPDATE para inserir a description e description_full dos produtos

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 21,3x2,3'
WHERE id = '4159';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 21,3x2,3'
WHERE id = '4160';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 21,3x3,73'
WHERE id = '4161';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 21,3x3,73'
WHERE id = '4162';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 22x3'
WHERE id = '4163';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 25x3'
WHERE id = '4164';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 26,9x2,60'
WHERE id = '4165';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 28x3'
WHERE id = '4166';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 30x2,60'
WHERE id = '4167';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 30x3'
WHERE id = '4168';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 33,7x2,6'
WHERE id = '4169';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 33,7x2,6'
WHERE id = '4170';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 33,7x4,0'
WHERE id = '4171';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 33,7x4,0'
WHERE id = '4172';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 33,7x3,20'
WHERE id = '4173';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 38x2,6'
WHERE id = '4174';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 38x2,6'
WHERE id = '4175';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 38x5,0'
WHERE id = '4176';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 38x5,0'
WHERE id = '4177';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 42,4x3,2'
WHERE id = '4178';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 42,4x3,2'
WHERE id = '4179';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 42,4x2,6'
WHERE id = '4180';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 42,4x4,85'
WHERE id = '4181';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 48,3x2,6'
WHERE id = '4182';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 48,3x2,6'
WHERE id = '4183';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 48,3x3,2'
WHERE id = '4184';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 48,3x3,60'
WHERE id = '4185';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 51x2,6'
WHERE id = '4186';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 51x2,7 (ST35.81)'
WHERE id = '4187';

UPDATE t_product_catalog
SET description = 'TUBO AÇO S/COST',
    description_full = 'TUBO AÇO SEM COSTURA 51x2,9 (ST35.81)'
WHERE id = '4188';


 
 
 INSERT INTO t_product_catalog (id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES 
('4189', '33005130', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 51x3,2', 'MT', '0.00', 'EUR', '3.22', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '3.2', '51', NULL),
('4190', '33005130', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 51x3,2', 'MT', '0.00', 'PTE', '645.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '3.2', '51', NULL),
('4191', '33005150', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 51x5,0', 'MT', '0.00', 'EUR', '6.26', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.0', '51', NULL),
('4192', '33005150', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 51x5,0', 'MT', '0.00', 'PTE', '1255.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.0', '51', NULL),
('4193', '33006029', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 60,3x2,9', 'MT', '0.00', 'EUR', '4.90', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '2.9', '60.3', NULL),
('4194', '33006029', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 60,3x2,9', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '2.9', '60.3', NULL),
('4195', '33006056', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 60,3x4mm', 'MT', '10.00', 'EUR', '10.40', '3.00', '3.00', '2019-01-07', '2019-01-07', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '4.0', '60.3', NULL),
('4196', '33006063', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 60,3x6,3', 'MT', '0.00', 'EUR', '9.48', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '6.3', '60.3', NULL),
('4197', '33007050', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 70x5,0', 'MT', '0.00', 'EUR', '8.35', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.0', '70', NULL),
('4198', '33007629', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 76,1x2,9', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '2.9', '76.1', NULL),
('4199', '33007629', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 76,1x2,9', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '2.9', '76.1', NULL),
('4200', '33007632', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 76,1x3,6', 'MT', '0.00', 'EUR', '6.25', '3.25', '3.25', '2023-03-02', '2023-03-03', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '3.6', '76.1', NULL);



INSERT INTO t_product_catalog (id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES 
('4202', '33007656', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST.2 1/2 ( 76,1)x4,50', 'MT', '0.00', 'EUR', '12.60', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '4.50', '76.1', NULL),
('4203', '33007663', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 76,1x6,3', 'MT', '0.00', 'EUR', '10.95', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '6.3', '76.1', NULL),
('4204', '33008075', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 80x7,5', 'ML', '0.00', 'EUR', '21.21', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '7.5', '80', NULL),
('4205', '33008214', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 82,5x14,5', 'MT', '0.00', 'EUR', '10.19', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '14.5', '82.5', NULL),
('4206', '33008832', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 88,9x5,49', 'MT', '0.00', 'EUR', '14.36', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.49', '88.9', NULL),
('4207', '33008832', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 88,9x5,49', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.49', '88.9', NULL),
('4208', '33008836', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 88,9x3,6', 'MT', '0.00', 'EUR', '17.50', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '3.6', '88.9', NULL),
('4209', '33008836', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 88,9x3,6', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '3.6', '88.9', NULL),
('4210', '33008863', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 88,9x6,3', 'MT', '0.00', 'EUR', '13.09', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '6.3', '88.9', NULL),
('4211', '33008863', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 88,9x6,3', 'MT', '0.00', 'PTE', '2625.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '6.3', '88.9', NULL),
('4212', '33010180', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 101,6x7,1', 'MT', '0.00', 'EUR', '41.80', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '7.1', '101.6', NULL),
('4213', '33010836', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 108x3,6', 'MT', '0.00', 'EUR', '12.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '3.6', '108', NULL),
('4214', '33010836', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 108x3,6', 'MT', '0.00', 'PTE', '1750.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '3.6', '108', NULL),
('4215', '33010850', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 108x5,0', 'MT', '0.00', 'EUR', '23.27', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.0', '108', NULL),
('4216', '33010850', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 108x5,0', 'MT', '0.00', 'PTE', '4665.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.0', '108', NULL);



INSERT INTO t_product_catalog (id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('4217', '33011436', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 114,3x3,6', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '3.6', '114.3', NULL),
('4218', '33011436', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 114,3x3,6', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '3.6', '114.3', NULL),
('4219', '33011445', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 114,3x4,50', 'MT', '0.00', 'EUR', '9.93', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '4.5', '114.3', NULL),
('4220', '33011445', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 114,3x4,50', 'MT', '0.00', 'PTE', '1990.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '4.5', '114.3', NULL),
('4221', '33011480', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 114,3x8.56', 'MT', '0.00', 'EUR', '31.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '8.56', '114.3', NULL),
('4222', '33011575', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 193,7x5', 'KG', '0.00', 'EUR', '33.80', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5', '193.7', NULL),
('4223', '33011575', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 193,7x5', 'KG', '0.00', 'PTE', '260.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5', '193.7', NULL),
('4224', '33011645', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 168,3x7,1', 'MT', '0.00', 'EUR', '38.10', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '7.1', '168.3', NULL),
('4225', '33011645', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 168,3x7,1', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '7.1', '168.3', NULL),
('4226', '33011956', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 193,7x5,6', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.6', '193.7', NULL),
('4227', '33011956', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 193,7x5,6', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.6', '193.7', NULL),
('4228', '33011957', 'TUBO AÇO S/COST.', 'TUBO ACO S/COST. 193,7x8.00', 'MT', '0.00', 'EUR', '24.94', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '8', '193.7', NULL),
('4229', '33011957', 'TUBO AÇO S/COST.', 'TUBO ACO S/COST. 193,7x8.00', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '8', '193.7', NULL),
('4230', '33012740', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 127x4,0', 'MT', '0.00', 'EUR', '21.60', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '4', '127', NULL),
('4231', '33012740', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 127x4,0', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '4', '127', NULL);



INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('33012780', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 127x8,00', 'ML', '0.00', 'EUR', '43.30', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '8', '127', NULL),
( '33013340', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 133x4,00', 'MT', '0.00', 'EUR', '14.30', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '4', '133', NULL),
('33013350', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 133x5,0', 'MT', '0.00', 'EUR', '23.66', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5', '133', NULL),
('33013935', 'TUBO MEC. ST52', 'TUBO MEC. ST52 139,7x69,7x35', 'ML', '0.00', 'EUR', '148.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL,NULL,  NULL, '69.7', '139.7', NULL, '35', NULL),
('33013940', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 139,7x4.85', 'MT', '0.00', 'EUR', '19.80', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '4.85', '139.7', NULL),
('33013963', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 139,7x6,3', 'MT', '0.00', 'EUR', '31.50', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '6.3', '139.7', NULL),
('33013999', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 139,7x10,0', 'UN', '0.00', 'EUR', '47.40', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '10', '139.7', NULL),
( '33015945', 'TUBO AÇO S/COST. RED.', 'TUBO AÇO S/COST.RED.159x4,5', 'MT', '0.00', 'EUR', '24.65', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', '11', NULL, '10', NULL, NULL, NULL, '4.5', '159', NULL),
( '33016845', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST. 168,3x10mm', 'MT', '0.00', 'EUR', '46.41', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '10', '168.3', NULL),
('33016845', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST. 168,3x10mm', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '10', '168.3', NULL),
('33021956', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 219,1x5,6', 'MT', '0.00', 'EUR', '25.94', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.6', '219.1', NULL),
('33021956', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 219,1x5,6', 'MT', '0.00', 'PTE', '5200.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5.6', '219.1', NULL),
('33021963', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 219,1x6,3', 'MT', '0.00', 'EUR', '46.16', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '6.3', '219.1', NULL),
('33021963', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 219,1x6,3', 'MT', '0.00', 'PTE', '6395.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '6.3', '219.1', NULL),
('33021999', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST 219,1x8,18mm', 'MT', '0.00', 'EUR', '86.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '8.18', '219.1', NULL);




INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('33024463', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 244,5x6,3', 'ML', '0.00', 'EUR', '60.96', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '6.3', '244.5', NULL),
('33027350', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 273x5,0mm', 'MT', '0.00', 'EUR', '28.40', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '5', '273', NULL),
('33027363', 'TUBO AÇO EST. RED.', 'TUBO AÇO EST.RED. 273x12,50', 'MT', '0.00', 'EUR', '200.51', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', '11', NULL, '10', NULL, NULL, NULL, '12.5', '273', NULL),
('33032371', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 323,1x7,10', 'MT', '0.00', 'EUR', '90.50', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '7.1', '323.1', NULL),
('33032371', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 323,1x7,10', 'MT', '0.00', 'PTE', '10030.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '7.1', '323.1', NULL),
('33035580', 'TUBO AÇO S/COST.', 'TUBO AÇO S/COST. 355x8,00', 'MT', '0.00', 'EUR', '71.80', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '8', '355', NULL);



INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('33106371', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST.ST.52 63,5x7,1', 'MT', '0.00', 'EUR', '13.60', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '7.1', '63.5', NULL),
('33107050', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST.ST.52 70x5,0', 'MT', '0.00', 'EUR', '19.08', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '5.0', '70', NULL),
('33107650', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST.ST.52 76,1x5,00', 'MT', '0.00', 'EUR', '19.08', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '5.0', '76.1', NULL),
('33108950', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST.ST.52 88,9x5,0', 'MT', '12.20', 'EUR', '22.84', '10.43', '10.43', '2017-10-10', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '5.0', '88.9', NULL),
('33108963', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST.ST.52 88,9x6,3', 'MT', '0.00', 'EUR', '32.56', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '6.3', '88.9', NULL),
('33110150', 'TUBO EST. RED.', 'TUBO EST. RED. 101,6x10,00', 'MT', '0.00', 'EUR', '59.75', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL,       '6', '2', '11', NULL, NULL, NULL, NULL, NULL, '10.0', '101.6', NULL),
('33111410', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST. 101,1x10mm', 'MT', '0.00', 'EUR', '59.75', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '10', '101.1', NULL),
('33111463', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST.ST.52 114,3x6,3', 'MT', '0.00', 'EUR', '44.69', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '6.3', '114.3', NULL),
('33111463', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST.ST.52 114,3x6,3', 'MT', '0.00', 'PTE', '44.69', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '6.3', '114.3', NULL),
('33113340', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST. 133x3,00', 'MT', '0.00', 'EUR', '20.24', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '3.0', '133', NULL);


INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('33113950', 'TUBO AÇO RED.', 'TUBO AÇO RED. 139,7x10', 'MT', '0.00', 'EUR', '74.09', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', '11', NULL, NULL, NULL, NULL, NULL, '10', '139.7', NULL),
('33113980', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST. 139,7x8mm', 'MT', '0.00', 'EUR', '59.96', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '8', '139.7', NULL),
('33115910', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST. 159x10mm', 'MT', '0.00', 'EUR', '90.29', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '10', '159', NULL),
('33116863', 'TUBO AÇO S355 J2H', 'TUBO AÇO S355 J2H 168,3x10', 'MT', '0.00', 'EUR', '90.29', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '10', '168.3', NULL),
('33119340', 'TUBO AÇO C/COST.', 'TUBO ACO C/COST.ST.37 193,7x4,0', 'MT', '0.00', 'EUR', '39.56', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '4.0', '193.7', NULL),
('33121945', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST. 219,1x4,00', 'MT', '0.00', 'EUR', '45.09', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '4', '219.1', NULL),
('33121945', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST. 219,1x4,00', 'MT', '0.00', 'PTE', '45.09', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '4', '219.1', NULL),
('33121963', 'TUBO AÇO C/COST.', 'TUBO ACO C/COST.ST.37 219,1x6,3', 'MT', '0.00', 'EUR', '86.40', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '6.3', '219.1', NULL),
('33121999', 'TUBO AÇO RED.', 'TUBO AÇO RED.C/COST. 219,1x8mm', 'KG', '0.00', 'EUR', '95.93', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', '11', NULL, '9', NULL, NULL, NULL, '8', '219.1', NULL),
('33124450', 'TUBO AÇO RED.', 'TUBO AÇO EST. RED. 244,5x12mm', 'MT', '0.00', 'EUR', '171.80', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', '11', NULL, NULL, NULL, NULL, NULL, '12', '244.5', NULL),
('33127350', 'TUBO AÇO C/COST.', 'TUBO EST. C/COST. 273x5,00', 'MT', '0.00', 'EUR', '74.30', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '5', '273', NULL),
('33127350', 'TUBO AÇO C/COST.', 'TUBO EST. C/COST. 273x5,00', 'MT', '0.00', 'PTE', '74.30', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '5', '273', NULL),
('33132340', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST. 323,9x4,0', 'KG', '0.00', 'EUR', '69.41', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '4', '323.9', NULL),
('33132340', 'TUBO AÇO C/COST.', 'TUBO AÇO C/COST. 323,9x4,0', 'MT', '0.00', 'EUR', '69.41', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, '9', NULL, NULL, NULL, '4', '323.9', NULL),
('33132350', 'TUBO AÇO RED.', 'TUBO AÇO RED. 323,9x10', 'MT', '0.00', 'EUR', '225.44', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', '11', NULL, NULL, NULL, NULL, NULL, '10', '323.9', NULL);


INSERT INTO t_product_catalog ( product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('33303030', 'TUBO AÇO S/COSTURA', 'TUBO AÇO S/COSTURA 2391 30x03mm', 'MT', '0.00', 'EUR', '4.29', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '2', NULL, NULL, '10', NULL, NULL, NULL, '3', '30', NULL),
('33308032', 'TUBO S/COST.', 'TUBO S/COST. ST.37 82,5x3,2', 'MT', '0.00', 'EUR', '4.30', '0.00', '0.00', '1900-01-01', '1900-01-01', '41', NULL, '6', '1', NULL, NULL, '10', NULL, NULL, NULL, '3.2', '82.5', NULL),
('33313340', 'TUBO C/COST.', 'TUBO C/COST.ST.37 133x4,0', 'MT', '0.00', 'EUR', '26.90', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '9', NULL, NULL, NULL, '4', '133', NULL),
('33315240', 'TUBO AÇO', 'TUBO AÇO ST37 152,4x4,0 (24589', 'MT', '0.00', 'EUR', '32.15', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '4', '152.4', NULL),
('33361063', 'TUBO C/COST. RED.', 'TUBO C/COST.RED. 177,8x5,00', 'MT', '0.00', 'EUR', '46.29', '0.00', '0.00', '1900-01-01', '1900-01-01', '23', NULL, '6', '1', '11', NULL, '9', NULL, NULL, NULL, '5', '177.8', NULL),
('33419363', 'TUBO C/COST. HELICOIDAL', 'TUBO C/COST.HELICOIDAL ST-37.0 193,7x6,3', 'MT', '0.00', 'EUR', '27.80', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '9', NULL, NULL, NULL, '6.3', '193.7', NULL);




INSERT INTO t_product_catalog (id, product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('5587', '36000010', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 10 mm', 'MT', '0.00', 'EUR', '1.20', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '10', NULL),
('5588', '36000010', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 10 mm', 'MT', '0.00', 'PTE', '240.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '10', NULL),
('5589', '36000012', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 12 mm', 'MT', '0.00', 'EUR', '1.50', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '12', NULL),
('5590', '36000012', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 12 mm', 'MT', '0.00', 'PTE', '320.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '12', NULL),
('5591', '36000015', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 15 mm', 'MT', '0.00', 'EUR', '1.68', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '15', NULL),
('5592', '36000015', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 15 mm', 'MT', '0.00', 'PTE', '347.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '15', NULL),
('5593', '36000018', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 18 mm', 'MT', '0.00', 'EUR', '2.31', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '18', NULL),
('5594', '36000018', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 18 mm', 'MT', '0.00', 'PTE', '447.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '18', NULL),
('5595', '36000022', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 22 mm', 'MT', '0.00', 'EUR', '2.76', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL,'304', '22', NULL),
('5596', '36000022', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 22 mm', 'MT', '0.00', 'PTE', '545.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL,'304', '22', NULL),
('5597', '36000028', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 28 mm', 'MT', '0.00', 'EUR', '4.13', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '28', NULL),
('5598', '36000028', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 28 mm', 'MT', '0.00', 'PTE', '767.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '28', NULL),
('5599', '36000035', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 35 mm', 'MT', '0.00', 'EUR', '6.55', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '35', NULL),
('5600', '36000035', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 35 mm', 'MT', '0.00', 'PTE', '1140.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '35', NULL),
('5601', '36000042', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 42 mm', 'MT', '0.00', 'EUR', '8.93', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '42', NULL);




INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('36000042', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 42 mm', 'MT', '0.00', 'PTE', '1420.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '42', NULL),
('36000054', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 54 mm', 'MT', '0.00', 'EUR', '11.90', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '54', NULL),
('36000054', 'TUBO AÇO INOX AISI 304', 'TUBO AÇO INOX AISI 304 54 mm', 'MT', '0.00', 'PTE', '2230.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '2', NULL, NULL, NULL, NULL, NULL, NULL, '304', '54', NULL),
('36000055', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 15 mm', 'MT', '0.00', 'EUR', '2.77', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '15', NULL),
('36000055', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 15 mm', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '15', NULL),
('36000056', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 18 mm', 'MT', '0.00', 'EUR', '3.71', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '18', NULL),
('36000056', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 18 mm', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '18', NULL),
('36000057', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 22 mm', 'MT', '0.00', 'EUR', '4.48', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '22', NULL),
('36000057', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 22 mm', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '22', NULL),
('36000058', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 28 mm', 'MT', '0.00', 'EUR', '6.27', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '28', NULL),
('36000058', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 28 mm', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '28', NULL),
('36000059', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 35 mm', 'MT', '0.00', 'EUR', '9.45', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '35', NULL),
('36000059', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 35 mm', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '35', NULL),
('36000060', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 42 mm', 'MT', '0.00', 'EUR', '12.95', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '42', NULL),
('36000060', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 42 mm', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '42', NULL),
('36000061', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 54 mm', 'MT', '0.00', 'EUR', '19.25', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '54', NULL),
('36000061', 'TUBO INOX AISI 316', 'TUBO INOX AISI 316 54 mm', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '4', NULL, NULL, NULL, NULL, NULL, NULL, '316', '54', NULL);


INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('36600101', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 10x1', 'MT', '0.00', 'EUR', '1.39', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '10', NULL),
('36600101', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 10x1', 'MT', '0.00', 'PTE', '278.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '10', NULL),
('36600121', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 12x1', 'MT', '0.00', 'EUR', '1.56', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '12', NULL),
('36600121', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 12x1', 'MT', '0.00', 'PTE', '312.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '12', NULL),
('36600141', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 14x1', 'MT', '0.00', 'EUR', '1.84', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '14', NULL),
('36600141', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 14x1', 'MT', '0.00', 'PTE', '368.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '14', NULL),
('36600151', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 15x1', 'MT', '0.00', 'EUR', '2.08', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '15', NULL),
('36600151', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 15x1', 'MT', '0.00', 'PTE', '416.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '15', NULL),
('36600181', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 18x1', 'MT', '0.00', 'EUR', '2.49', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '18', NULL),
('36600181', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 18x1', 'MT', '0.00', 'PTE', '500.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '18', NULL),
('36600221', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 22x1', 'MT', '0.00', 'EUR', '3.36', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '22', NULL),
('36600221', 'TUBO COBRE ROLO REV.', 'TUBO COBRE ROLO REV. 22x1', 'MT', '0.00', 'PTE', '674.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '5', NULL, NULL, NULL, NULL, NULL, NULL, '1', '22', NULL);


INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('37001206', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 12x6 mm', 'MT', '0.00', 'EUR', '0.18', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '12', NULL),
('37001206', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 12x6 mm', 'MT', '0.00', 'PTE', '37.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '12', NULL),
('37001306', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 13x6 mm', 'MT', '0.00', 'EUR', '0.18', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '13', NULL),
('37001306', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 13x6 mm', 'MT', '0.00', 'PTE', '37.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '13', NULL),
('37001406', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 14x6 mm', 'MT', '0.00', 'EUR', '0.21', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '14', NULL),
('37001406', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 14x6 mm', 'MT', '0.00', 'PTE', '43.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '14', NULL),
('37001506', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 15x6 mm', 'MT', '0.00', 'EUR', '0.21', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '15', NULL),
('37001506', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 15x6 mm', 'MT', '0.00', 'PTE', '43.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '15', NULL),
('37001606', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 16x6 mm', 'MT', '0.00', 'EUR', '0.21', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '16', NULL),
('37001606', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 16x6 mm', 'MT', '0.00', 'PTE', '43.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '16', NULL),
('37001806', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 18x6 mm', 'MT', '0.00', 'EUR', '0.22', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '18', NULL),
('37001806', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 18x6 mm', 'MT', '0.00', 'PTE', '60.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '18', NULL),
('37001906', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 19x6 mm', 'MT', '0.00', 'EUR', '0.22', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '19', NULL),
('37001906', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 19x6 mm', 'MT', '0.00', 'PTE', '60.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '19', NULL);



INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('37002206', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 22x6 mm', 'MT', '0.00', 'EUR', '0.27', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '22', NULL),
('37002206', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 22x6 mm', 'MT', '0.00', 'PTE', '68.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '22', NULL),
('37002306', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 23x6 mm', 'MT', '0.00', 'EUR', '0.27', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '23', NULL),
('37002306', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 23x6 mm', 'MT', '0.00', 'PTE', '68.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '23', NULL),
('37002706', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 27x6 mm', 'MT', '0.00', 'EUR', '0.34', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '27', NULL),
('37002706', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 27x6 mm', 'MT', '0.00', 'PTE', '89.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '27', NULL),
('37002906', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 29x6 mm', 'MT', '0.00', 'EUR', '0.34', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '29', NULL),
('37002906', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 29x6 mm', 'MT', '0.00', 'PTE', '89.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '29', NULL),
('37003406', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 34x6 mm', 'MT', '0.00', 'EUR', '0.57', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '34', NULL),
('37003406', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 34x6 mm', 'MT', '0.00', 'PTE', '115.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '34', NULL),
('37003606', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 36x6 mm', 'MT', '0.00', 'EUR', '0.57', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '36', NULL),
('37003606', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 36x6 mm', 'MT', '0.00', 'PTE', '115.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '36', NULL),
('37004306', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 43x6 mm', 'MT', '0.00', 'EUR', '0.75', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '43', NULL),
('37004306', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 43x6 mm', 'MT', '0.00', 'PTE', '140.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '6', '43', NULL),
('37011210', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 12x10 mm', 'MT', '0.00', 'EUR', '0.47', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '12', NULL);


INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('37011210', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 12x10 mm', 'MT', '0.00', 'PTE', '95.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '12', NULL),
('37011510', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 15x10 mm', 'MT', '0.00', 'EUR', '0.50', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '15', NULL),
('37011510', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 15x10 mm', 'MT', '0.00', 'PTE', '100.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '15', NULL),
('37011810', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 18x10 mm', 'MT', '0.00', 'EUR', '0.54', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '18', NULL),
('37011810', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 18x10 mm', 'MT', '0.00', 'PTE', '108.50', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '18', NULL),
('37012210', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 22x10 mm', 'MT', '0.00', 'EUR', '0.65', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '22', NULL),
('37012210', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 22x10 mm', 'MT', '0.00', 'PTE', '130.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '22', NULL),
('37012710', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 27x10 mm', 'MT', '0.00', 'EUR', '0.75', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '27', NULL),
('37012710', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 27x10 mm', 'MT', '0.00', 'PTE', '150.50', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '27', NULL),
('37013410', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 34x10 mm', 'MT', '0.00', 'EUR', '0.94', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '34', NULL),
('37013410', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 34x10 mm', 'MT', '0.00', 'PTE', '188.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '34', NULL),
('37014310', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 43x10 mm', 'MT', '0.00', 'EUR', '1.19', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '43', NULL),
('37014310', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 43x10 mm', 'MT', '0.00', 'PTE', '238.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '43', NULL),
('37014910', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 49x10 mm', 'MT', '0.00', 'EUR', '1.64', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '49', NULL),
('37014910', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 49x10 mm', 'MT', '0.00', 'PTE', '329.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '49', NULL);




INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('37015410', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 54x10 mm', 'MT', '0.00', 'EUR', '1.88', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '54', NULL),
('37015410', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 54x10 mm', 'MT', '0.00', 'PTE', '377.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '54', NULL),
('37016110', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 61x10 mm', 'MT', '0.00', 'EUR', '2.06', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '61', NULL),
('37016110', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 61x10 mm', 'MT', '0.00', 'PTE', '412.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '61', NULL),
('37017710', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 77x10 mm', 'MT', '0.00', 'EUR', '2.38', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '77', NULL),
('37017710', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 77x10 mm', 'MT', '0.00', 'PTE', '478.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '77', NULL),
('37019110', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 91x10 mm', 'MT', '0.00', 'EUR', '3.26', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '91', NULL),
('37019110', 'TUBO ISOLAM. REVEST.', 'TUBO ISOLAM. REVEST. 91x10 mm', 'MT', '0.00', 'PTE', '653.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, '4', NULL, NULL, NULL, '10', '91', NULL);




INSERT INTO t_product_catalog (product_code, description, description_full, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('40000021', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 1/2"', 'MT', '0.00', 'EUR', '0.53', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '12.7', NULL),
('40000021', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 1/2"', 'MT', '0.00', 'PTE', '183.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '12.7', NULL),
('40000026', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 3/4"', 'MT', '0.00', 'EUR', '0.74', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '19.05', NULL),
('40000026', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 3/4"', 'MT', '0.00', 'PTE', '264.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '19.05', NULL),
('40000033', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 1"', 'MT', '0.00', 'EUR', '1.19', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '25.4', NULL),
('40000033', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 1"', 'MT', '0.00', 'PTE', '416.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '25.4', NULL),
('40000042', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 1 1/4"', 'MT', '0.00', 'EUR', '1.71', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '31.75', NULL),
('40000042', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 1 1/4"', 'MT', '0.00', 'PTE', '588.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '31.75', NULL),
('40000048', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 1 1/2"', 'MT', '0.00', 'EUR', '2.08', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '38.1', NULL),
('40000048', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 1 1/2"', 'MT', '0.00', 'PTE', '735.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '38.1', NULL),
('40000060', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 2"', 'MT', '0.00', 'EUR', '3.02', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50.8', NULL),
('40000060', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 2"', 'MT', '0.00', 'PTE', '1083.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50.8', NULL),
('40000075', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 2 1/2"', 'MT', '0.00', 'EUR', '3.77', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '63.5', NULL),
('40000075', 'TUBO PVC RIGIDO', 'TUBO PVC RIGIDO 2 1/2"', 'MT', '0.00', 'PTE', '1259.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '63.5', NULL);



INSERT INTO t_product_catalog (product_code, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('40000100', 'TUBO PVC P/ FUROS', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '6.0', '140', NULL),
('41000032', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '0.44', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '3.2', '32', NULL),
('41000032', 'TUBO PVC DIN', 'MT', '0.00', 'PTE', '182.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '3.2', '32', NULL),
('41000040', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '0.57', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '3.5', '40', NULL),
('41000040', 'TUBO PVC DIN', 'MT', '0.00', 'PTE', '224.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '3.5', '40', NULL),
('41000050', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '0.73', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '3.8', '50', NULL),
('41000050', 'TUBO PVC DIN', 'MT', '0.00', 'PTE', '284.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '3.8', '50', NULL),
('41000063', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '0.94', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '4.0', '63', NULL),
('41000063', 'TUBO PVC DIN', 'MT', '0.00', 'PTE', '340.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '4.0', '63', NULL),
('41000075', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '1.10', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '4.5', '75', NULL),
('41000075', 'TUBO PVC DIN', 'MT', '0.00', 'PTE', '431.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '4.5', '75', NULL),
('41000090', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '1.29', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '4.8', '90', NULL),
('41000090', 'TUBO PVC DIN', 'MT', '0.00', 'PTE', '519.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '4.8', '90', NULL),
('41000110', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '1.93', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '5.5', '110', NULL),
('41000110', 'TUBO PVC DIN', 'MT', '0.00', 'PTE', '780.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '5.5', '110', NULL),
('41000125', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '2.39', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '6.0', '125', NULL),
('41000125', 'TUBO PVC DIN', 'MT', '0.00', 'PTE', '995.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, '6.0', '125', NULL);


UPDATE t_product_catalog
SET description = CASE product_code
    WHEN '40000021' THEN 'TUBO PVC RIGIDO 1/2"'
    WHEN '40000026' THEN 'TUBO PVC RIGIDO 3/4"'
    WHEN '40000033' THEN 'TUBO PVC RIGIDO 1"'
    WHEN '40000042' THEN 'TUBO PVC RIGIDO 1 1/4"'
    WHEN '40000048' THEN 'TUBO PVC RIGIDO 1 1/2"'
    WHEN '40000060' THEN 'TUBO PVC RIGIDO 2"'
    WHEN '40000075' THEN 'TUBO PVC RIGIDO 2 1/2"'
    WHEN '40000100' THEN 'TUBO PVC P/ FUROS 140x6 Kg'
    WHEN '41000032' THEN 'TUBO PVC DIN 32mm'
    WHEN '41000040' THEN 'TUBO PVC DIN 40mm'
    WHEN '41000050' THEN 'TUBO PVC DIN 50mm'
    WHEN '41000063' THEN 'TUBO PVC DIN 63mm'
    WHEN '41000075' THEN 'TUBO PVC DIN 75mm'
    WHEN '41000090' THEN 'TUBO PVC DIN 90mm'
    WHEN '41000110' THEN 'TUBO PVC DIN 110mm'
    WHEN '41000125' THEN 'TUBO PVC DIN 125mm'
    ELSE description -- Keeps current value if no match
END
WHERE product_code IN ('40000021', '40000026', '40000033', '40000042', '40000048', '40000060', '40000075', '40000100', '41000032', '41000040', '41000050', '41000063', '41000075', '41000090', '41000110', '41000125');




INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter)
VALUES
('41000140', 'TUBO PVC DIN           140', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '2.99', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '140'),
('41000160', 'TUBO PVC DIN           160', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '3.94', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '160'),
('41000200', 'TUBO PVC DIN           200', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '6.02', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '200'),
('41000250', 'TUBO PVC DIN           250', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '8.69', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '250'),
('41000315', 'TUBO PVC DIN           315', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '15.24', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '315'),
('41000400', 'TUBO PVC DIN           400', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '24.89', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '400'),
('41000500', 'TUBO PVC DIN           500', 'TUBO PVC DIN', 'MT', '0.00', 'EUR', '52.52', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '500');


	
INSERT INTO t_product_catalog 
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter)
VALUES
('42000032', 'TUBO PVC UNI           32', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '0.39', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '32'),
('42000032', 'TUBO PVC UNI           32', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '130.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '32'),
('42000040', 'TUBO PVC UNI           40', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '0.48', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '40'),
('42000040', 'TUBO PVC UNI           40', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '162.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '40'),
('42000050', 'TUBO PVC UNI           50', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '0.61', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50'),
('42000050', 'TUBO PVC UNI           50', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '206.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '50'),
('42000063', 'TUBO PVC UNI           63', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '0.77', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '63'),
('42000063', 'TUBO PVC UNI           63', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '317.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '63'),
('42000075', 'TUBO PVC UNI           75', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '0.95', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '75'),
('42000075', 'TUBO PVC UNI           75', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '343.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '75'),
('42000090', 'TUBO PVC UNI           90', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '1.19', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '90'),
('42000090', 'TUBO PVC UNI           90', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '413.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '90'),
('42000110', 'TUBO PVC UNI           110', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '1.65', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '110'),
('42000110', 'TUBO PVC UNI           110', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '578.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '110'),
('42000125', 'TUBO PVC UNI           125', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '2.03', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '125'),
('42000125', 'TUBO PVC UNI           125', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '739.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '125');



INSERT INTO t_product_catalog 
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter)
VALUES
('42000140', 'TUBO PVC UNI           140', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '2.47', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '140'),
('42000140', 'TUBO PVC UNI           140', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '1162.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '140'),
('42000160', 'TUBO PVC UNI           160', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '3.42', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '160'),
('42000160', 'TUBO PVC UNI           160', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '1522.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '160'),
('42000200', 'TUBO PVC UNI           200', 'TUBO PVC UNI', 'MT', '0.00', 'EUR', '4.39', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '200'),
('42000200', 'TUBO PVC UNI           200', 'TUBO PVC UNI', 'MT', '0.00', 'PTE', '2335.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '200'),
('42700060', 'ORINGS P/TUBO 90x10KG', 'ORINGS P/TUBO', 'UN', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL,'10', '90');




INSERT INTO t_product_catalog 
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('43106306', 'TUBO PVC PRESS. 63 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '1.20', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '6', '63', NULL),
('43106306', 'TUBO PVC PRESS. 63 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '342.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '6', '63', NULL),
('43106310', 'TUBO PVC PRESS. 63 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '1.97', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '10', '63', NULL),
('43106310', 'TUBO PVC PRESS. 63 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '521.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '10', '63', NULL),
('43106316', 'TUBO PVC PRESS. 63 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '2.99', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '16', '63', NULL),
('43106316', 'TUBO PVC PRESS. 63 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '882.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '16', '63', NULL),
('43107506', 'TUBO PVC PRESS. 75 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '1.64', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '6', '75', NULL),
('43107506', 'TUBO PVC PRESS. 75 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '476.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '6', '75', NULL),
('43107510', 'TUBO PVC PRESS. 75 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '2.80', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '10', '75', NULL),
('43107510', 'TUBO PVC PRESS. 75 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '744.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '10', '75', NULL),
('43107516', 'TUBO PVC PRESS. 75 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '4.24', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '16', '75', NULL),
('43107516', 'TUBO PVC PRESS. 75 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '1244.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '16', '75', NULL),
('43109006', 'TUBO PVC PRESS. 90 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '2.40', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '6', '90', NULL),
('43109006', 'TUBO PVC PRESS. 90 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '689.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '6', '90', NULL);






INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('43109010', 'TUBO PVC PRESS. 90 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '4.02', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '10', '90', NULL),
('43109010', 'TUBO PVC PRESS. 90 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '1067.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '10', '90', NULL),
('43109016', 'TUBO PVC PRESS. 90 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '6.08', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '16', '90', NULL),
('43109016', 'TUBO PVC PRESS. 90 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '1786.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '16', '90', NULL),
('43111006', 'TUBO PVC PRESS. 110 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '2.84', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '6', '110', NULL),
('43111006', 'TUBO PVC PRESS. 110 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '1000.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '6', '110', NULL),
('43111010', 'TUBO PVC PRESS. 110 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '4.84', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '10', '110', NULL),
('43111010', 'TUBO PVC PRESS. 110 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '1591.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL,NULL, NULL, NULL, NULL, '10', '110', NULL),
('43111016', 'TUBO PVC PRESS. 110 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '7.44', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '16', '110', NULL),
('43111016', 'TUBO PVC PRESS. 110 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '2668.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '16', '110', NULL),
('43112506', 'TUBO PVC PRESS. 125 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '3.71', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '6', '125', NULL),
('43112506', 'TUBO PVC PRESS. 125 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '1298.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL,NULL, NULL, NULL, NULL, NULL, '6', '125', NULL),
('43112510', 'TUBO PVC PRESS. 125 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '6.28', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL,NULL, NULL, NULL, NULL, '10', '125', NULL);


INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('43112510', 'TUBO PVC PRESS. 125 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '2036.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL,'10', '125', NULL),
('43112516', 'TUBO PVC PRESS. 125 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '9.49', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,NULL, '16', '125', NULL),
('43112516', 'TUBO PVC PRESS. 125 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '3427.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,NULL, '16', '125', NULL),
('43114006', 'TUBO PVC PRESS. 140 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '4.69', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,NULL, '6', '140', NULL),
('43114006', 'TUBO PVC PRESS. 140 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '1615.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL,'6', '140', NULL),
('43114010', 'TUBO PVC PRESS. 140 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '7.93', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,NULL, '10', '140', NULL),
('43114010', 'TUBO PVC PRESS. 140 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '2548.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL,'10', '140', NULL),
('43114016', 'TUBO PVC PRESS. 140 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '11.95', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,NULL, '16', '140', NULL),
('43114016', 'TUBO PVC PRESS. 140 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '4289.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL,'16', '140', NULL),
('43116006', 'TUBO PVC PRESS. 160 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '6.13', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL,'6', '160', NULL),
('43116006', 'TUBO PVC PRESS. 160 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '2098.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL,'6', '160', NULL),
('43116010', 'TUBO PVC PRESS. 160 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '10.40', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,NULL, '10', '160', NULL),
('43116010', 'TUBO PVC PRESS. 160 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '3335.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL,'10', '160', NULL),
('43116016', 'TUBO PVC PRESS. 160 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '15.66', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL, NULL,'16', '160', NULL);


INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('43116016', 'TUBO PVC PRESS. 160 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '5588.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,  NULL, '16', '160', NULL),
('43120006', 'TUBO PVC PRESS. 200 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '9.34', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,  NULL, '6', '200', NULL),
('43120006', 'TUBO PVC PRESS. 200 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '3274.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '6', '200', NULL),
('43120010', 'TUBO PVC PRESS. 200 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '16.17', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '10', '200', NULL),
('43120010', 'TUBO PVC PRESS. 200 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '5188.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '10', '200', NULL),
('43120016', 'TUBO PVC PRESS. 200 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '24.41', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '16', '200', NULL),
('43120016', 'TUBO PVC PRESS. 200 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '8755.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '16', '200', NULL),
('43125006', 'TUBO PVC PRESS. 250 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '14.83', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '6', '250', NULL),
('43125006', 'TUBO PVC PRESS. 250 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '5064.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '6', '250', NULL),
('43125010', 'TUBO PVC PRESS. 250 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '25.13', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,  NULL, '10', '250', NULL),
('43125010', 'TUBO PVC PRESS. 250 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '8047.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '10', '250', NULL),
('43125016', 'TUBO PVC PRESS. 250 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '37.93', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '16', '250', NULL),
('43125016', 'TUBO PVC PRESS. 250 J16', 'TUBO PVC PRESS', 'MT', '0.00', 'PTE', '13612.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '16', '250', NULL),
('43131510', 'TUBO PVC PRESS. 315 J10', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '39.96', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,  NULL, '10', '315', NULL),
('43140006', 'TUBO PVC PRESS. 400 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '37.49', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL,  NULL, NULL, '6', '400', NULL),
('43150006', 'TUBO PVC PRESS. 500 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '68.08', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,  NULL, '6', '500', NULL),
('43163006', 'TUBO PVC PRESS. 630 J6', 'TUBO PVC PRESS', 'MT', '0.00', 'EUR', '111.47', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', NULL, NULL, NULL, NULL, NULL,  NULL, '6', '630', NULL);





INSERT INTO mf_product_finishing VALUES (11, "Corrugado");


INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('43300063', 'TUBO CORRUGADO TPC 63', 'TUBO CORRUGADO TPC', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, 11, NULL, NULL, NULL, NULL, NULL, '63', NULL),
('43300063', 'TUBO CORRUGADO TPC 63', 'TUBO CORRUGADO TPC', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, 11, NULL, NULL, NULL, NULL, NULL, '63', NULL),
('43300075', 'TUBO CORRUGADO TPC 75', 'TUBO CORRUGADO TPC', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, 11, NULL, NULL, NULL, NULL, NULL, '75', NULL),
('43300075', 'TUBO CORRUGADO TPC 75', 'TUBO CORRUGADO TPC', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, 11, NULL, NULL, NULL, NULL, NULL, '75', NULL),
('43300090', 'TUBO CORRUGADO TPC 90', 'TUBO CORRUGADO TPC', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, 11, NULL, NULL, NULL, NULL, NULL, '90', NULL),
('43300090', 'TUBO CORRUGADO TPC 90', 'TUBO CORRUGADO TPC', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, 11, NULL, NULL, NULL, NULL, NULL, '90', NULL),
('43300110', 'TUBO CORRUGADO TPC 110', 'TUBO CORRUGADO TPC', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, 11, NULL, NULL, NULL, NULL, NULL, '110', NULL),
('43300110', 'TUBO CORRUGADO TPC 110', 'TUBO CORRUGADO TPC', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, 11, NULL, NULL, NULL, NULL, NULL, '110', NULL),
('43300160', 'TUBO CORRUGADO TPC 160', 'TUBO CORRUGADO TPC', 'MT', '0.00', 'EUR', '4.49', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, 11, NULL, NULL, NULL, NULL, NULL, '160', NULL),
('43300160', 'TUBO CORRUGADO TPC 160', 'TUBO CORRUGADO TPC', 'MT', '0.00', 'PTE', '900.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, 11, NULL, NULL, NULL, NULL, NULL, '160', NULL);



INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('43400090', 'TUBO DRENAGEM ABOB. 90', 'TUBO DRENAGEM ABOB.', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '90', NULL),
('43400090', 'TUBO DRENAGEM ABOB. 90', 'TUBO DRENAGEM ABOB.', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '90', NULL),
('43400110', 'TUBO DRENAGEM ABOB. 110', 'TUBO DRENAGEM ABOB.', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '110', NULL),
('43400110', 'TUBO DRENAGEM ABOB. 110', 'TUBO DRENAGEM ABOB.', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '110', NULL),
('43400160', 'TUBO DRENAGEM ABOB. 160', 'TUBO DRENAGEM ABOB.', 'MT', '0.00', 'EUR', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '160', NULL),
('43400160', 'TUBO DRENAGEM ABOB. 160', 'TUBO DRENAGEM ABOB.', 'MT', '0.00', 'PTE', '0.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '160', NULL);


INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('44000332', 'TUBO POLIETILENE  1x2', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.28', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '1', NULL, NULL),
('44000332', 'TUBO POLIETILENE  1x2', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '55.50', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '1', NULL, NULL),
('44000422', 'TUBO POLIETILENE  1 1/4x2', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.33', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '1.25', NULL, NULL),
('44000422', 'TUBO POLIETILENE  1 1/4x2', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '67.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '1.25', NULL, NULL),
('44000482', 'TUBO POLIETILENE  1 1/2x2', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.46', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '1.5', NULL, NULL),
('44000482', 'TUBO POLIETILENE  1 1/2x2', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '93.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '1.5', NULL, NULL),
('44000602', 'TUBO POLIETILENE  2x2', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.58', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '2', NULL, NULL),
('44000602', 'TUBO POLIETILENE  2x2', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '116.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '2', NULL, NULL),
('44000752', 'TUBO POLIETILENE  2 1/2x2', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.85', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '2.5', NULL, NULL),
('44000752', 'TUBO POLIETILENE  2 1/2x2', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '170.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '2.5', NULL, NULL),
('44000892', 'TUBO POLIETILENE  3x2', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.85', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '3', NULL, NULL),
('44000892', 'TUBO POLIETILENE  3x2', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '171.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '3', NULL, NULL),
('44001152', 'TUBO POLIETILENE  4x2', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '1.03', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '4', NULL, NULL),
('44001152', 'TUBO POLIETILENE  4x2', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '207.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '4', NULL, NULL),
('44001402', 'TUBO POLIETILENE  5x2', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '1.61', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '5', NULL, NULL),
('44001402', 'TUBO POLIETILENE  5x2', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '323.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, '5', NULL, NULL);


INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('44001662', 'TUBO POLIETILENE  6x2', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '2.20', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '6', NULL),
('44001662', 'TUBO POLIETILENE  6x2', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '440.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '6', NULL),
('44010264', 'TUBO POLIETILENE  3/4x4', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.21', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0.75', NULL),
('44010264', 'TUBO POLIETILENE  3/4x4', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '41.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0.75', NULL),
('44010334', 'TUBO POLIETILENE  1x4', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.30', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', NULL),
('44010334', 'TUBO POLIETILENE  1x4', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '60.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', NULL),
('44010424', 'TUBO POLIETILENE  1 1/4x4', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.38', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1.25', NULL),
('44010424', 'TUBO POLIETILENE  1 1/4x4', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '77.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1.25', NULL),
('44010484', 'TUBO POLIETILENE  1 1/2x4', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.54', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1.5', NULL),
('44010484', 'TUBO POLIETILENE  1 1/2x4', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '109.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1.5', NULL),
('44010604', 'TUBO POLIETILENE  2x4', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.79', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2', NULL),
('44010604', 'TUBO POLIETILENE  2x4', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '159.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2', NULL),
('44010754', 'TUBO POLIETILENE  2 1/2x4', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '1.18', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2.5', NULL),
('44010754', 'TUBO POLIETILENE  2 1/2x4', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '236.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2.5', NULL),
('44010894', 'TUBO POLIETILENE  3x4', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '1.50', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '3', NULL);





INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('44010894', 'TUBO POLIETILENE  3x4', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '301.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '3', NULL),
('44011154', 'TUBO POLIETILENE  4x4', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '2.28', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '4', NULL),
('44011154', 'TUBO POLIETILENE  4x4', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '457.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '4', NULL),
('44011404', 'TUBO POLIETILENE  5x4', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '2.89', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '5', NULL),
('44011404', 'TUBO POLIETILENE  5x4', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '579.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '5', NULL),
('44011604', 'TUBO POLIETILENE  6x4', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '3.89', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '6', NULL),
('44011604', 'TUBO POLIETILENE  6x4', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '779.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '6', NULL),
('44020218', 'TUBO POLIETILENE  1/2x8', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.14', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0.5', NULL),
('44020218', 'TUBO POLIETILENE  1/2x8', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '28.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0.5', NULL),
('44020228', 'TUBO POLIETILENE  5/8x8', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.17', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0.625', NULL),
('44020228', 'TUBO POLIETILENE  5/8x8', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '34.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0.625', NULL),
('44020268', 'TUBO POLIETILENE  3/4x8', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.25', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0.75', NULL),
('44020268', 'TUBO POLIETILENE  3/4x8', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '51.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0.75', NULL),
('44020338', 'TUBO POLIETILENE  1x8', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.39', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', NULL),
('44020338', 'TUBO POLIETILENE  1x8', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '79.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', NULL);



INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('44020428', 'TUBO POLIETILENE  1 1/4x8', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.64', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1.25, '1.25'),
('44020428', 'TUBO POLIETILENE  1 1/4x8', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '128.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1.25, '1.25'),
('44020488', 'TUBO POLIETILENE  1 1/2x8', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '0.85', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1.5, '1.5'),
('44020488', 'TUBO POLIETILENE  1 1/2x8', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '171.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1.5, '1.5'),
('44020608', 'TUBO POLIETILENE  2x8', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '1.21', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, '2'),
('44020608', 'TUBO POLIETILENE  2x8', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '242.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, '2'),
('44020758', 'TUBO POLIETILENE  2 1/2x8', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '1.91', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2.5, '2.5'),
('44020758', 'TUBO POLIETILENE  2 1/2x8', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '382.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2.5, '2.5'),
('44020898', 'TUBO POLIETILENE  3x8', 'TUBO POLIETILENE', 'MT', '0.00', 'EUR', '2.58', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3, '3'),
('44020898', 'TUBO POLIETILENE  3x8', 'TUBO POLIETILENE', 'MT', '0.00', 'PTE', '517.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3, '3'),
('46000017', 'TUBO RECTANG.PVC    80x40', 'TUBO RECTANG.PVC', 'MT', '0.00', 'EUR', '2.36', '0.00', '0.00', '1900-01-01', '1900-01-01', '62', NULL, '6', '6', '1', NULL, NULL, NULL, '80', '40', NULL, NULL, NULL);




-- MAIS CHAPAS



INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('68902445', 'CH.CRESC.RIDGID   31190  150 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '15.10', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 150, '150'),
('68902445', 'CH.CRESC.RIDGID   31190  150 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '2715.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 150, '150'),
('68902453', 'CH.CRESC.RIDGID   31195  200 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '16.90', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 200, '200'),
('68902453', 'CH.CRESC.RIDGID   31195  200 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '3055.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 200, '200'),
('68902461', 'CH.CRESC.RIDGID   31200  250 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '21.70', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 250, '250'),
('68902461', 'CH.CRESC.RIDGID   31200  250 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '3915.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 250, '250'),
('68902488', 'CH.CRESC.RIDGID   31205  300 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '31.30', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 300, '300'),
('68902488', 'CH.CRESC.RIDGID   31205  300 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '5665.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 300, '300'),
('68902496', 'CH.CRESC.RIDGID   31210  375 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '49.30', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 375, '375'),
('68902496', 'CH.CRESC.RIDGID   31210  375 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '8725.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 375, '375'),
('68902518', 'CH.CRESC.RIDGID   31215  450 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '84.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 450, '450'),
('68902518', 'CH.CRESC.RIDGID   31215  450 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '14665.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 450, '450'),
('68902526', 'CH.CRESC.RIDGID   31220  600 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '133.40', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 600, '600'),
('68902526', 'CH.CRESC.RIDGID   31220  600 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '24450.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 600, '600'),
('68902542', 'CH.CRESC.RIDGID   31230  150 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '10.58', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 150, '150');


INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('68902542', 'CH.CRESC.RIDGID   31230  150 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '2120.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 150, '150'),
('68902550', 'CH.CRESC.RIDGID   31235  200 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '13.19', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 200, '200'),
('68902550', 'CH.CRESC.RIDGID   31235  200 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '2645.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 200, '200'),
('68902569', 'CH.CRESC.RIDGID   31240  250 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '15.46', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 250, '250'),
('68902569', 'CH.CRESC.RIDGID   31240  250 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '3100.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 250, '250'),
('68902577', 'CH.CRESC.RIDGID   31245  300 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '23.52', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 300, '300'),
('68902577', 'CH.CRESC.RIDGID   31245  300 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '4715.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 300, '300'),
('68902585', 'CH.CRESC.RIDGID   31250  375 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '38.76', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 375, '375'),
('68902585', 'CH.CRESC.RIDGID   31250  375 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '7770.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 375, '375'),
('68902593', 'CH.CRESC.RIDGID   31255  450 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'EUR', '56.66', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 450, '450'),
('68902593', 'CH.CRESC.RIDGID   31255  450 CR', 'CHAVE CRESC.RIDGID', 'UN', '0.00', 'PTE', '11360.00', '0.00', '0.00', '1900-01-01', '1900-01-01', '6', NULL, '5', '1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 450, '450');



INSERT INTO t_product_catalog
(id, product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
(13375, '76815112', 'CHAPA OND FIBRA 1500x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 1500, 1120, NULL, NULL, NULL, NULL),
(13376, '76815112', 'CHAPA OND FIBRA 1500x1120', 'CHAPA ONDULADA FIBRA', 'UN', '0.00', 'EUR', '5.99', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 1500, 1120, NULL, NULL, NULL, NULL),
(13377, '76818112', 'CHAPA OND FIBRA 1830x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 1830, 1120, NULL, NULL, NULL, NULL),
(13378, '76818112', 'CHAPA OND FIBRA 1830x1120', 'CHAPA ONDULADA FIBRA', 'UN', '0.00', 'EUR', '5.99', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 1830, 1120, NULL, NULL, NULL, NULL),
(13379, '76820112', 'CHAPA OND FIBRA 2000x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '4.25', '4.25', '2017-10-10', '2018-12-28', 62, NULL, 5, 1, NULL, NULL, 3, 2000, 1120, NULL, NULL, NULL, NULL),
(13380, '76820112', 'CHAPA OND FIBRA 2000x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '4.25', '4.25', '2017-10-10', '2018-12-28', 62, NULL, 5, 1, NULL, NULL, 3, 2000, 1120, NULL, NULL, NULL, NULL),
(13381, '76825112', 'CHAPA OND FIBRA 2500x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '4.24', '4.24', '2017-10-10', '2020-03-10', 62, NULL, 5, 1, NULL, NULL, 3, 2500, 1120, NULL, NULL, NULL, NULL),
(13382, '76825112', 'CHAPA OND FIBRA 2500x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '4.24', '4.24', '2017-10-10', '2020-03-10', 62, NULL, 5, 1, NULL, NULL, 3, 2500, 1120, NULL, NULL, NULL, NULL),
(13383, '76830112', 'CHAPA OND FIBRA 3000x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '4.25', '4.25', '2017-10-10', '2020-03-10', 62, NULL, 5, 1, NULL, NULL, 3, 3000, 1120, NULL, NULL, NULL, NULL),
(13384, '76830112', 'CHAPA OND FIBRA 3000x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '4.25', '4.25', '2017-10-10', '2020-03-10', 62, NULL, 5, 1, NULL, NULL, 3, 3000, 1120, NULL, NULL, NULL, NULL),
(13385, '76835106', 'CHAPA OND FIBRA 3500x1060', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 3500, 1060, NULL, NULL, NULL, NULL),
(13386, '76835106', 'CHAPA OND FIBRA 3500x1060', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 3500, 1060, NULL, NULL, NULL, NULL),
(13387, '76835112', 'CHAPA OND FIBRA 3500x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '4.45', '4.45', '2017-10-10', '2020-03-10', 62, NULL, 5, 1, NULL, NULL, 3, 3500, 1120, NULL, NULL, NULL, NULL),
(13388, '76835112', 'CHAPA OND FIBRA 3500x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '4.45', '4.45', '2017-10-10', '2020-03-10', 62, NULL, 5, 1, NULL, NULL, 3, 3500, 1120, NULL, NULL, NULL, NULL),
(13389, '76840112', 'CHAPA OND FIBRA 4000x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '4.25', '4.25', '2020-03-10', '2020-03-10', 62, NULL, 5, 1, NULL, NULL, 3, 4000, 1120, NULL, NULL, NULL, NULL),
(13390, '76840112', 'CHAPA OND FIBRA 4000x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '4.25', '4.25', '2020-03-10', '2020-03-10', 62, NULL, 5, 1, NULL, NULL, 3, 4000, 1120, NULL, NULL, NULL, NULL);


INSERT INTO t_product_catalog
(id, product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
(13393, '76842000', 'CHAPA FIBRA LISA 3000x1000', 'CHAPA FIBRA LISA', 'M2', '0.00', 'EUR', '6.38', '4.50', '4.50', '2017-10-10', '2018-12-28', 62, NULL, 5, 1, NULL, NULL, 1, 3000, 1000, NULL, NULL, NULL, NULL),
(13394, '76845112', 'CHAPA OND FIBRA 4500x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 4500, 1120, NULL, NULL, NULL, NULL),
(13395, '76850112', 'CHAPA OND FIBRA 5000X1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 5000, 1120, NULL, NULL, NULL, NULL),
(13396, '76850112', 'CHAPA OND FIBRA 5000X1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 5000, 1120, NULL, NULL, NULL, NULL),
(13397, '76855112', 'CHAPA OND FIBRA 5500X1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 5500, 1120, NULL, NULL, NULL, NULL),
(13398, '76855112', 'CHAPA OND FIBRA 5500X1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 5500, 1120, NULL, NULL, NULL, NULL),
(13399, '76860112', 'CHAPA OND FIBRA 6000x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 6000, 1120, NULL, NULL, NULL, NULL),
(13400, '76860112', 'CHAPA OND FIBRA 6000x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 6000, 1120, NULL, NULL, NULL, NULL),
(13401, '76870112', 'CHAPA OND FIBRA 7000x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 7000, 1120, NULL, NULL, NULL, NULL),
(13402, '76870112', 'CHAPA OND FIBRA 7000x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 7000, 1120, NULL, NULL, NULL, NULL),
(13403, '76875112', 'CHAPA OND FIBRA 7550x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'EUR', '6.38', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 7550, 1120, NULL, NULL, NULL, NULL),
(13404, '76875112', 'CHAPA OND FIBRA 7550x1120', 'CHAPA ONDULADA FIBRA', 'M2', '0.00', 'PTE', '1200.00', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 7550, 1120, NULL, NULL, NULL, NULL),
(13405, '76885112', 'CHAPA OND CRISTAL 5000x1120', 'CHAPA ONDULADA CRISTAL', 'M2', '0.00', 'EUR', '11.35', '0.00', '0.00', '1900-01-01', '1900-01-01', 62, NULL, 5, 1, NULL, NULL, 3, 5000, 1120, NULL, NULL, NULL, NULL);



-- CALHAS

INSERT INTO mf_product_shape VALUES (22, "Calha");

INSERT INTO t_product_catalog
(product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
('18315115', 'CALHA OMEGA 20mm GALV. 0,80 mm', 'CALHA OMEGA GALV.', 'MT', 0.00, 'EUR', 0.57, 0.00, 0.00, '1900-01-01', '1900-01-01', 62, NULL, 6, 1, 22, 2, NULL, 6000, 20, NULL, 0.80, NULL, NULL),
('24302026', 'CALHA U 30x20x2,6', 'CALHA U', 'KG', 0.00, 'EUR', 2.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 26, NULL, 6, 1, 22, NULL, NULL, 6000, 30, 20, 2.6, NULL, NULL),
('24502530', 'CALHA U 50x25x3,2', 'CALHA U', 'KG', 1336.00, 'EUR', 2.03, 0.76, 0.76, '2024-05-02', '2024-05-24', 26, NULL, 6, 1, 22, NULL, NULL, 6000, 50, 25, 3.2, NULL, NULL),
('24603032', 'CALHA U 60x30x3,2', 'CALHA U', 'KG', 1200.00, 'EUR', 2.03, 1.16, 1.16, '2022-04-06', '2024-03-08', 26, NULL, 6, 1, 22, NULL, NULL, 6000, 60, 30, 3.2, NULL, NULL),
('24703532', 'CALHA U 70x35x3,2', 'CALHA U', 'KG', 2526.00, 'EUR', 2.03, 0.82, 0.76, '2024-05-02', '2024-05-03', 26, NULL, 6, 1, 22, NULL, NULL, 6000, 70, 35, 3.2, NULL, NULL),
('24801010', 'CALHA U 80x10x10x3', 'CALHA U', 'KG', 0.00, 'EUR', 0.00, 0.00, 0.00, '1900-01-01', '1900-01-01', 26, NULL, 6, 1, 22, NULL, NULL, 6000, 80, 10, 3.0, NULL, NULL),
('24804032', 'CALHA U 80x40x3,2', 'CALHA U', 'KG', 2418.00, 'EUR', 2.03, 0.77, 0.76, '2024-05-02', '2024-05-09', 26, NULL, 6, 1, 22, NULL, NULL, 6000, 80, 40, 3.2, NULL, NULL),
('24804040', 'CALHA U 80x40x4', 'CALHA U', 'KG', 0.00, 'EUR', 2.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 26, NULL, 6, 1, 22, NULL, NULL, 6000, 80, 40, 4.0, NULL, NULL),
('24904532', 'CALHA U 90x45x3,2', 'CALHA U', 'KG', 0.00, 'EUR', 2.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 26, NULL, 6, 1, 22, NULL, NULL, 6000, 90, 45, 3.2, NULL, NULL),
('24915032', 'CALHA U 100x50x3,2', 'CALHA U', 'KG', 2044.00, 'EUR', 2.03, 0.85, 0.85, '2023-09-29', '2024-04-18', 26, NULL, 6, 1, 22, NULL, NULL, 6000, 100, 50, 3.2, NULL, NULL),
('24925032', 'CALHA U 150x50x3,2', 'CALHA U', 'KG', 0.00, 'EUR', 2.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 26, NULL, 6, 1, 22, NULL, NULL, 6000, 150, 50, 3.2, NULL, NULL),
('24935032', 'CALHA U C/ABAS 20x50x150x3', 'CALHA U C/ABAS', 'KG', 0.00, 'EUR', 2.19, 0.00, 0.00, '1900-01-01', '1900-01-01', 26, NULL, 6, 1, 22, NULL, 12, 6000, 50, 20, 3.0, NULL, NULL),
('24936030', 'CALHA U C/ABAS 15x30x60x3', 'CALHA U C/ABAS', 'KG', 0.00, 'EUR', 2.19, 0.00, 0.00, '1900-01-01', '1900-01-01', 26, NULL, 6, 1, 22, NULL, 12, 6000, 30, 15, 3.0, NULL, NULL);


INSERT INTO t_product_catalog
(id, product_code, description_full, description, unit, stock_current, currency, price_pvp, price_avg, price_last, date_last_entry, date_last_exit, family_id, price_ref_market, type_id, material_id, shape_id, finishing_id, surface_id, length, width, height, thickness, diameter, nominal_dimension)
VALUES
(3623, '24991402', 'CALHA PERFIL Z 140x52x18x2', 'CALHA PERFIL Z', 'KG', 0.00, 'EUR', 3.03, 0.00, 0.00, '1900-01-01', '1900-01-01', 	 26, NULL, 6, 1, 22, NULL, NULL, 6000, 140, 52, 2.0, NULL, NULL),
(3624, '24991502', 'CALHA PERFIL Z 170x50x20x2', 'CALHA PERFIL Z', 'KG', 0.00, 'EUR', 3.03, 4.75, 4.75, '2023-03-13', '2023-04-28', 	 26, NULL, 6, 1, 22, NULL, NULL, 6000, 170, 50, 2.0, NULL, NULL),
(3625, '24992002', 'CALHA PERFIL Z200 GALV. 2mm', 'CALHA PERFIL Z', 'KG', 0.00, 'EUR', 4.64, 0.00, 0.00, '1900-01-01', '1900-01-01',	 26, NULL, 6, 1, 22, 2, NULL, 6000, 200, NULL, 2.0, NULL, NULL),
(3626, '25000071', 'CALHA ESPEC.F 71x1mm', 'CALHA ESPECIAL', 'KG', 0.00, 'EUR', 4.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 			 26, NULL, 6, 1, 22, NULL, NULL, 6000, 71, NULL, 1.0, NULL, NULL),
(3627, '25000072', 'CALHA ESPEC.F 72x1mm', 'CALHA ESPECIAL', 'KG', 0.00, 'EUR', 4.33, 0.00, 0.00, '1900-01-01', '1900-01-01', 			 26, NULL, 6, 1, 22, NULL, NULL, 6000, 72, NULL, 1.0, NULL, NULL),
(3628, '25000073', 'CALHA ESPEC.F 73378x2mm', 'CALHA ESPECIAL', 'KG', 1340.00, 'EUR', 3.03, 1.52, 1.52, '2022-10-14', '2024-05-16',		 26, NULL, 6, 1, 22, NULL, NULL, 6000, 378, NULL, 2.0, NULL, NULL),
(3629, '25000074', 'CALHA ESPEC.F 74253x2,3mm', 'CALHA ESPECIAL', 'KG', 1927.00, 'EUR', 2.92, 1.16, 1.16, '2024-03-19', '2024-06-06',    26, NULL, 6, 1, 22, NULL, NULL, 6000, 253, NULL, 2.3, NULL, NULL),
(3630, '25000075', 'CALHA ESPEC.F 75x2,3mm', 'CALHA ESPECIAL', 'KG', 0.00, 'EUR', 2.47, 0.00, 0.00, '1900-01-01', '1900-01-01', 		 26, NULL, 6, 1, 22, NULL, NULL, 6000, 75, NULL, 2.3, NULL, NULL),
(3631, '25000076', 'CALHA ESPEC.F 76x3,2mm', 'CALHA ESPECIAL', 'KG', 672.00, 'EUR', 2.92, 1.38, 1.29, '2023-05-17', '2024-05-16',		 26, NULL, 6, 1, 22, NULL, NULL, 6000, 76, NULL, 3.2, NULL, NULL),
(3632, '25000077', 'CALHA P/CORRER F/73 GALV.', 'CALHA P/CORRER', 'KG', 1556.00, 'EUR', 4.46, 1.62, 1.62, '2024-03-19', '2024-06-12',    26, NULL, 6, 1, 22, 2, NULL, 6000, 73, NULL, NULL, NULL, NULL),
(3633, '25000078', 'CALHA P/CORRER F/74 GALV.', 'CALHA P/CORRER', 'KG', 1276.00, 'EUR', 4.46, 1.69, 1.69, '2024-03-19', '2024-05-16',    26, NULL, 6, 1, 22, 2, NULL, 6000, 74, NULL, NULL, NULL, NULL);







