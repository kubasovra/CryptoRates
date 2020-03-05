import { Component, OnInit, Input } from '@angular/core';
import * as CanvasJS from './canvasjs.min';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {
  @Input('chartId') chartId: string;
  @Input('prices') data: number[];
  chart;

  constructor() { }
  
  ngOnInit() {

  }

  ngAfterViewInit() {
    if (this.data !== null) {
      let chartData = [];
      for (let i = 0; i < this.data.length; i = i + 10) {
        chartData.push({ y: this.data[i] });
      }
      this.chart = new CanvasJS.Chart(this.chartId, {
        zoomEnabled: true,
        axisX: {
          gridThickness: 0,
          tickLength: 0,
          lineThickness: 0,
          labelFormatter: function () {
            return " ";
          }
        },
        axisY: {
          minimum: Math.min.apply(null, this.data),
          maximum: Math.max.apply(null, this.data),
          gridThickness: 0,
          tickLength: 0,
          lineThickness: 0,
          labelFormatter: function () {
            return " ";
          }
        },
        data: [
          {
            type: "line",
            dataPoints: chartData
          }]
      });
      this.chart.render();
    }
  }

  ngOnDestroy() {
    this.chart = null;
  }
}
