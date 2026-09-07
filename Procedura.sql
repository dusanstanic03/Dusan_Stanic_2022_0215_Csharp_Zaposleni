CREATE PROCEDURE sp_azuriraj_racun
	@RacunId INT,
	@RedniBroj INT
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM Racun WHERE Id = @RacunId)
		THROW 50001, 'Racun ne postoji', 1;

	IF NOT EXISTS (SELECT 1 FROM StavkaRacuna WHERE RacunId = @RacunId AND RedniBroj = @RedniBroj)
		THROW 50002, 'Stavka ne postoji', 1;

	DELETE FROM StavkaRacuna 
	WHERE RacunId = @RacunId AND RedniBroj = @RedniBroj;

	UPDATE Racun
	SET Iznos = (
		SELECT COALESCE(SUM(Iznos),0)
		FROM StavkaRacuna
		WHERE RacunId = @RacunId
		)
	WHERE Id = @RacunId;
	END;