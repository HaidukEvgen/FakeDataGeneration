export interface User {
  index: number;
  id: string;
  name: string;
  address: number;
  phoneNumber: string;
}

export interface UsersPaginator {
  items: User[];
  page: number;
}
