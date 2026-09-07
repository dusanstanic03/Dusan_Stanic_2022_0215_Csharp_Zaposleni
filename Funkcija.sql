CREATE FUNCTION fn_vratiNajveciRacunZaKasira 
(
	@KasirId INT
)
RETURNS INT
AS
BEGIN

	DECLARE @RacunId INT;

	SELECT TOP 1 @RacunId=R.Id
	FROM Racun R 
	JOIN StavkaRacuna SR ON R.Id = SR.RacunId
	JOIN Proizvod P ON SR.ProizvodId = P.Id
	WHERE R.KasirId = @KasirId
	GROUP BY R.Id
	ORDER BY SUM(SR.Kolicina * P.Cena) DESC;

	RETURN @RacunId;

END;