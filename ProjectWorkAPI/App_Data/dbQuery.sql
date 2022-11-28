
DROP TABLE IF EXISTS Stats
DROP TABLE IF EXISTS OrderItems
DROP TABLE IF EXISTS Orders
DROP TABLE IF EXISTS OrderStates
DROP TABLE IF EXISTS Items
DROP TABLE IF EXISTS Users


CREATE TABLE Users(
	Id CHAR(10) PRIMARY KEY,
	Username VARCHAR(100) NOT NULL,
	Password VARCHAR(MAX) NOT NULL,
	Salt CHAR(32) NOT NULL,
	Role VARCHAR(20)
)

INSERT INTO Users VALUES('KJAJKD9AKA','Azienda','934BD008853BBEBDD6DF7D1DC1CD350AD5BB759D8CAFD89BBE862F58CCC88506FE9D5AF1AB2153FBD78C5F6F544D6BC4E4D7E865E6AD858F0E9C657EC6078DBB','6C3B5D7E39F41A8E4B45224F4F2DE4D6','Azienda')
INSERT INTO Users VALUES('MS9239KKSA','Rivenditore','B1F8BA8A5EA5D14D7D455DABF0DD95795B222160055C575A3EEA19D4F369A8F817ACA0356FCD926DBF624C30E70DCD682B1DDFAEF58BBC91DB87EBDC0948F322','212D8763D22A3B19B937B14932A8A2DB','Rivenditore')
INSERT INTO Users VALUES('CMMNQ92MMA','Rivenditore2','93527948D459295C89D16144D5C20E7315E110B496D42061EE4F1DB5FBAB65DD86A677DA6416E6D76EF24C5C3F039584861C70A5BB8347DFC0A37DF2FE104C22','1DB5D8376FFBFD8149776FFDD36B407C','Rivenditore')
INSERT INTO Users VALUES('ZZ9ALSPAAP','Admin','B2161ED0364DDBA40B24912E6A9D35097FAB04F89B9C749529AC69118784FB3901B6ABC2B930C29F37CBC034A24F3195DD68C0A90338BCB3D32881FE3EC727CA','B50213C7500D0CA73A0A5CEFCB90EA7D','Admin')

CREATE TABLE Items(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Code CHAR(8) NOT NULL,
	Name VARCHAR(1000) NOT NULL,
	Weight FLOAT,
	Volume FLOAT,
	UnitaryPrice FLOAT,
	LeadTime INT
)

INSERT INTO Items VALUES('GRDN001B','Tavolo da giardino 160x80 bianco',10, 0.3, 100, 5)
INSERT INTO Items VALUES('GRDN001N','Tavolo da giardino 160x80 nero',  10, 0.3, 100, 6)
INSERT INTO Items VALUES('GRDN002B','Tavolo da giardino 80x80 bianco',5.9, 0.2, 60, 5)
INSERT INTO Items VALUES('GRDN002N','Tavolo da giardino 80x80 nero',  5.9, 0.2, 60, 6)
INSERT INTO Items VALUES('GRDN003B','Sedia da giardino bianca (confezione da 2)',  4.5, 0.5, 20, 7)
INSERT INTO Items VALUES('GRDN003N','Sedia da giardino nera (confezione da 2)',  4.5, 0.5, 20, 8)
INSERT INTO Items VALUES('GRDN004B','Sedia da giardino bianca (confezione da 4)', 9, 0.5, 40, 5)
INSERT INTO Items VALUES('GRDN004N','Sedia da giardino nera (confezione da 4)',  9, 0.5, 40, 6)
INSERT INTO Items VALUES('SLTT001N','Mobile porta TV Noce 200x50x50',  50, 3, 500, 9)
INSERT INTO Items VALUES('SLTT001B','Mobile porta TV Laccato Bianco 200x50x50',  50, 3, 900, 13)

CREATE TABLE OrderStates(
	Id INT PRIMARY KEY,
	Name VARCHAR(20) NOT NULL
)

INSERT INTO OrderStates VALUES(10, 'Inserito')
INSERT INTO OrderStates VALUES(20, 'Confermato')
INSERT INTO OrderStates VALUES(30, 'InProduzione')
INSERT INTO OrderStates VALUES(40, 'Prodotto')

CREATE TABLE Orders(
	Id VARCHAR(32) PRIMARY KEY,
	ResellerId CHAR(10) NOT NULL,
	CustomerId INT NOT NULL,
	OrderDate DATE NOT NULL,
	OrderStateId INT NOT NULL,
	SendDate DATETIME NOT NULL,
	StartProductionDate DATETIME,
	StopProductionDate DATETIME,
	Notes VARCHAR(MAX),
	FOREIGN KEY (OrderStateId) REFERENCES OrderStates(Id)
)

CREATE TABLE OrderItems(
	Id INT PRIMARY KEY IDENTITY(1,1),
	OrderId VARCHAR(32) NOT NULL,
	ItemId INT NOT NULL,
	Quantity INT NOT NULL,
	UnitaryPrice FLOAT NOT NULL,
	FOREIGN KEY (OrderId) REFERENCES Orders(Id),
	FOREIGN KEY (ItemId) REFERENCES Items(Id)
)

INSERT INTO Orders VALUES('DLOESKASJSJWUAHDKJAHDKJAHDIAUSDH','MS9239KKSA',1,'2022-11-16',20,'2022-11-16',NULL,NULL,NULL)
INSERT INTO OrderItems VALUES('DLOESKASJSJWUAHDKJAHDKJAHDIAUSDH',1,6,100)
INSERT INTO OrderItems VALUES('DLOESKASJSJWUAHDKJAHDKJAHDIAUSDH',3,1,60)

CREATE TABLE Stats(
	UserId CHAR(10) NOT NULL,
	OrderStateId INT NOT NULL,
	NumberOfOrders INT NOT NULL,
	ValueOfOrders FLOAT NOT NULL,
	PRIMARY KEY (UserId, OrderStateId),
	FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (OrderStateId) REFERENCES OrderStates(Id)
)