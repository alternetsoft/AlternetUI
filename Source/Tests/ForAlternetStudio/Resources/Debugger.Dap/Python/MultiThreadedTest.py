import time
import threading
from queue import Empty, Queue
from threading import *
 
class Producer:
    def __init__(self, queue):
        self.queue = queue;
        self.a = 1
        for i in range(1, 6):
            print(f'Inserting item {i} into the queue')
            time.sleep(1)
            self.queue.put(i)
 
class Consumer:
    def __init__(self, queue):
        self.queue = queue;
        while True:
            try:
                item = self.queue.get()
            except Empty:
                continue
            else:
                print(f'Processing item {item}')
                time.sleep(2)
                self.queue.task_done()
 
def main():
    queue = Queue()
    producerThread = threading.Thread(target=Producer, args=(queue,))
    producerThread.start()
    consumerThread = threading.Thread(target=Consumer, args=(queue,))
    consumerThread.start()
    producerThread.join()
    queue.join()
 
main()
print("Finished")
