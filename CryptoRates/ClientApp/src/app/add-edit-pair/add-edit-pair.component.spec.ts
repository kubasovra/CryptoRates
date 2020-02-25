import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditPairComponent } from './add-edit-pair.component';

describe('AddEditPairComponent', () => {
  let component: AddEditPairComponent;
  let fixture: ComponentFixture<AddEditPairComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEditPairComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditPairComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
