// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views;
public class GetOrderDtoResponse : GenericResponse
{
    public OrderDTO? order { get; set; }

    public GetOrderDtoResponse(OrderDTO order, int result_code, string language) : base(result_code, language)
    {
        this.order = order;
    }

    public GetOrderDtoResponse(int result_code, string language) : base(result_code, language)
    {
    }
}



public class GetOrderDtoListResponse : GenericResponse
{
    public List<OrderDTO>? orders { get; set; }

    public GetOrderDtoListResponse(List<OrderDTO> orders, int result_code, string language) : base(result_code, language)
    {
        this.orders = orders;
    }

    public GetOrderDtoListResponse(int result_code, string language) : base(result_code, language)
    {
    }
}