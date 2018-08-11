CREATE TABLE [dbo].[Stock](
	[ItemBarcode] [varchar](13) NOT NULL,
	[ItemName] [nvarchar](50) NOT NULL,
	[ItemCount] [int] NULL,
	[ItemPrice] [numeric](10, 2) NULL,
	[StoredBy] [varchar](20) NULL,
	[StoredDate] [datetime] NOT NULL,
	[ModBy] [varchar](20) NOT NULL,
	[ModDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED 
(
	[ItemBarcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Stock] ADD  CONSTRAINT [DF_Stock_ItemCount]  DEFAULT ((0)) FOR [ItemCount]
GO

ALTER TABLE [dbo].[Stock] ADD  CONSTRAINT [DF_Stock_ItemPrice]  DEFAULT ((0.0)) FOR [ItemPrice]
GO

ALTER TABLE [dbo].[Stock] ADD  CONSTRAINT [DF_Stock_StoredDate]  DEFAULT (getdate()) FOR [StoredDate]
GO

ALTER TABLE [dbo].[Stock] ADD  CONSTRAINT [DF_Stock_ModDate]  DEFAULT (getdate()) FOR [ModDate]
GO

CREATE TABLE [dbo].[OrderDetail](
	[DetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ItemBarcode] [varchar](13) NOT NULL,
	[UnitPrice] [numeric](10, 2) NOT NULL,
	[ItemCount] [int] NOT NULL,
	[Discount] [numeric](6, 2) NOT NULL,
	[DetailPrice] [numeric](10, 2) NOT NULL,
	[IsValid] [bit] NOT NULL,
	[DatailDate] [datetime] NOT NULL,
	[Remarks] [nvarchar](100) NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[DetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[OrderDetail] ADD  CONSTRAINT [DF_OrderDetail_Discount]  DEFAULT ((1)) FOR [Discount]
GO

ALTER TABLE [dbo].[OrderDetail] ADD  CONSTRAINT [DF_OrderDetail_DatailDate]  DEFAULT (getdate()) FOR [DatailDate]
GO

CREATE TABLE [dbo].[Order](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[MemberID] [int] NULL,
	[DiscountID] [int] NULL,
	[OrderPrice] [numeric](10, 2) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[IsValid] [bit] NOT NULL,
	[ModBy] [datetime] NOT NULL,
	[Remarks] [nvarchar](100) NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_OrderDate]  DEFAULT (getdate()) FOR [OrderDate]
GO


ssh-keygen-t rsa-C "zhyrain@msn.com"