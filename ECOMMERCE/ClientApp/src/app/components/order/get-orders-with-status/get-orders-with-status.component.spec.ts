import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetOrdersWithStatusComponent } from './get-orders-with-status.component';

describe('GetOrdersWithStatusComponent', () => {
  let component: GetOrdersWithStatusComponent;
  let fixture: ComponentFixture<GetOrdersWithStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetOrdersWithStatusComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GetOrdersWithStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
