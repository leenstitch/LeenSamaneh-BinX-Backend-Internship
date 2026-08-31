## Day 3 — Core Routes I: Catalog & Read Operations
- Implemented pagination for the Vital Signs GetAll endpoint using Page and PageSize.
 <img width="612" height="180" alt="image" src="https://github.com/user-attachments/assets/4af42872-e924-4e9d-ad29-b563942de064" />

- Added TotalCount and TotalPages to the paginated response.
 <img width="698" height="376" alt="image" src="https://github.com/user-attachments/assets/10ac4502-83d4-456a-a82a-8d2e51159ba6" />

- Implemented filtering by patient name and gender using query parameters.
  <img width="755" height="225" alt="image" src="https://github.com/user-attachments/assets/b19e4ecb-7b64-44c8-a0b7-4165fb6091c2" />

  <img width="663" height="342" alt="image" src="https://github.com/user-attachments/assets/919ff563-483c-4b85-ad03-30fe490677a6" />

- Implemented sorting by measurement date or name using asc and descending order.
<img width="676" height="617" alt="image" src="https://github.com/user-attachments/assets/f02be66c-28d6-4dea-8d65-4726f42e1325" />


- Created a reusable generic PaginatedResponseDto<T>.
- <img width="731" height="231" alt="image" src="https://github.com/user-attachments/assets/7c7869f6-2bbc-4a23-a4e8-f781867b07a6" />

- Created VitalSignQueryDto to handle pagination, filtering, and sorting parameters.
 <img width="878" height="472" alt="image" src="https://github.com/user-attachments/assets/96ab3143-02b5-44e4-a437-df6317dd0487" />

- Updated the Repository to use Skip() and Take() for pagination.
- Updated the Service to validate pagination parameters, calculate total pages, and map entities to DTOs.
- Improved error handling so invalid query values such as an invalid gender return 400 Bad Request.
- Tested the endpoint with different pagination, filtering, and sorting combinations using Postman.
