using BeHealthy.Domain.Entities;
using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.API.Mapping;

public static class AppSettingMappingExtensions
{
    public static AppSettingResponse MapToDto(this AppSetting setting) => new()
    {
        Id = setting.Id,
        Key = setting.Key,
        Type = setting.Type,
        Group = setting.Group,
        Value = setting.Value,
        Caption = setting.Caption,
        Description = setting.Description
    };
}
