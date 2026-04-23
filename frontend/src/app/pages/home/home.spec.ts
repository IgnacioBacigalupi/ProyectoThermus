import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { Home } from './home';
import { ReadingService } from '../../services/reading.service';

describe('Home', () => {
  let component: Home;
  let fixture: ComponentFixture<Home>;
  const readingServiceMock = {
    getLatestReadingsByDevice: () => of([]),
    getLatestHistoryByDevice: () => of([]),
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Home],
      providers: [
        { provide: ReadingService, useValue: readingServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Home);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
