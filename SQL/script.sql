USE [master]
GO
/****** Object:  Database [Gesdentdb]    Script Date: 07/05/2023 21:13:14 ******/
CREATE DATABASE [Gesdentdb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Gesdentdb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Gesdentdb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Gesdentdb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Gesdentdb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Gesdentdb] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Gesdentdb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Gesdentdb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Gesdentdb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Gesdentdb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Gesdentdb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Gesdentdb] SET ARITHABORT OFF 
GO
ALTER DATABASE [Gesdentdb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Gesdentdb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Gesdentdb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Gesdentdb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Gesdentdb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Gesdentdb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Gesdentdb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Gesdentdb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Gesdentdb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Gesdentdb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Gesdentdb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Gesdentdb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Gesdentdb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Gesdentdb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Gesdentdb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Gesdentdb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Gesdentdb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Gesdentdb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Gesdentdb] SET  MULTI_USER 
GO
ALTER DATABASE [Gesdentdb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Gesdentdb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Gesdentdb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Gesdentdb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Gesdentdb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Gesdentdb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Gesdentdb] SET QUERY_STORE = ON
GO
ALTER DATABASE [Gesdentdb] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Gesdentdb]
GO
/****** Object:  Table [dbo].[tooth_history]    Script Date: 07/05/2023 21:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tooth_history](
	[patient_record_id] [int] NOT NULL,
	[number] [int] NOT NULL,
	[apical_reaction] [bit] NOT NULL,
	[decay] [bit] NOT NULL,
	[time_start] [datetime2](7) NOT NULL,
	[time_end] [datetime2](7) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Index [ix_tooth_history]    Script Date: 07/05/2023 21:13:14 ******/
CREATE CLUSTERED INDEX [ix_tooth_history] ON [dbo].[tooth_history]
(
	[time_end] ASC,
	[time_start] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tooth]    Script Date: 07/05/2023 21:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tooth](
	[patient_record_id] [int] NOT NULL,
	[number] [int] NOT NULL,
	[apical_reaction] [bit] NOT NULL,
	[decay] [bit] NOT NULL,
	[time_start] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[time_end] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[patient_record_id] ASC,
	[number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([time_start], [time_end])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[tooth_history])
)
GO
/****** Object:  Table [dbo].[patient_record_history]    Script Date: 07/05/2023 21:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[patient_record_history](
	[patient_id] [int] NOT NULL,
	[time_start] [datetime2](7) NOT NULL,
	[time_end] [datetime2](7) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Index [ix_patient_record_history]    Script Date: 07/05/2023 21:13:14 ******/
CREATE CLUSTERED INDEX [ix_patient_record_history] ON [dbo].[patient_record_history]
(
	[time_end] ASC,
	[time_start] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[patient_record]    Script Date: 07/05/2023 21:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[patient_record](
	[patient_id] [int] NOT NULL,
	[time_start] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[time_end] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[patient_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([time_start], [time_end])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[patient_record_history])
)
GO
/****** Object:  Table [dbo].[patient]    Script Date: 07/05/2023 21:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[patient](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[last_name] [varchar](50) NOT NULL,
	[first_name] [varchar](50) NOT NULL,
	[surname] [varchar](50) NULL,
	[sex] [bit] NOT NULL,
	[phone] [varchar](20) NOT NULL,
	[phone_alt] [varchar](20) NULL,
	[email] [varchar](255) NULL,
	[birth_date] [date] NOT NULL,
	[profession] [varchar](150) NULL,
	[address] [varchar](255) NULL,
	[motive] [varchar](255) NULL,
	[oriented_by] [varchar](150) NULL,
	[preferred_day] [varchar](100) NULL,
	[parent_name] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[staff]    Script Date: 07/05/2023 21:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[staff](
	[user_id] [uniqueidentifier] NOT NULL,
	[last_name] [nvarchar](50) NOT NULL,
	[first_name] [nvarchar](50) NOT NULL,
	[sex] [bit] NOT NULL,
	[phone] [nvarchar](20) NOT NULL,
	[phone_alt] [nvarchar](20) NULL,
	[email] [nvarchar](255) NOT NULL,
	[birth_date] [date] NOT NULL,
 CONSTRAINT [PK_staff] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 07/05/2023 21:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[id] [uniqueidentifier] NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[hash] [varbinary](100) NOT NULL,
	[salt] [varbinary](50) NOT NULL,
	[email] [nvarchar](255) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Unique_Username] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tooth] ADD  DEFAULT ((0)) FOR [apical_reaction]
GO
ALTER TABLE [dbo].[tooth] ADD  DEFAULT ((0)) FOR [decay]
GO
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [DF_User_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[patient_record]  WITH CHECK ADD  CONSTRAINT [FK__patient_r__patie__7E37BEF6] FOREIGN KEY([patient_id])
REFERENCES [dbo].[patient] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[patient_record] CHECK CONSTRAINT [FK__patient_r__patie__7E37BEF6]
GO
ALTER TABLE [dbo].[staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_User] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[staff] CHECK CONSTRAINT [FK_Staff_User]
GO
ALTER TABLE [dbo].[tooth]  WITH CHECK ADD  CONSTRAINT [FK__tooth__patient_r__02084FDA] FOREIGN KEY([patient_record_id])
REFERENCES [dbo].[patient_record] ([patient_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tooth] CHECK CONSTRAINT [FK__tooth__patient_r__02084FDA]
GO
USE [master]
GO
ALTER DATABASE [Gesdentdb] SET  READ_WRITE 
GO
