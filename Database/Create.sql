CREATE TABLE NotaFiscal(
    IdNotaFiscal INT IDENTITY(1,1) PRIMARY KEY,
    nNF VARCHAR(255),
    chNFe VARCHAR(255),
    dhEmi VARCHAR(255),
    CNPJEmit VARCHAR(255),
    xNomeEmit VARCHAR(255),
    CNPJDest VARCHAR(255),
    xNomeDest VARCHAR(255),
    EmailDest VARCHAR(255)
)

CREATE TABLE NotaFiscalProdutos(
    IdNotaFiscalProdutos INT IDENTITY(1,1) PRIMARY KEY,
    IdNotaFiscal INT,
    NumeroItem VARCHAR(255),
    CodigoProduto VARCHAR(255),
    Nome VARCHAR(255),
    Qtde DECIMAL,
    ValorUnitario DECIMAL,
    ValorTotal DECIMAL
    FOREIGN KEY (IdNotaFiscal) REFERENCES NotaFiscal(IdNotaFiscal)
)

CREATE TABLE NotaFiscalEnderecos(
    IdNotaFiscalEnderecos INT IDENTITY(1,1) PRIMARY KEY,
    IdNotaFiscal INT,
    DestEmit INT,
    xLgr VARCHAR(255),
    nro VARCHAR(255),
    xBairro VARCHAR(255),
    xMun VARCHAR(255),
    UF VARCHAR(255),
    CEP VARCHAR(255),
    FOREIGN KEY (IdNotaFiscal) REFERENCES NotaFiscal(IdNotaFiscal)
)

-- DROP TABLE NotaFiscalEnderecos
-- DROP TABLE NotaFiscalProdutos
-- DROP TABLE NotaFiscal