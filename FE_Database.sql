BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS `Users` (
	`userID`	integer,
	`permType`	varchar ( 255 ),
	`userPassword`	TEXT,
	PRIMARY KEY(`userID`),
	FOREIGN KEY(`permType`) REFERENCES `Permissions`(`permType`)
);
INSERT INTO `Users` VALUES (100,'Administrator',NULL);
INSERT INTO `Users` VALUES (200,'Database Manager',NULL);
INSERT INTO `Users` VALUES (300,'Contributor',NULL);
INSERT INTO `Users` VALUES (400,'Viewer',NULL);
CREATE TABLE IF NOT EXISTS `Transactions` (
	`transactionID`	INTEGER,
	`transactionDate`	TEXT,
	`transactionTime`	TEXT,
	PRIMARY KEY(`transactionID`)
);
CREATE TABLE IF NOT EXISTS `Permissions` (
	`permType`	varchar ( 255 ),
	`permImport`	boolean NOT NULL CHECK(permImport IN ( 0 , 1 )),
	`permExport`	boolean NOT NULL CHECK(permExport IN ( 0 , 1 )),
	`permChPerm`	boolean NOT NULL CHECK(permChPerm IN ( 0 , 1 )),
	`permEditDB`	boolean NOT NULL CHECK(permEditDB IN ( 0 , 1 )),
	PRIMARY KEY(`permType`)
);
INSERT INTO `Permissions` VALUES ('Administrator',1,1,1,1);
INSERT INTO `Permissions` VALUES ('Database Manager',1,1,0,1);
INSERT INTO `Permissions` VALUES ('Contributor',1,1,0,0);
INSERT INTO `Permissions` VALUES ('Viewer',0,0,0,0);
CREATE TABLE IF NOT EXISTS `DataSets` (
	`dataSetID`	TEXT,
	`dataSetType`	TEXT,
	`dataSetUploadTime`	TEXT,
	`dataSetUploadDate`	TEXT,
	`transactionID`	INTEGER,
	FOREIGN KEY(`transactionID`) REFERENCES `Transactions`(`transactionID`),
	PRIMARY KEY(`dataSetID`)
);
CREATE TABLE IF NOT EXISTS `ComplianceEntries` (
	`entryID`	INTEGER,
	`status`	TEXT,
	`sysID`	INTEGER NOT NULL,
	`swName`	TEXT,
	`vKey`	TEXT,
	`comments`	TEXT,
	PRIMARY KEY(`entryID`),
	FOREIGN KEY(`comments`) REFERENCES `Comments`(`commentID`)
);
CREATE TABLE IF NOT EXISTS `Comments` (
	`commentID`	INTEGER,
	`commentText`	TEXT,
	`entryID`	INTEGER,
	`transactionID`	INTEGER,
	PRIMARY KEY(`commentID`),
	FOREIGN KEY(`transactionID`) REFERENCES `Transactions`(`transactionID`),
	FOREIGN KEY(`entryID`) REFERENCES `ComplianceEntries`(`entryID`)
);
COMMIT;
