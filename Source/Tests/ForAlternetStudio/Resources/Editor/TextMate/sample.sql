select studentID, FullName, sat_score, recordUpdated
from student
where
  (
    studentID between 1 and 5
    or studentID = 8
    or FullName like '%Maximo%'
  )
and sat_score NOT in (1000, 1400);

select studentID, FullName, sat_score
from student
where 
  (
    studentID between 1 
    and 5 -- inclusive
    or studentID = 8 
    or FullName like '%Maximo%'
  ) 
  and sat_score NOT in (1000, 1400)
order by FullName DESC;

select Candidate, Election_year, sum(Total_$), count(*)
from combined_party_data
where Election_year = 2016
group by Candidate, Election_year
having count(*) > 80
order by count(*) DESC;