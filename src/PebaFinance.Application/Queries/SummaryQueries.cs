using MediatR;
using PebaFinance.Application.DTOs;

namespace PebaFinance.Application.Queries;

public record GetSummaryByYearAndMonthQuery(int year, int month) : IRequest<SummaryDto>;
