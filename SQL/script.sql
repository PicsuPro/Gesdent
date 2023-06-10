USE [Gesdentdb]
GO
/****** Object:  Table [dbo].[tooth_history]    Script Date: 09/06/2023 17:10:04 ******/
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
/****** Object:  Table [dbo].[tooth]    Script Date: 09/06/2023 17:10:04 ******/
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
/****** Object:  Table [dbo].[patient_record_history]    Script Date: 09/06/2023 17:10:05 ******/
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
/****** Object:  Table [dbo].[patient_record]    Script Date: 09/06/2023 17:10:05 ******/
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
/****** Object:  Table [dbo].[appointment]    Script Date: 09/06/2023 17:10:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[appointment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[patient_id] [int] NOT NULL,
	[staff_id] [uniqueidentifier] NULL,
	[creation_date] [datetime] NOT NULL,
	[subject] [nvarchar](20) NOT NULL,
	[date] [date] NOT NULL,
	[start_time] [time](2) NOT NULL,
	[end_time] [time](2) NOT NULL,
 CONSTRAINT [PK_appointment] PRIMARY KEY CLUSTERED 
(
	[patient_id] ASC,
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[medication]    Script Date: 09/06/2023 17:10:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[medication](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_medication] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[patient]    Script Date: 09/06/2023 17:10:05 ******/
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
/****** Object:  Table [dbo].[staff]    Script Date: 09/06/2023 17:10:05 ******/
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
	[surname] [nvarchar](50) NULL,
 CONSTRAINT [PK_staff] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 09/06/2023 17:10:05 ******/
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
ALTER TABLE [dbo].[appointment] ADD  CONSTRAINT [DF_appointment_creation_date_1]  DEFAULT (getdate()) FOR [creation_date]
GO
ALTER TABLE [dbo].[tooth] ADD  DEFAULT ((0)) FOR [apical_reaction]
GO
ALTER TABLE [dbo].[tooth] ADD  DEFAULT ((0)) FOR [decay]
GO
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [DF_User_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[appointment]  WITH CHECK ADD  CONSTRAINT [FK_appointment_patient] FOREIGN KEY([patient_id])
REFERENCES [dbo].[patient] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[appointment] CHECK CONSTRAINT [FK_appointment_patient]
GO
ALTER TABLE [dbo].[appointment]  WITH CHECK ADD  CONSTRAINT [FK_appointment_staff] FOREIGN KEY([staff_id])
REFERENCES [dbo].[staff] ([user_id])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[appointment] CHECK CONSTRAINT [FK_appointment_staff]
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
