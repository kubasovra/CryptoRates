import { Component, OnInit, Input } from '@angular/core';
import * as CanvasJS from './canvasjs.min.js';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {
  @Input('prices') data: number[];

  constructor() { }
  
  ngOnInit() {
    let chart = new CanvasJS.Chart("chartContainer", {
      zoomEnabled: true,
      animationEnabled: true,
      width: 200,
      height: 50,
      axisX: {
        gridThickness: 0,
        tickLength: 0,
        lineThickness: 0,
        labelFormatter: function () {
          return " ";
        }
      },
      axisY: {
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
          dataPoints: this.data
        }]
    });

    chart.render();
  }
}
