import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { UsersPaginator } from '../models/models';
import { BehaviorSubject, Observable, scan, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  public paginator$!: Observable<UsersPaginator>;
  public loading$ = new BehaviorSubject(true);
  public page$ = new BehaviorSubject(0);

  public selectedRegion: number = 0;
  public selectedSeed: number = 0;
  public perPage: number = 20;

  sliderErrorsAmount: number = 0;
  inputErrorsAmount: number = 0;
  maxErrorSliderValue = 10;
  maxErrorsAmount = 1000;

  constructor(private userService: UserService) {
    this.paginator$ = this.loadUsers$();
  }

  private loadUsers$(): Observable<UsersPaginator> {
    return this.page$.pipe(
      tap(() => this.loading$.next(true)),
      switchMap((page) =>
        this.userService.getUsers(
          this.selectedRegion,
          this.selectedSeed,
          this.inputErrorsAmount,
          page,
          this.perPage
        )
      ),
      scan(this.updatePaginator, { items: [], page: 0 } as UsersPaginator),
      tap(() => this.loading$.next(false))
    );
  }

  private updatePaginator(
    accumulator: UsersPaginator,
    value: UsersPaginator
  ): UsersPaginator {
    if (value.page === 0) {
      return value;
    }

    accumulator.items.push(...value.items);
    accumulator.page = value.page;

    return accumulator;
  }

  public loadMoreUsers(paginator: UsersPaginator) {
    this.page$.next(paginator.page + 1);
  }

  errorSliderChangeHandler() {
    this.inputErrorsAmount = this.sliderErrorsAmount;
    this.regenerateTable();
  }

  errorInputChangeHandler(){
    if (this.inputErrorsAmount > this.maxErrorsAmount) {
      this.inputErrorsAmount = this.maxErrorsAmount;
    }
    this.sliderErrorsAmount = Math.floor(Math.min(this.maxErrorSliderValue, this.inputErrorsAmount));
    this.regenerateTable();
  }

  public regenerateTable(): void {
    this.page$.next(0);
  }
  
  public generateRandomSeed(): void {
    this.selectedSeed = Math.floor(Math.random() * 100000000);
    this.regenerateTable();
  }

  title = 'fakedatageneration.client';
}
