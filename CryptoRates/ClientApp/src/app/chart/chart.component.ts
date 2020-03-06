import { Component, OnInit, Input } from '@angular/core';
import * as CanvasJS from './canvasjs.min';
import { CDK_DESCRIBEDBY_HOST_ATTRIBUTE } from '@angular/cdk/a11y';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {
  @Input('chartId') chartId: string;
  @Input('prices') rawData: number[];
  chart;

  constructor() { }
  
  ngOnInit() {

  }

  ngAfterViewInit() {
    if (this.rawData !== null) {
      let chartData = [];
      for (let i: number = 0; i < this.rawData.length; i += 10) {
        chartData.push({ y: this.rawData[i] });
      }

      //If there is only one price record (in case of pairs like BTC/BTC), then arrays are modified so that chart is a flat line 
      if (this.rawData.length === 1) {
        //Second point to make a line
        chartData.push({ y: this.rawData[0] });
        //Min and max on Y axis depend on "rawData" array, so two values are added to make a corridor for the line
        this.rawData.push(this.rawData[0] - 1, this.rawData[0] + 1);
      }

      this.chart = new CanvasJS.Chart(this.chartId, {
        zoomEnabled: true,
        axisX: {
          minimum: 0,
          maximum: chartData.length - 1,
          gridThickness: 0,
          tickLength: 0,
          lineThickness: 0,
          labelFormatter: function () {
            return " ";
          }
        },
        axisY: {
          minimum: Math.min.apply(null, this.rawData),
          maximum: Math.max.apply(null, this.rawData),
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
