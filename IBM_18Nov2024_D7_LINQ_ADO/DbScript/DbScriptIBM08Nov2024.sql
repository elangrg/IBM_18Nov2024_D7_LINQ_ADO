USE [IBM08Nov2024Db]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 20-11-2024 02:09:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](50) NOT NULL,
	[Qty] [int] NOT NULL,
	[Rate] [money] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetProductNameByID]    Script Date: 20-11-2024 02:09:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetProductNameByID] 
	@PrdId int =  0, 
	@PrdName varchar(50) out
AS
BEGIN
	    
	SELECT @PrdName=ProductName from Product where ProductID =@PrdId
END
GO


CREATE PROCEDURE GetAllProducts
	
AS
BEGIN
	    
	SELECT * from Product 
END
GO
