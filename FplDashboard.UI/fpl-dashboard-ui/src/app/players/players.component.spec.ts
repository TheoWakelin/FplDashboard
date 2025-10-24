import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { PlayersComponent } from './players.component';
import { ApiDataService } from '../core/services/api-data.service';
import { PlayerPagedDto } from './player-paged.model';
import { PAGINATION } from '../shared/constants';

describe('PlayersComponent', () => {
  let component: PlayersComponent;
  let fixture: ComponentFixture<PlayersComponent>;
  let mockApiService: jasmine.SpyObj<ApiDataService>;

  const mockPlayers: PlayerPagedDto[] = [{
    playerId: 1, playerName: 'Test Player', teamName: 'Test Team', position: 'FWD',
    cost: 100, bonus: 5, totalPoints: 150, minutes: 900, goals: 10, assists: 5,
    cleanSheets: 0, pointsPerGame: 5.5, form: 6.0, expectedAssistsPer90: 0.3,
    expectedGoalInvolvementsPer90: 0.8, expectedGoalsPer90: 0.5, expectedGoalsConcededPer90: 0.0,
    defensiveContributionPer90: 0.0, savesPer90: 0.0, selectedByPercent: 25.5,
    valueSeason: 15.0, valueForm: 16.7, bps: 450, influence: 800.0,
    creativity: 600.0, threat: 1200.0, ictIndex: 26.0
  }];

  beforeEach(async () => {
    mockApiService = jasmine.createSpyObj('ApiDataService', ['getPagedPlayers']);
    mockApiService.getPagedPlayers.and.returnValue(of(mockPlayers));

    await TestBed.configureTestingModule({
      imports: [PlayersComponent],
      providers: [{ provide: ApiDataService, useValue: mockApiService }]
    }).compileComponents();

    fixture = TestBed.createComponent(PlayersComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load players on fetch', () => {
    component.fetchPlayers();
    expect(mockApiService.getPagedPlayers).toHaveBeenCalledWith({
      page: 1, pageSize: PAGINATION.DEFAULT_PAGE_SIZE
    });
    expect(component.players).toEqual(mockPlayers);
    expect(component.loading).toBe(false);
  });

  it('should update filter and reset page', () => {
    component.page = 5;
    component.onFilterChange({ teamIds: [1] });
    expect(component.page).toBe(1);
    expect(component.filterModel).toEqual({ teamIds: [1] });
  });

  it('should update sort and reset page', () => {
    component.page = 3;
    component.setOrder('TotalPoints', 'desc');
    expect(component.page).toBe(1);
    expect(component.orderBy).toBe('TotalPoints');
    expect(component.orderDir).toBe('desc');
  });

  it('should navigate to next page', () => {
    component.loading = false;
    component.page = 1;
    component.totalPages = 5;
    component.onPageChange(2);
    expect(component.page).toBe(2);
  });

  it('should navigate to previous page', () => {
    component.loading = false;
    component.page = 3;
    component.totalPages = 5;
    component.onPageChange(2);
    expect(component.page).toBe(2);
  });

  it('should reset filters', () => {
    component.filterModel = { teamIds: [1], minMinutes: 500 };
    component.page = 3;
    component.resetFilters();
    expect(component.filterModel).toEqual({});
    expect(component.page).toBe(1);
  });
});
