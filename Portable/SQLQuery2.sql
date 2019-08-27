
CREATE TABLE [dbo].[OrderDetail](
	[DetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ItemBarcode] [varchar](15) NOT NULL,
	[UnitPrice] [numeric](10, 2) NOT NULL,
	[ItemCount] [int] NOT NULL,
	[Discount] [numeric](6, 2) NOT NULL,
	[DetailPrice] [numeric](10, 2) NOT NULL,
	[ActualPrice] [numeric](10, 2) NULL,
	[IsValid] [bit] NOT NULL,
	[DatailDate] [datetime] NOT NULL,
	[Remarks] [nvarchar](100) NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[DetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[Stock](
	[ItemBarcode] [varchar](15) NOT NULL,
	[ItemName] [nvarchar](50) NOT NULL,
	[ItemCount] [int] NULL,
	[ItemPrice] [numeric](10, 2) NULL,
	[SalePrice] [numeric](10, 2) NULL,
	[StoredBy] [varchar](20) NULL,
	[StoredDate] [datetime2](7) NOT NULL,
	[ModBy] [varchar](20) NOT NULL,
	[ModDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED 
(
	[ItemBarcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]