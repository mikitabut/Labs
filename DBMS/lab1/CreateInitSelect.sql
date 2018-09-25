--DROP TABLE Games; DROP TABLE TournamentPlayers; DROP TABLE Tournaments; DROP TABLE Players; 
/*
SELECT * FROM Players; 
SELECT * FROM Tournaments; 
SELECT * FROM TournamentPlayers; 
SELECT * FROM Games; 
*/

CREATE TABLE Players ( 
	Id NUMBER NOT NULL PRIMARY KEY, 
	LastName VARCHAR2(100) NOT NULL, 
	FirstName VARCHAR2(100) NOT NULL, 
	MiddleName VARCHAR2(100), 
	Rating NUMBER NOT NULL CHECK (Rating BETWEEN 1000 AND 4000), 
	BirthDate DATE NOT NULL CHECK (BirthDate > TO_DATE('01-01-1920', 'dd-mm-yyyy')), 
	ClubEntryDate DATE NOT NULL CHECK (ClubEntryDate > TO_DATE('01-01-2000', 'dd-mm-yyyy')), 
	AnnualMemberPayment VARCHAR2(5) NOT NULL CHECK (AnnualMemberPayment IN ('true', 'false')), 
	ClubMemberDiscount NUMBER NOT NULL CHECK (ClubMemberDiscount BETWEEN 0 AND 100) 
); 

CREATE TABLE Tournaments ( 
	Id NUMBER NOT NULL PRIMARY KEY, 
	Name VARCHAR2(100) NOT NULL UNIQUE, 
	StartDate DATE NOT NULL CHECK (StartDate > TO_DATE('01-01-1920', 'dd-mm-yyyy')), 
	EndDate DATE NOT NULL CHECK (EndDate > TO_DATE('01-01-1920', 'dd-mm-yyyy')), 
	RegistrationFee NUMBER, 
	TournamentSystem VARCHAR2(100) NOT NULL CHECK(TournamentSystem IN('One round-robin', 'Double round-robin', 'Swiss', 'Olympic', 'Match')), 
	PrizeForFirstPlace NUMBER, 
	PrizeForSecondPlace NUMBER, 
	PrizeForThirdPlace NUMBER 
); 

CREATE TABLE TournamentPlayers ( 
	Id NUMBER NOT NULL PRIMARY KEY, 
	TournamentId NUMBER NOT NULL, 
	PlayerId NUMBER NOT NULL, 
	CONSTRAINT FK_TournamentPlayers_Tournaments FOREIGN KEY (TournamentId) REFERENCES Tournaments(Id), 
	CONSTRAINT FK_TournamentPlayers_Players FOREIGN KEY (PlayerId) REFERENCES Players(Id) 
); 

CREATE TABLE Games ( 
	Id NUMBER NOT NULL PRIMARY KEY, 
	TournamentId NUMBER NOT NULL, 
	GameDate DATE NOT NULL CHECK (GameDate > TO_DATE('01-01-1920', 'dd-mm-yyyy')), 
	WhitePlayerId NUMBER NOT NULL, 
	BlackPlayerId NUMBER NOT NULL, 
	Result VARCHAR2(10) NOT NULL CHECK (Result IN ('1-0', '0.5-0.5', '0-1', '+--', '--+', '0.5-0', '0-0.5', '0-0')), 
	Debute VARCHAR2(100) NOT NULL CHECK(Debute IN('Open Game', 'Four Knights Game', 'Philidor Defense', 'Kings Gambit', 'Center Game', 
		'Sicilian Defense', 'French Defense', 'Caro–Kann Defense', 'Queens Gambit', 'Modern Benoni', 'Catalan Opening', 'English Opening')), 
	Moves NUMBER NOT NULL CHECK (Moves < 6000), 
	CONSTRAINT FK_Games_Players_White FOREIGN KEY (WhitePlayerId) REFERENCES Players(Id), 
	CONSTRAINT FK_Games_Players_Black FOREIGN KEY (BlackPlayerId) REFERENCES Players(Id), 
	CONSTRAINT FK_Games_Tournaments FOREIGN KEY (TournamentId) REFERENCES Tournaments(Id) 
); 

