USE [GoalsDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 25/07/2024 03:32:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Goals]    Script Date: 25/07/2024 03:32:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Goals](
	[GoalId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](80) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[TotalTasks] [int] NOT NULL,
 CONSTRAINT [PK_Goals] PRIMARY KEY CLUSTERED 
(
	[GoalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskItems]    Script Date: 25/07/2024 03:32:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskItems](
	[TaskItemId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](80) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[GoalId] [int] NOT NULL,
	[GoalId1] [int] NULL,
 CONSTRAINT [PK_TaskItems] PRIMARY KEY CLUSTERED 
(
	[TaskItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240725212947_InitialMigration', N'7.0.1')
GO
SET IDENTITY_INSERT [dbo].[Goals] ON 

INSERT [dbo].[Goals] ([GoalId], [Name], [Date], [TotalTasks]) VALUES (1, N'Configurar plan de compensación', CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), 2)
INSERT [dbo].[Goals] ([GoalId], [Name], [Date], [TotalTasks]) VALUES (2, N'Ejercicio', CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Goals] ([GoalId], [Name], [Date], [TotalTasks]) VALUES (3, N'Prueba Meta 3', CAST(N'2024-07-25T15:31:01.1250000' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Goals] OFF
GO
SET IDENTITY_INSERT [dbo].[TaskItems] ON 

INSERT [dbo].[TaskItems] ([TaskItemId], [Name], [Date], [Status], [GoalId], [GoalId1]) VALUES (1, N'Tarea 1', CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), N'Completada', 1, NULL)
INSERT [dbo].[TaskItems] ([TaskItemId], [Name], [Date], [Status], [GoalId], [GoalId1]) VALUES (2, N'Tarea 2', CAST(N'2024-07-25T00:00:00.0000000' AS DateTime2), N'Abierta', 1, NULL)
INSERT [dbo].[TaskItems] ([TaskItemId], [Name], [Date], [Status], [GoalId], [GoalId1]) VALUES (3, N'Ir al gym', CAST(N'2024-07-25T15:30:36.1310000' AS DateTime2), N'Completada', 2, NULL)
INSERT [dbo].[TaskItems] ([TaskItemId], [Name], [Date], [Status], [GoalId], [GoalId1]) VALUES (4, N'Pasear al perro', CAST(N'2024-07-25T15:30:41.5990000' AS DateTime2), N'Completada', 2, NULL)
INSERT [dbo].[TaskItems] ([TaskItemId], [Name], [Date], [Status], [GoalId], [GoalId1]) VALUES (5, N'Salir a correr', CAST(N'2024-07-25T15:30:45.2010000' AS DateTime2), N'Completada', 2, NULL)
INSERT [dbo].[TaskItems] ([TaskItemId], [Name], [Date], [Status], [GoalId], [GoalId1]) VALUES (6, N'Crear la meta', CAST(N'2024-07-25T15:31:10.0640000' AS DateTime2), N'Completada', 3, NULL)
INSERT [dbo].[TaskItems] ([TaskItemId], [Name], [Date], [Status], [GoalId], [GoalId1]) VALUES (7, N'Hacer 3 tareas', CAST(N'2024-07-25T15:31:17.2740000' AS DateTime2), N'Completada', 3, NULL)
INSERT [dbo].[TaskItems] ([TaskItemId], [Name], [Date], [Status], [GoalId], [GoalId1]) VALUES (8, N'Eliminar la Prueba Meta 3', CAST(N'2024-07-25T15:31:28.4330000' AS DateTime2), N'Abierta', 3, NULL)
SET IDENTITY_INSERT [dbo].[TaskItems] OFF
GO
ALTER TABLE [dbo].[TaskItems]  WITH CHECK ADD  CONSTRAINT [FK_TaskItems_Goals_GoalId] FOREIGN KEY([GoalId])
REFERENCES [dbo].[Goals] ([GoalId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TaskItems] CHECK CONSTRAINT [FK_TaskItems_Goals_GoalId]
GO
ALTER TABLE [dbo].[TaskItems]  WITH CHECK ADD  CONSTRAINT [FK_TaskItems_Goals_GoalId1] FOREIGN KEY([GoalId1])
REFERENCES [dbo].[Goals] ([GoalId])
GO
ALTER TABLE [dbo].[TaskItems] CHECK CONSTRAINT [FK_TaskItems_Goals_GoalId1]
GO
