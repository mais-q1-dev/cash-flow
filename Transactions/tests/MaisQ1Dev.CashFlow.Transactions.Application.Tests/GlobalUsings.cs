﻿global using FluentAssertions;
global using FluentAssertions.Execution;
global using MaisQ1Dev.CashFlow.Transactions.Application.Abstractions.Data;
global using MaisQ1Dev.CashFlow.Transactions.Application.Companies.Common;
global using MaisQ1Dev.CashFlow.Transactions.Application.Companies.CreateCompany;
global using MaisQ1Dev.CashFlow.Transactions.Application.Companies.DomainEvents;
global using MaisQ1Dev.CashFlow.Transactions.Application.Companies.GetCompanyById;
global using MaisQ1Dev.CashFlow.Transactions.Application.Companies.ListCompany;
global using MaisQ1Dev.CashFlow.Transactions.Application.Companies.UpdateCompany;
global using MaisQ1Dev.CashFlow.Transactions.Application.Tests.Utils;
global using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.Common;
global using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.CreateTransaction;
global using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.GetTransactionById;
global using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.ListTransaction;
global using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.UpdateTransaction;
global using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
global using MaisQ1Dev.CashFlow.Transactions.Domain.Companies.DomainEvents;
global using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
global using MaisQ1Dev.Libs.Domain;
global using MaisQ1Dev.Libs.Domain.Database;
global using MaisQ1Dev.Libs.IntegrationEvents.Companies;
global using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Query;
global using Moq;
global using System.Linq.Expressions;


