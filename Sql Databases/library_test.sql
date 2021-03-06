USE [master]
GO
/****** Object:  Database [library_test]    Script Date: 7/21/2016 9:35:19 AM ******/
CREATE DATABASE [library_test]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'library', FILENAME = N'C:\Users\epicodus\library_test.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'library_log', FILENAME = N'C:\Users\epicodus\library_test_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [library_test] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [library_test].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [library_test] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [library_test] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [library_test] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [library_test] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [library_test] SET ARITHABORT OFF 
GO
ALTER DATABASE [library_test] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [library_test] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [library_test] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [library_test] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [library_test] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [library_test] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [library_test] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [library_test] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [library_test] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [library_test] SET  DISABLE_BROKER 
GO
ALTER DATABASE [library_test] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [library_test] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [library_test] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [library_test] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [library_test] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [library_test] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [library_test] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [library_test] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [library_test] SET  MULTI_USER 
GO
ALTER DATABASE [library_test] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [library_test] SET DB_CHAINING OFF 
GO
ALTER DATABASE [library_test] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [library_test] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [library_test] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [library_test] SET QUERY_STORE = OFF
GO
USE [library_test]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [library_test]
GO
/****** Object:  Table [dbo].[authors]    Script Date: 7/21/2016 9:35:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[authors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[authors_books]    Script Date: 7/21/2016 9:35:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[authors_books](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[author_id] [int] NULL,
	[book_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[books]    Script Date: 7/21/2016 9:35:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[books](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](255) NULL,
	[publish_date] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[checkouts]    Script Date: 7/21/2016 9:35:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[checkouts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[copy_id] [int] NULL,
	[patron_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[copies]    Script Date: 7/21/2016 9:35:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[copies](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[checkout_date] [datetime] NULL,
	[condition] [varchar](255) NULL,
	[book_id] [int] NULL,
	[due_date] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[patrons]    Script Date: 7/21/2016 9:35:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[patrons](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [varchar](255) NULL,
	[last_name] [varchar](255) NULL,
	[phone_number] [varchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [library_test] SET  READ_WRITE 
GO
