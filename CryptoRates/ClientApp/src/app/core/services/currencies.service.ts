import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Currency } from 'src/app/core/models/currency';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CurrenciesService {

  baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getAllCurrencies(): Observable<Currency[]> {
    return this.http.get<Currency[]>(this.baseUrl + 'api/currencies');
  }
}
