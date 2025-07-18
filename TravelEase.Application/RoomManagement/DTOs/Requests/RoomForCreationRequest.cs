﻿namespace TravelEase.Application.RoomManagement.DTOs.Requests
{
    public class RoomForCreationRequest
    {
        public Guid RoomTypeId { get; set; }
        public int AdultsCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public string View { get; set; }
        public float Rating { get; set; }
    }
}