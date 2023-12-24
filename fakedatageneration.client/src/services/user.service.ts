import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User, UsersPaginator } from '../models/models';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiUrl = 'http://localhost:5249/api/FakeData';

  constructor(private http: HttpClient) {}

  public getUsers(
    region: number = 0,
    seed: number = 0,
    errorsAmount: number = 0,
    pageNumber: number = 0,
    perPage: number = 20
  ): Observable<UsersPaginator> {
    return this.http
      .get<User[]>(this.apiUrl, {
        params: {
          region: region,
          seed: seed,
          pageNumber: pageNumber,
          errorsAmount: errorsAmount,
          perPage: perPage,
        },
      })
      .pipe(
        map(
          (response) =>
            ({
              items: response,
              page: pageNumber,
            } as UsersPaginator)
        )
      );
  }
}
