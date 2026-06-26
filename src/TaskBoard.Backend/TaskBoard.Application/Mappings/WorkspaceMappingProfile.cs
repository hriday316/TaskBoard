using System;
using AutoMapper;
using TaskBoard.Domain.Entities;
using TaskBoard.Application.Workspaces.Dtos;

namespace TaskBoard.Application.Mappings;

public class WorkspaceMappingProfile: Profile
{
    public WorkspaceMappingProfile()
    {
        CreateMap<Workspace, WorkspaceDto>();
        CreateMap<WorkspaceDto, Workspace>();

    }

}
