import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParkingSpacesComponent } from './parking-spaces';

describe('ParkingSpacesComponent', () => {
  let component: ParkingSpacesComponent;
  let fixture: ComponentFixture<ParkingSpacesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ParkingSpacesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ParkingSpacesComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
