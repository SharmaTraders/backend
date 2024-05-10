using MediatR;

namespace CommandContracts.item;

public static class UpdateItemCommand {

    public record Request(string Id, string Name) : IRequest;



    
}