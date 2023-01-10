SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Produse](
	[ProdusId] [int] NOT NULL,
	[UtilizatorId] [int] NOT NULL,
	[PretBuc] [decimal](18, 0) NOT NULL,
	[Cantitate] [decimal](18, 0) NOT NULL,
	[PretFinal] [decimal](18, 0) NOT NULL,
	[Adresa] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Produse] PRIMARY KEY CLUSTERED 
(
	[ProdusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Produse]  WITH CHECK ADD  CONSTRAINT [FK_Produse_Utilizatori] FOREIGN KEY([UtilizatorId])
REFERENCES [dbo].[Utilizatori] ([UtilizatorId])
GO
ALTER TABLE [dbo].[Produse] CHECK CONSTRAINT [FK_Produse_Utilizatori]
GO
