import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Currency } from '../../currency';

@Injectable({
  providedIn: 'root'
})
export class CurrenciesService {

  baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getAllCurrencies() {
    return this.http.get<Currency[]>(this.baseUrl + 'currencies');
  }
}
