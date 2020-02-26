import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Pair } from 'src/app/pair';

@Injectable({
  providedIn: 'root'
})
export class PairsService {

  baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getAllPairs() {
    return this.http.get<Pair[]>(this.baseUrl + 'pairs');
  }

  addPair(pair: Pair) {
    return this.http.post<Pair>(this.baseUrl + 'pairs', pair);
  }
}
