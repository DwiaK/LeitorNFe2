SELECT * FROM [NotaFiscal]
SELECT * FROM [NotaFiscalEnderecos]
SELECT * FROM [NotaFiscalProdutos]



-- DELETE FROM [NotaFiscalProdutos]
-- DELETE FROM [NotaFiscalEnderecos]
-- DELETE FROM [NotaFiscal]



SELECT
  [NF].*,
  [nfEmitente].*,
  [nfDestinatario].*
    FROM
        [NotaFiscal][NF]
    JOIN
        [NotaFiscalEnderecos][nfEmitente]
        ON
            [nfEmitente].[IdNotaFiscal] = [NF].[IdNotaFiscal]
        AND
            [nfEmitente].[IsEmit] = 1
    JOIN
        [NotaFiscalEnderecos][nfDestinatario]
        ON
            [nfDestinatario].[IdNotaFiscal] = [NF].[IdNotaFiscal]
        AND
            [nfDestinatario].[IsEmit] = 0
WHERE
    [NF].[IdNotaFiscal] = 1