INSERT INTO Players VALUES (1, 'Carlsen', 'Magnus', 'Sven Een', 2839, TO_DATE('30-11-1990', 'dd-mm-yyyy'), TO_DATE('02-01-2000', 'dd-mm-yyyy'), 'true', 0); 
INSERT INTO Players VALUES (2, 'Caruana', 'Fabiano', 'Luigi', 2827, TO_DATE('30-07-1992', 'dd-mm-yyyy'), TO_DATE('01-02-2000', 'dd-mm-yyyy'), 'true', 0); 
INSERT INTO Players VALUES (3, 'Mamedyarov', 'Shakhriyar', 'Gamid ogly', 2820, TO_DATE('12-04-1985', 'dd-mm-yyyy'), TO_DATE('01-03-2000', 'dd-mm-yyyy'), 'true', 0); 
INSERT INTO Players VALUES (4, 'Ding', 'Liren', null, 2804, TO_DATE('24-10-1992', 'dd-mm-yyyy'), TO_DATE('01-04-2000', 'dd-mm-yyyy'), 'true', 0); 
INSERT INTO Players VALUES (5, 'Vachier-Lagrave', 'Maxime', null, 2780, TO_DATE('21-10-1990', 'dd-mm-yyyy'), TO_DATE('01-05-2000', 'dd-mm-yyyy'), 'true', 10); 
INSERT INTO Players VALUES (6, 'Aronian', 'Levon', 'Grigorievich', 2780, TO_DATE('06-10-1982', 'dd-mm-yyyy'), TO_DATE('01-06-2000', 'dd-mm-yyyy'), 'true', 0); 
INSERT INTO Players VALUES (7, 'Giri', 'Anish', 'Kumar', 2780, TO_DATE('28-06-1994', 'dd-mm-yyyy'), TO_DATE('01-07-2000', 'dd-mm-yyyy'), 'true', 0); 
INSERT INTO Players VALUES (8, 'Kramnik', 'Vladimir', 'Borisovich', 2779, TO_DATE('25-06-1975', 'dd-mm-yyyy'), TO_DATE('01-08-2000', 'dd-mm-yyyy'), 'true', 20); 
INSERT INTO Players VALUES (9, 'So', 'Wesley', 'Barbasa', 2776, TO_DATE('09-11-1993', 'dd-mm-yyyy'), TO_DATE('01-09-2000', 'dd-mm-yyyy'), 'true', 0); 
INSERT INTO Players VALUES (10, 'Anand', 'Viswanathan', null, 2771, TO_DATE('11-12-1969', 'dd-mm-yyyy'), TO_DATE('01-10-2000', 'dd-mm-yyyy'), 'true', 0); 

INSERT INTO Tournaments VALUES (1, 'Candidates Tournament 2018', TO_DATE('10-03-2018', 'dd-mm-yyyy'), TO_DATE('28-03-2018', 'dd-mm-yyyy'), 0, 'Double round-robin', 95000, 88000, 75000); 
INSERT INTO Tournaments VALUES (2, 'Chess World Cup 2017', TO_DATE('02-09-2017', 'dd-mm-yyyy'), TO_DATE('27-09-2017', 'dd-mm-yyyy'), 1000, 'Olympic', 120000, 80000, 50000); 

INSERT INTO TournamentPlayers VALUES (1, 1, 2); 
INSERT INTO TournamentPlayers VALUES (2, 1, 3); 
INSERT INTO TournamentPlayers VALUES (3, 1, 4); 
INSERT INTO TournamentPlayers VALUES (4, 1, 6); 
INSERT INTO TournamentPlayers VALUES (5, 1, 8); 
INSERT INTO TournamentPlayers VALUES (6, 1, 9); 
INSERT INTO TournamentPlayers VALUES (7, 2, 1); 
INSERT INTO TournamentPlayers VALUES (8, 2, 2); 
INSERT INTO TournamentPlayers VALUES (9, 2, 3); 
INSERT INTO TournamentPlayers VALUES (10, 2, 4); 
INSERT INTO TournamentPlayers VALUES (11, 2, 5); 
INSERT INTO TournamentPlayers VALUES (12, 2, 6); 
INSERT INTO TournamentPlayers VALUES (13, 2, 7); 
INSERT INTO TournamentPlayers VALUES (14, 2, 8); 
INSERT INTO TournamentPlayers VALUES (15, 2, 9); 
INSERT INTO TournamentPlayers VALUES (16, 2, 10); 

INSERT INTO Games VALUES (1, 1, TO_DATE('10-03-2018', 'dd-mm-yyyy'), 6, 4, '0.5-0.5', 'Catalan Opening', 62); 
INSERT INTO Games VALUES (2, 1, TO_DATE('10-03-2018', 'dd-mm-yyyy'), 2, 9, '1-0', 'Four Knights Game', 57); 
INSERT INTO Games VALUES (3, 1, TO_DATE('12-03-2018', 'dd-mm-yyyy'), 2, 3, '0.5-0.5', 'Caro–Kann Defense', 32); 
INSERT INTO Games VALUES (4, 1, TO_DATE('15-03-2018', 'dd-mm-yyyy'), 9, 8, '0.5-0.5', 'French Defense', 78); 
INSERT INTO Games VALUES (5, 1, TO_DATE('10-03-2018', 'dd-mm-yyyy'), 3, 4, '0-1', 'Sicilian Defense', 41);
