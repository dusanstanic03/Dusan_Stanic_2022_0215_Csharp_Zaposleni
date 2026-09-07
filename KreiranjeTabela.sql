CREATE TABLE Prodavnica(
	Id INT IDENTITY(1,1) NOT NULL,
	Naziv NVARCHAR(20) NOT NULL,
	Adresa NVARCHAR(50) NOT NULL,
	Mesto NVARCHAR(20) NOT NULL,

	CONSTRAINT PK_Prodavnica PRIMARY KEY (Id),
	CONSTRAINT UQ_Prodavnica_Id UNIQUE (Id),
	CONSTRAINT CK_Prodavnica_Mesto CHECK (MESTO IN ('Beograd', 'Novi Sad', 'Smederevo'))
);

CREATE TABLE Kasir(
	Id INT NOT NULL,
	ImePrezime NVARCHAR(50) NOT NULL,
	ProdavnicaId INT NOT NULL,

	CONSTRAINT PK_Kasir PRIMARY KEY (Id),
	CONSTRAINT UQ_Kasir_Id UNIQUE (Id),
	CONSTRAINT FK_Kasir_Prodavnica FOREIGN KEY (ProdavnicaId) REFERENCES Prodavnica(Id)
);

CREATE TABLE Proizvod(
	Id INT NOT NULL,
	Naziv NVARCHAR(20) NOT NULL,
	Cena DECIMAL(10,2) NOT NULL,
	JedinicaMere NVARCHAR(10) NOT NULL,

	CONSTRAINT PK_Proizvod PRIMARY KEY (Id),
	CONSTRAINT UQ_Proizvod_Id UNIQUE (Id),
	CONSTRAINT CK_Proizvod_Cena CHECK (Cena > 0),
	CONSTRAINT CK_Proizvod_JedinicaMere CHECK (JedinicaMere IN ('kom','kg','litar')),
);

CREATE TABLE Racun(
	Id INT NOT NULL,
	Broj NVARCHAR(15) NOT NULL,
	Datum DATE NOT NULL,
	Iznos DECIMAL(10,2) NOT NULL DEFAULT 0,
	KasirId INT NOT NULL,

	CONSTRAINT PK_Racun PRIMARY KEY (Id),
	CONSTRAINT UQ_Racun UNIQUE (Id),
	CONSTRAINT FK_Racun_Kasir FOREIGN KEY (KasirId) REFERENCES Kasir(Id)
);


CREATE TABLE StavkaRacuna(
	RacunId INT NOT NULL,
	RedniBroj INT NOT NULL,
	Kolicina FLOAT NOT NULL,
	Iznos DECIMAL(10,2) NOT NULL DEFAULT 0,
	ProizvodId INT NOT NULL,

	CONSTRAINT PK_StavkaRacuna PRIMARY KEY (RacunId, RedniBroj),
	CONSTRAINT CK_StavkaRacuna_RedniBroj CHECK (RedniBroj > 0),
	CONSTRAINT CK_StavkaRacuna_Kolicina CHECK (Kolicina > 0),
	CONSTRAINT FK_Stavka_Proizvod FOREIGN KEY (ProizvodId) REFERENCES Proizvod(Id),
	CONSTRAINT FK_Stavka_Racun FOREIGN KEY (RacunId) REFERENCES Racun(Id)
);




