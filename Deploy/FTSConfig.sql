-- Create catalog
CREATE FULLTEXT CATALOG ftCustomersCatalog AS DEFAULT;

-- Create index on Customer
CREATE UNIQUE INDEX ui_uCustomer ON Customers(Id); 

-- Create FTS index
CREATE FULLTEXT INDEX ON Customers (SearchKeywords) KEY INDEX ui_uCustomer ON ftCustomersCatalog

-- Enabled indexing
ALTER FULLTEXT INDEX ON Customers ENABLE; 
GO 

-- Retrieve available stoplists
-- select * from sys.fulltext_stoplists

-- Create empty stop list and assign it
CREATE FULLTEXT STOPLIST uCustomersSLEmpty;
ALTER FULLTEXT INDEX ON Customers SET STOPLIST uCustomersSLEmpty;

-- Start populating
ALTER FULLTEXT INDEX ON Customers START FULL POPULATION;

-- Monitor
-- SELECT * FROM sys.dm_fts_index_population

-- Sample Using CONTAINS
-- SET STATISTICS TIME ON
-- SELECT * FROM Customers
-- WHERE  CONTAINS(SearchKeywords, 'v AND b AND z')
